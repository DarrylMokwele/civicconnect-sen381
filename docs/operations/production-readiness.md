# CivicConnect — Member 3 Production Readiness Implementation

## Purpose

This document records the Member 3 implementation for Milestone 3 production-readiness and operational evidence. It does not claim that CivicConnect is production-certified. It provides a repeatable local staging arrangement, operational health checks, request observability, migration controls, release checks and recovery guidance.

## Implemented controls

| Concern | Implementation | Evidence to collect |
|---|---|---|
| Environment parity | Dockerfile + `docker-compose.staging.yml` | Container startup, Swagger and health screenshots |
| Secrets/configuration | Environment-variable connection string and `.env.example`; real `.env` ignored | Repository check showing no real `.env` committed |
| Release/deployment | Repeatable container build and `scripts/release-check.ps1` | Successful release-check output and image build |
| Database migration control | `Database:ApplyMigrations` is false by default and explicitly enabled by staging configuration | Startup logs + database schema after staging startup |
| Seed data control | `Database:SeedData` is false by default and explicitly enabled for local staging | Seeded test request visible in database |
| Health/readiness | `/health/live` and `/health/ready` | HTTP 200 responses and failure behaviour when DB is unavailable |
| Observability | `X-Correlation-ID` plus method/path/status/duration logging | API log containing correlation ID and elapsed time |
| Failure handling | Generic unhandled errors return a safe 500 ProblemDetails response | Negative test and response screenshot |
| Recovery | Rollback/roll-forward and SQL backup/restore guidance | Runbook evidence from a rehearsal |

## Environment strategy

### Development

Uses the developer's configured SQL Server/LocalDB connection. Development seeding is not automatically forced by code; database lifecycle behaviour is controlled by configuration.

### Staging/test

The supplied Docker Compose file creates an isolated SQL Server container and CivicConnect API container. The API uses environment variables for the database connection and explicitly enables migrations and seed data for this local staging arrangement.

### Production

Production values should override the defaults through the deployment platform's secret/configuration store. `Database:ApplyMigrations` and `Database:SeedData` are false by default so a production process does not unexpectedly mutate schema or insert development data.

## Health model

- `/health/live`: process-level liveness. It does not depend on SQL Server.
- `/health/ready`: application readiness including database connectivity.

A service can therefore be alive while not ready to serve database-dependent traffic.

## Observability model

Each request receives an `X-Correlation-ID`. A valid caller-supplied UUID is preserved; otherwise the API creates one. Request logs contain only operational metadata:

- HTTP method;
- request path;
- response status;
- elapsed milliseconds;
- correlation ID.

Request bodies, authorization headers and other potentially sensitive request data are deliberately not logged by this middleware.

## Release gate

Run:

```powershell
./scripts/release-check.ps1
```

The script performs restore, Release build with warnings treated as errors, and the full automated test suite. The existing Member 2 CI workflow remains the repository-level build/test/quality/security gate.

## Current evidence status

Implementation is included in the Member 3 handoff. Actual staging execution, screenshots, recovery rehearsal and final release approval must be performed against the team's selected release candidate and recorded in the final PED/RTM.
