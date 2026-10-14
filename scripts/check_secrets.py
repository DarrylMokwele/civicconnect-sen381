"""Small heuristic scanner. Findings expose paths/rules only, never secret values."""
import pathlib, re, subprocess, sys
root = pathlib.Path(__file__).resolve().parents[1]
rules = {
    "private-key": re.compile(r"-----BEGIN (?:RSA |EC |OPENSSH )?PRIVATE KEY-----"),
    "github-token": re.compile(r"(?:gh[pousr]_[A-Za-z0-9]{20,}|github_pat_[A-Za-z0-9_]{20,})"),
    "aws-key": re.compile(r"AKIA[A-Z0-9]{16}"),
    "assigned-password": re.compile(r"(?i)(?:password|pwd|client_secret|api_key)\s*[=:]\s*[\"']?[^\s;\"'<>]{8,}"),
}
def check(name, content):
    # The scanner's own pattern definitions are not credentials.
    if name == "scripts/check_secrets.py": return []
    # Templates and parameterised deployment configuration intentionally contain
    # placeholders, not secret values. Real .env files remain blocked by the
    # sensitive-file rule below and are ignored by Git.
    if name in (".env.example",) or name.endswith(".env.example"):
        return []
    issues = []
    for key, rule in rules.items():
        match = rule.search(content)
        if not match:
            continue
        # A deployment placeholder such as Password=${SA_PASSWORD} is safe:
        # the secret must be injected by the environment at runtime.
        if key == "assigned-password" and "${" in match.group(0):
            continue
        issues.append(name + ": " + key)
    if pathlib.PurePosixPath(name).name in (".env", "appsettings.Production.json"):
        issues.append(name + ": sensitive configuration file requires removal/review")
    return issues
issues = []
for path in root.rglob("*"):
    if not path.is_file() or any(x in path.parts for x in (".git", "bin", "obj")): continue
    issues.extend(check(path.relative_to(root).as_posix(), path.read_text(errors="ignore")))
if "--history" in sys.argv:
    result = subprocess.run(["git", "rev-list", "--all"], cwd=root, capture_output=True, text=True)
    if result.returncode: sys.exit("History scan blocked: run inside the controlled Git checkout")
    for commit in result.stdout.splitlines():
        names = subprocess.check_output(["git", "ls-tree", "-r", "--name-only", commit], cwd=root, text=True)
        for name in names.splitlines():
            content = subprocess.check_output(["git", "show", commit + ":" + name], cwd=root).decode(errors="ignore")
            issues.extend(check(name, content))
if issues: sys.exit("Potential secret findings (human review required):\n" + "\n".join(sorted(set(issues))))
print("Heuristic scan passed for selected scope; unrecognised secrets may remain.")
