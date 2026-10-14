# Technical Debt Register - M3
| ID | Compromise | Consequence | Owner / next action | Status |
|---|---|---|---|---|
| TD-M2-01 | Test-only SQLite rather than SQL Server API fixture | Does not verify SQL Server concurrency/migrations | Maqoba/team: add SQL Server staging verification | Open |
| TD-M2-02 | In-process notifications saved after status | Partial success on notification failure | Team: evaluate transaction/outbox and failure testing | Open |
| TD-M2-03 | Heuristic secrets scanner | Unknown formats/encoding can escape detection | Maqoba: supplement with platform/scanner and manual review | Open |
| TD-M2-04 | Major-version action tags; no dependency lock files | Moving action/package resolution can vary | Team: consider verified full action SHAs and reviewed lock files | Open |
