# CivicConnect Recovery Runbook

## Scope

Use this runbook for the local staging/release-candidate environment. Production recovery must be adapted to the actual hosting and database platform selected by the team.

## Before deployment

1. Confirm the release candidate commit/tag.
2. Run `scripts/release-check.ps1`.
3. Confirm CI is green.
4. Confirm no real secrets are committed.
5. Confirm a database backup exists before destructive schema/data work.
6. Confirm the migration change is compatible with the application version being deployed.

## Deployment failure

1. Stop promotion of the failed release.
2. Capture the deployment error and correlation IDs from affected requests.
3. Check `/health/live` and `/health/ready`.
4. Check application logs for startup/migration failures.
5. If the new application is incompatible but data remains valid, roll forward to a corrected build.
6. If a rollback is required, first verify database compatibility with the previous application version.
7. Restore the database only when data corruption/loss requires it and the restore point is approved.
8. Re-run health and critical API checks.

## Database migration safety

CivicConnect does not enable automatic migrations by default. The staging compose configuration explicitly enables migrations so that the environment is repeatable. Production deployment should use an approved migration process and compatibility checks.

## Local staging stop

```powershell
./scripts/staging-stop.ps1
```

By default, `docker compose down` does not delete the named SQL data volume.

## Local staging rebuild

```powershell
./scripts/staging-start.ps1
```

If the database schema or image must be recreated, make that a deliberate operation and preserve a backup when data matters.

## Recovery evidence to record

- incident/release identifier;
- failed version;
- detected time;
- health status;
- action taken;
- whether rollback or roll-forward was used;
- database migration state;
- recovery completion time;
- data-loss assessment;
- residual risk.
