# M3 Defect Log & Retest Evidence

| Defect ID | Severity | Affected Requirement | Defect Description | Root Cause | Resolution & Retest Evidence | Status |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- |
| **DEF-001** | High | `FR-010` / `AC-010.2` | Transition from 'Assigned' to 'Closed' allowed when status ID was supplied directly. | Domain validation bypassed in service layer direct update. | Enforced `StatusTransitionPolicy.CanTransition()` check in Application command handler. Retested and passed via `StatusTransitionPolicyTests`. | CLOSED |
| **DEF-002** | Medium | `NFR-005` | `dbo.StatusHistories` accepted NULL for `ChangedByUserId`. | Foreign key column configured as nullable in EF Core entity configuration. | Added `.IsRequired()` constraint to `CivicConnectDbContext` model builder and generated EF migration. Verified in SQL Server table. | CLOSED |
| **DEF-003** | Low | `FR-001` | Submission accepted 1000+ whitespace characters in `Description`. | Validation only checked `.Length` without trimming. | Added `.Trim()` validation check in command validator. Verified via Boundary Value test suite. | CLOSED |