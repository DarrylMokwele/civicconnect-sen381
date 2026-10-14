# Member 2 - API, security and integration evidence
Owner: Maqoba Mphelo. Date: 13 October 2026. Status: implemented, execution pending.

## Basis and scope
The Master Brief sections 9, 10, 11, 15 and 16 require controlled reviews, transparent AI use, evolving traceability and credible security/quality evidence. M1 baselines requirements/ACs; M2 establishes the modular monolith, SQL Server and interface boundaries; M3 sections 8-12 require meaningful API cases, quality gates and execution evidence. Preserve existing IDs and records.

Tests invoke the actual HTTP pipeline, model binding, controller, lifecycle policy, EF Core mappings, history and notification persistence. A separate SQLite in-memory relational database per test prevents shared-state pollution. No lifecycle or notification mocks are used. It is API integration, not deployed E2E and not a SQL Server migration/concurrency test.

## Test records
Local execution: PASS.
Date: 13 October 2026.
Environment: Windows, Visual Studio 2026, .NET 8 target.
Command: dotnet test CivicConnect/CivicConnect.sln --no-restore --logger trx --results-directory TestResults
Passed: 8.
Failed: 0.
Skipped: 0.
Evidence: TestResults/ .trx filename(s)].
GitHub CI execution: Pending.

| ID / method | Basis and risk | Preconditions/input | Expected result | Technique / interpretation |
|---|---|---|---|---|
| API-01 / ValidTransition_PersistsStatusHistoryAndNotification | FR-010 AC-010.3; NFR-005; partial FR-005 AC-005.1; R-002 | Submitted request, existing actor, Assigned target | 204; Assigned persisted; exactly one history with actor/status/time and one StatusChanged notification | State transition; proves stored feedback only, not delivered/displayed feedback or authorised staff |
| API-02 / InvalidTransition_Returns400WithoutSideEffects | FR-010 AC-010.2; R-002 | Submitted request, Closed target | 400 error; original status; zero history/notifications | Forbidden transition; no mutation for this negative path |
| API-03 / MissingRequest_Returns404WithoutSideEffects | API resource contract | Random absent request GUID, valid target/actor | 404; existing request unchanged; no history/notifications | Equivalence partition: valid-format but absent resource |
| API-04 / MissingStatus_Returns404WithoutSideEffects | API target-resource contract | Existing request, random absent status GUID | 404; no mutation | Absent target partition |
| SEC-01a / InvalidBody: {} | Input-validation risk, M2-VAL-01 | Empty object | 400; no mutation | Missing required IDs |
| SEC-01b / InvalidBody: empty GUIDs | Input-validation risk, M2-VAL-01 | Both IDs Guid.Empty | 400; no mutation | Default-value partition |
| SEC-02a / InvalidBody: invalid GUID | Input-validation contract | String that cannot parse as GUID | 400; no mutation | Invalid-type partition |
| SEC-02b / InvalidBody: malformed JSON | Input-validation contract | Unclosed JSON object | 400; no mutation | Invalid-syntax partition |
| SEC-03 / AnonymousRequest_CurrentlySucceeds_DocumentsAccessControlGap | FR-010 AC-010.1, NFR-002; M2-SEC-01/R-008 | No authentication header; existing actor ID in body | CURRENT implementation: 204 with mutation. REQUIRED security outcome after authentication integration: 401 | Security characterisation; a passing test CONFIRMS the defect and must never count as security acceptance |

The four API cases meet Member 2's planned scenario scope; they count as evidenced cases only after execution. Legitimate black-box overlap uses state transitions and equivalence partitioning. Do not claim these alone meet the team's minimum 15 meaningful tests, five unit, five black-box and two E2E cases. Untouched scaffold tests are not meaningful evidence.

## Authentication/authorisation findings
`UseAuthorization()` alone does not enforce access control. There is no registered authentication scheme, authentication middleware or controller/policy restriction. Role/User entities are not RBAC enforcement. 401/403 role acceptance tests cannot be truthfully supplied for the current implementation.

M2-SEC-01 is high severity: an anonymous caller can mutate request status. M2-SEC-02 is high severity: ChangedByUserId is client-supplied and is not tied to an authenticated identity. A valid but unrelated existing user ID can falsify audit attribution; SQLite foreign keys do not establish authorisation. A random absent actor may produce a database exception rather than a controlled client error. Empty/missing actor IDs now fail validation, but identity spoofing remains.

Required follow-up: select and record a real authentication approach with the team; validate trusted identity; derive actor ID server-side; enforce staff permission and resource scope; test anonymous 401, authenticated requester 403, permitted staff success, invalid/expired credential rejection, cross-scope denial, and actor spoofing rejection. Use real configured authentication or explicit test scheme with documented limitations. Remove/update SEC-03 characterisation when fixed. Block external release until verified.

## Other security/quality checks
- Build enables compiler/.NET analyzers and treats warnings as errors. Existing baseline warnings must be fixed rather than hidden.
- Restore audits direct/transitive NuGet dependencies; all NU1901-NU1904 findings block restore. JSON audit script additionally rejects reported vulnerabilities and incomplete top-level reports. Feed failures block verification; an absent report is not a pass.
- Secrets script scans working files and, in GitHub CI, all reachable checkout commits. It reports path/rule only. Local snapshot scan passed on 13 October; no Git history exists in the ZIP. Heuristics do not detect all tokens, encoded credentials, inaccessible branches or provider secrets. Add platform secret scanning if available and review manually.
- No actual dependency audit was executed here. Package versions remain baseline versions; do not assert they are safe. Fix audit findings through reviewed upgrades and regression execution.
- API validation added for non-empty IDs; malformed JSON/GUIDs are rejected through ApiController binding. SQL injection exposure is reduced by EF parameterisation for this path; broader injection testing is not established.
- Notification persistence happens after the status/history save. Failure can leave a committed transition with no notification. Transactions/outbox and SQL Server failure/concurrency tests remain deferred.

## Execution
From the repository root with .NET 8 SDK and NuGet access:

```powershell
dotnet restore CivicConnect/CivicConnect.sln
dotnet build CivicConnect/CivicConnect.sln --no-restore -c Release -warnaserror -p:EnableNETAnalyzers=true
dotnet test CivicConnect/CivicConnect.sln --no-build --no-restore -c Release --logger trx --results-directory TestResults
python scripts/check_secrets.py --history
```

SQL Server is not needed for these API tests. Existing production setup remains SQL Server. Run the exact CI commands for the authoritative gates; ordinary restore above is a convenient first local run. Inspect all failed cases, retain reports, link defect/requirement and fix on a reviewed branch. Record commit, run URL, environment, actual status, TRX case name and limitations in this table/RTM. Do not convert blocked to pass based on code inspection.
