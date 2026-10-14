"""Fail on any reported vulnerability or incomplete/errored audit."""
import json, sys
with open(sys.argv[1], encoding="utf-8-sig") as source:
    data = json.load(source)
if data.get("problems") or not data.get("projects"):
    sys.exit("Dependency audit incomplete: inspect feed connectivity and JSON report")
findings = []
for project in data["projects"]:
    for framework in project.get("frameworks", []):
        for kind in ("topLevelPackages", "transitivePackages"):
            for package in framework.get(kind, []):
                if package.get("vulnerabilities"):
                    findings.append(package.get("id", "unknown"))
if findings:
    sys.exit("Vulnerable dependencies: " + ", ".join(sorted(set(findings))))
print("Dependency report parsed: no reported vulnerable packages (not proof of absence).")
