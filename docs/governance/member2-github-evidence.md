# Member 2 - CI, GitHub governance and defence
Owner: Maqoba Mphelo. Status: workflow prepared; live repository controls unverified.

## CI gate
`.github/workflows/ci.yml` checks out full history, sets up .NET 8, restores with NuGet audit, builds with compiler/analyzers and warnings-as-errors, runs the whole solution including HTTP integration tests, parses a transitive vulnerability report and scans for potential secrets. Any failed step makes job `build-test-quality-security` fail; no continue-on-error is used. Reports upload even after failure when files exist. Workflow has read-only repository permissions, uses pull_request (not privileged pull_request_target), no deployment and no required application secrets.

A configured workflow is not an observed run. This ZIP has no .git metadata, PRs, reviewer records, Actions runs or branch rules. We cannot verify progressive commits, author contribution, approvals, main protection, historical secrets or controlled merges from a source snapshot. No GitHub actions were performed on behalf of the team.

## Apply safely to the existing repository
1. In the existing clone, start from the current integration baseline and create `feature/member2-api-security-ci`. Copy changed/new files from this handoff, preserving teammate changes; use CHANGES.json as the file list. Do not replace your Git history or blindly overwrite later work.
2. Review and commit genuine increments: API fixture/cases and validation; CI/security gates; evidence/RTM/register updates. Commit authorship must reflect actual work and AI assistance, not manufactured Maqoba history.
3. Push the branch and create a substantive PR with issue, requirement links, tests, risks and CI run evidence. Maqoba owns explaining/reviewing this contribution; AI-generated output is not automatically verified individual work.
4. Jonathan Rossouw and Darryl Mokwele, if Maqoba is the actual author, each review the latest changes and approve independently. Both must document what they checked, findings and corrections. If a different team member authors the PR, use the other two teammates as reviewers.
5. Configure a main ruleset/branch protection: require PR, TWO approvals, dismiss stale approvals on new commits, require latest push review where available, resolve conversations, require `build-test-quality-security`, require up-to-date branch, block force push/deletion and restrict bypass. GitHub administrators must perform this in the live repository; the workflow cannot enforce it alone. Protect integration/develop similarly if used.
6. Verify enforcement with an unapproved/failed-check test PR (do not merge it), record actual rejection and settings evidence. After two substantive reviews and all gates pass, merge through the PR; identify exact release-candidate commit/tag only after the team release review.

## Evidence to collect (not yet available)
| Claim | Required authentic evidence | Current status |
|---|---|---|
| Controlled main | Ruleset/protection settings and blocked merge example | Unverified |
| Meaningful PR | PR URL, diff, issue link, review findings/fixes | Pending |
| Two approvals | Two independent review records on latest head | Pending |
| Progressive contributions | Real commit history, author and content | Unverified |
| Passing CI | Actions run URL, exact SHA, complete logs and artifact | Pending |
| Failure control | Genuine failed gate + fix PR/run, or clearly labelled controlled gate exercise | Pending |
| No committed secrets | Scan scope/results and manual review; rotate/remove any actual leak | Snapshot heuristic passed; history unverified |
| Controlled merge/RC | Merged PR, final SHA/tag matching demo/PED | Pending |

## Individual defence - explain using actual artefacts
**What does green CI prove?** For the exact tested commit, in that runner and dependency/configuration environment, configured restore/build/tests/analyzers/security checks completed successfully. Open the run, SHA, commands and TRX evidence to support this. Required status checks plus protected-main rules control progression.

**What does it not prove?** It does not prove all requirements, deployed E2E journeys, usability, production performance, SQL Server behaviour, absence of unknown vulnerabilities/secrets or complete access control. SEC-03 can be green while confirming that anonymous mutation remains possible. Passing CI alone cannot override a high-severity known security blocker. No arbitrary coverage percentage proves correctness.

**What happens when checks fail?** The job fails and later normal steps stop; available evidence is retained by the always-run upload. Protected-main required checks prevent merge only once the team configures the rule. Record the issue/defect, inspect root cause, fix on a branch, add/retain regression coverage and rerun the full checks on the new SHA with renewed reviews. Audit feed/tool errors are blocked checks, not successful security checks. A flaky test is investigated rather than repeatedly rerun until green. Do not disable gates to hide findings.

**Trace one requirement.** FR-010 AC-010.2 -> central transition policy -> PATCH controller -> API-02 -> 400 plus unchanged persisted state/history/notifications -> actual TRX/CI run (pending). Explain why this protects lifecycle integrity and why it does not establish AC-010.1 authorisation.

**M2 consequence.** Modular monolith layering allows real API integration testing without external network services. SQL Server remains production persistence; SQLite is a test-only adapter and cannot certify SQL Server-specific migrations/locking. In-process notification persistence provides stored feedback but leaves partial-commit risk.

**Release judgement.** Member 2 contribution is prepared for review, not release-approved. Authentication/actor attribution, unexecuted .NET/CI/audit results and absent governance evidence remain blockers. Other owners must supply agreed scope, E2E, performance, staging and overall release evidence. Demonstrate only the identified final commit, not this unmerged handoff.
