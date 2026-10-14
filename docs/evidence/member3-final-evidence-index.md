# Member 3 Final Evidence Index

## Purpose

This index maps the Member 3 implementation to the execution evidence that must be captured before the release candidate is submitted.

## Implemented controls

| Area | Implementation | Evidence to capture |
|---|---|---|
| Health | `/health/live`, `/health/ready`, `DatabaseHealthCheck` | Browser/Postman responses showing 200 |
| Observability | `RequestLoggingMiddleware`, `X-Correlation-ID` | Response header + application log |
| Release validation | `scripts/release-check.ps1` | Console output + TRX file |
| Staging | `docker-compose.staging.yml`, Dockerfile | `docker compose ps`, health checks, frontend |
| Recovery | `scripts/recovery-drill.ps1`, recovery runbook | Backup file + restore rehearsal evidence |
| Security/quality | CI, secret scan, dependency audit | GitHub Actions run + artifacts |
| E2E | `Member3EndToEndTests.cs` | Test Explorer result |
| Authentication | `AdminAuthenticationTests.cs` | Test Explorer + browser login/logout |
| Performance | `performance/CivicConnect_Performance.jmx` | JMeter summary/results |
| Frontend | `WebApi/wwwroot` | Screenshots of Citizen, Staff and Admin flows |

## Execution order

1. Rebuild solution in Release configuration.
2. Run all automated tests.
3. Run the WebApi locally and verify Citizen/Staff/Admin flows.
4. Verify `/health/live` and `/health/ready`.
5. Start Docker staging.
6. Run `scripts/staging-verify.ps1`.
7. Run the recovery drill and complete a controlled restore rehearsal.
8. Run the JMeter plan with the API available.
9. Export/capture all evidence.
10. Update PED v3.0, RTM and registers with actual results.

## Evidence rule

Code, scripts and documentation demonstrate that a control is implemented. They do not prove that it was executed successfully. Final submission evidence must contain actual results from the release candidate.
