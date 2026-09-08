# CivicConnect — Project Constraints & Engineering Implications (Milestone 1)
* **Lead Author:** Jonathan Rossouw (Member B — Scope, Constraints & Risk Lead)
* **Reviewers:** Maqoba Mphelo (Member A), Darryl Mokwele (Member C)
* **Governing Standards:** ISO/IEC/IEEE 29148:2018; Protection of Personal Information Act 4 of 2013

---

## 1. Constraint Baseline
CivicConnect is governed by five categorical constraints that establish the boundaries for all downstream architectural decisions (Sommerville, 2016):

| Constraint Category | Formal Constraint Statement | Downstream Engineering Implication |
|---|---|---|
| **Scope Constraint** | Milestone 1 baseline rules strictly prohibit premature construction, including production UI code, database schemas, CI pipelines, and final technology stack selections (SEN381 Teaching Team, 2026b). | Engineering artefacts must remain declarative and verifiable; architectural options must be preserved for Milestone 2 without premature technical lock-in. |
| **Schedule Constraint** | Fixed, non-negotiable delivery deadlines across academic milestone gates (M1 Baseline Foundation → M2 Architecture → M3 Construction/Verification) (SEN381 Teaching Team, 2026a). | Development work packages must decouple cross-dependencies; integration bottlenecks across team members must be eliminated through early interface contracts. |
| **Cost & Resource Constraint** | Academic capstone environment staffed by three student software engineers (Jonathan Rossouw, Maqoba Mphelo, Darryl Mokwele) with zero capital budget (SEN381 Teaching Team, 2026a). | Enterprise software licenses and paid cloud computing tiers cannot be used; architecture must rely entirely on open-source frameworks, local containerization, and free-tier runtimes. |
| **Quality & Verification Constraint** | Every functional and non-functional requirement must possess measurable, testable acceptance criteria (e.g., NFR-001: 95% < 3s latency; NFR-004: 80% unassisted usability) (ISO/IEC/IEEE, 2018). | Software components must be designed for automated test harnesses, isolated unit execution, and reproducible performance profiling. |
| **Security & Regulatory Constraint** | Service requests process citizen personal details, protected under the statutory provisions of the Protection of Personal Information Act 4 of 2013 (POPIA, 2013) and NFR-002. | Citizen Personally Identifiable Information (PII) must be logically partitioned; unauthorized access to requester contact records must be enforced by system boundaries (AC-003.2). |

---

## 2. Constraint Interaction & Ripple Effect Trade-Off Analysis
* **Primary Interaction:** **Visibility vs. Controlled Access (Conflict 2.1) × Verification Performance (NFR-001 / NFR-005) × Schedule Constraint.**
* **The Conflict:** Requesters require instantaneous visibility into submitted service requests (FR-003, FR-004), while POPIA (2013) and NFR-002 dictate that citizen identifying information must never be exposed across tenant or user boundaries. Simultaneously, Management requires aggregate operational tracking across all municipal issues (FR-013, FR-016).
* **The Downstream Ripple Effect:**
  1. Enforcing fine-grained, row-level access control on every ticket transaction prevents public caching of service request data (Bass et al., 2021).
  2. Every query execution must validate user authorization (NFR-002) and record immutable audit logs (NFR-005), which introduces CPU execution and database I/O overhead.
  3. Under peak concurrent loads, this logging and validation pipeline directly jeopardizes the sub-3-second response threshold mandated by NFR-001.
* **Engineering Resolution:** The team resolves this conflict by separating operational workflow state tracking from citizen identity records. Read-heavy management dashboards (FR-014, FR-015) operate over materialized, anonymized summary views, decoupling reporting workloads from identity tables. This preserves POPIA compliance and role isolation without imposing real-time sanitization overhead on live transactional workflows.