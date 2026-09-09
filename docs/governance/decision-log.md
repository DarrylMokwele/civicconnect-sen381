# CivicConnect — Engineering Decision Log — Milestone 1

* **Owner:** Darryl Mokwele (Member C — Traceability, Governance & Document Control)
* **Purpose:** Record genuine M1 decisions already made, and justified deferments where evidence is not yet sufficient.
* **Updated:** references below now cite confirmed AC IDs from Member A's `acceptance-criteria.md`

| Decision ID | Type | Decision / Deferment | Rationale | Related Artefacts | Owner | Status |
|---|---|---|---|---|---|---|
| DEC-001 | Decision | Access control will be enforced as Role-Based Access Control (RBAC) across requester/staff/management boundaries, not attribute-based or a custom model. | Matches stakeholder conflict "Request Visibility vs Access Control" (stakeholder-analysis.md) and directly satisfies NFR-002 ("users shall only access functions and request information allowed for their role") and AC-006.2 / AC-003.2. | NFR-002, AC-003.2, AC-006.2, STK-001, STK-002, constraints.md §1 | Darryl Mokwele | Decided |
| DEC-002 | Deferment | In-app municipal payment/fine settlement gateway is deliberately excluded from all milestones unless separately re-scoped. | PCI-DSS compliance overhead and zero causal link to core service-tracking problem (scope_baseline.md §3). | Scope baseline, NFR-001, NFR-002 | Jonathan Rossouw | Deferred (excluded) |
| DEC-003 | Deferment | Automated ML-based departmental dispatch is deferred to Milestone 2 Architecture Baseline. | Requires training data validation and introduces non-deterministic behaviour before manual assignment (FR-009, AC-009.1–AC-009.2) is stabilised. | scope_baseline.md §2, FR-009 | Jonathan Rossouw | Deferred — revisit at M2 Architecture gate |
| DEC-004 | Deferment | External push notification channels (SMS/WhatsApp) deferred to Milestone 3; feedback (FR-005) remains in-app for M1 baseline. | Third-party gateway costs risk breaching NFR-006 (99% availability commitment only covers CivicConnect's own uptime, not third parties); network failure modes not yet analysed. | scope_baseline.md §2, FR-005, AC-005.1, NFR-006 | Jonathan Rossouw | Deferred — revisit at M3 |
| DEC-005 | Deferment | Public incident upvoting and geospatial deduplication deferred to Milestone 2 Design Baseline. | Requires spatial indexing/GIS clustering; avoids premature database schema commitment (Section 5 M1 boundaries). | scope_baseline.md §2 | Jonathan Rossouw | Deferred — revisit at M2 Design gate |
| DEC-006 | Deferment | Audit logging execution model (synchronous vs asynchronous/queued) is not decided at M1. | Synchronous writes risk breaching NFR-001 (95% of actions <3s); final approach depends on backend framework, not yet selected. Must still satisfy NFR-005 and AC-011.2 ("the system records who added it and when") regardless of execution model chosen. | RSK-02, NFR-001, NFR-005, AC-011.2, FEC-01, FEC-03 | Jonathan Rossouw | Deferred — revisit at M2 |
| DEC-007 | Deferment | Final backend technology stack, persistence tier (relational vs document store) and deployment/container target are not decided at M1. | Explicit M1 boundary (Section 5 of brief); needs FR/NFR baseline to stabilise first (now confirmed), plus data retention requirements from municipal stakeholders (FEC-02, FEC-05). | FEC-02, FEC-05, constraints.md | Darryl Mokwele | Deferred — revisit at M2 |
| DEC-008 | Decision | Management reporting (FR-013–FR-016) will read from anonymised/materialised summary views rather than querying live identity-linked transaction tables directly. | Resolves the conflict between real-time visibility, POPIA compliance and NFR-001 latency, per the ripple-effect analysis in constraints.md §2. Supports AC-016.2 ("information comes from stored CivicConnect request records") without exposing raw identity-linked data. | constraints.md §2, NFR-001, NFR-002, FR-013–016, AC-016.2 | Jonathan Rossouw | Decided |

## Highest-priority decision to defend: DEC-006 (Audit Logging Execution Model — Deferred)

**Why this one:** it sits at the intersection of a functional requirement (FR-010, FR-011), two
non-functional requirements (NFR-001 latency, NFR-005 auditability), a confirmed acceptance criterion
(AC-011.2), and the team's #2 risk (RSK-02). Choosing wrong here — locking into synchronous writes before
the backend framework is chosen — would create rework and possibly breach the latency NFR. Deferring it is
the *correct* engineering call at M1, but the team must be able to explain exactly what evidence (backend
framework choice, expected throughput) is needed before deciding, and by when (Milestone 2 Architecture
Baseline).
