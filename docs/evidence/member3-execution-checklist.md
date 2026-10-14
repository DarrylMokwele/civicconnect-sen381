# Member 3 Execution Evidence Checklist

This file separates implemented controls from evidence that must be executed against the team's release candidate.

## Release candidate
- [ ] `dotnet build CivicConnect/CivicConnect.sln -c Release -warnaserror`
- [ ] `dotnet test CivicConnect/CivicConnect.sln -c Release`
- [ ] Capture the final Test Explorer result.

## Application
- [ ] Frontend loads at `/`.
- [ ] Citizen can submit a request without logging in.
- [ ] Staff can sign in and sign out.
- [ ] Admin can sign in and sign out.
- [ ] Admin can change a permitted status.
- [ ] Admin assignment changes persist.
- [ ] StatusHistory and Notifications are created after a successful status change.

## Operational readiness
- [ ] `/health/live` returns 200.
- [ ] `/health/ready` returns 200 with database available.
- [ ] `X-Correlation-ID` is visible in a response.
- [ ] Staging starts with `scripts/staging-start.ps1`.
- [ ] `scripts/staging-verify.ps1` passes.

## Recovery
- [ ] `scripts/recovery-drill.ps1` creates a valid backup.
- [ ] Perform a controlled restore rehearsal using the recovery runbook.
- [ ] Capture before/after evidence and record the result.

## Performance
- [ ] Run `performance/CivicConnect_Performance.jmx` with JMeter.
- [ ] Record request count, error rate, average, median/p95, min/max and throughput.
- [ ] State workload and environment in the conclusion.

## Security and quality
- [ ] Run `scripts/check_secrets.py --history`.
- [ ] Run dependency audit.
- [ ] Review CI artifact evidence.

Do not mark an item complete merely because the code or script exists. Mark it complete only after the execution evidence has been captured.
