# CivicConnect — Forward Engineering Considerations (Milestone 1)
* **Lead Author:** Jonathan Rossouw (Member B — Scope, Constraints & Risk Lead)
* **Reviewers:** Maqoba Mphelo (Member A), Darryl Mokwele (Member C)
* **Governing Framework:** SEN381 Master Project Brief; Bass et al. (2021) Software Architecture in Practice

The following 6 architectural and operational concerns influence current baseline assumptions, risk mitigations, and requirements without committing to premature design decisions (SEN381 Teaching Team, 2026b).

---

### FEC-01: Automated Testability & Continuous Verification (Relates to: NFR-001, NFR-004, AC-010.2)
* **Engineering Owner:** Jonathan Rossouw
* **Why It Matters Now:** Acceptance criteria demand objective, repeatable verification (e.g., verifying that invalid status transitions are rejected and 95% of transactions complete in < 3s). Designing loosely coupled components now ensures services can be verified programmatically without depending on brittle UI automation (ISO/IEC/IEEE, 2018).
* **Later Decision Influenced:** The selection of unit testing frameworks, mock integration harnesses, and automated GitHub Actions CI runner merge gates during Milestone 2 and Milestone 3.
* **Missing Information / Trigger to Decide:** The definitive backend technology stack and runtime framework (intentionally deferred to Milestone 2).
* **Risk of Ignoring It:** Constructing tightly coupled, monolithic components that resist automated testing, forcing error-prone manual regression testing and risking milestone gate rejection.

---

### FEC-02: POPIA-Compliant Data Architecture & Retention (Relates to: NFR-002, STK-001, FR-008)
* **Engineering Owner:** Jonathan Rossouw (Co-reviewed with Maqoba Mphelo)
* **Why It Matters Now:** Service requests capture citizen names, residential addresses, and contact details. Under Section 14 of POPIA (2013), personal records must not be retained longer than necessary and must be safeguarded against unauthorized internal access.
* **Later Decision Influenced:** Database schema normalization, encryption-at-rest specifications, automated data scrubbing upon ticket closure (FR-012), and role-based data view definitions.
* **Missing Information / Trigger to Decide:** Formal data retention schedules from municipal stakeholders and selection of the persistence tier (relational vs. document store).
* **Risk of Ignoring It:** Persisting unencrypted citizen PII across application tables, creating statutory non-compliance liabilities and requiring expensive database schema migrations late in construction.

---

### FEC-03: Immutable Audit Trail & Observability Logging (Relates to: NFR-005, FR-010, FR-016)
* **Engineering Owner:** Jonathan Rossouw
* **Why It Matters Now:** Management oversight (STK-003) depends entirely on reliable operational auditability. Because disputes arise over municipal response intervals, lifecycle status updates cannot overwrite previous states; they must produce an append-only audit record (AC-011.2, NFR-005) (Bass et al., 2021).
* **Later Decision Influenced:** Selection between dedicated relational audit tables, event sourcing patterns, or structured telemetry frameworks (e.g., OpenTelemetry / Serilog).
* **Missing Information / Trigger to Decide:** Expected request throughput volumes and long-term disk storage quotas.
* **Risk of Ignoring It:** Inability to reconstruct ticket histories or diagnose operational bottlenecks, directly invalidating management accountability requirements (FR-016).

---

### FEC-04: Finite State Machine Engine for Lifecycle Governance (Relates to: FR-010, AC-010.2)
* **Engineering Owner:** Jonathan Rossouw
* **Why It Matters Now:** System integrity requires that service requests follow strict, deterministic lifecycles (e.g., preventing a ticket from jumping directly from "Submitted" to "Closed" without assignment). Hardcoding transitional rules via scattered conditional checks leads to logic defects and race conditions (Sommerville, 2016).
* **Later Decision Influenced:** Architectural implementation of an explicit Finite State Machine (FSM) pattern or workflow orchestration library during Milestone 2.
* **Missing Information / Trigger to Decide:** Finalized municipal status dictionary and administrative authorization matrix across all sub-categories.
* **Risk of Ignoring It:** Corrupted workflow records entering persistent storage, distorting management reporting (FR-014) and violating business process rules.

---

### FEC-05: Zero-Budget Deployment Target & Container Strategy (Relates to: Schedule, Cost Constraints, NFR-006)
* **Engineering Owner:** Jonathan Rossouw (Co-reviewed with Darryl Mokwele)
* **Why It Matters Now:** The team operates under a strict zero-budget academic constraint (SEN381 Teaching Team, 2026a). System architecture must not rely on proprietary or paid cloud tiers (e.g., AWS RDS, Azure App Service premium plans).
* **Later Decision Influenced:** Packaging modular application services using multi-container specifications (Docker Compose) and selecting free-tier hosting environments.
* **Missing Information / Trigger to Decide:** Target production operating environment and container runtime memory ceilings.
* **Risk of Ignoring It:** Developing an application that functions on an individual local workstation but fails deployment verification due to environment drift or prohibitive hosting costs.

---

### FEC-06: Asynchronous Feedback & Communication Decoupling (Relates to: FR-005, NFR-001)
* **Engineering Owner:** Jonathan Rossouw
* **Why It Matters Now:** Operational status updates require stakeholder notification (FR-005). If notification delivery is executed synchronously within the primary database update transaction, external network latency will breach the 3-second response limit mandated by NFR-001 (Bass et al., 2021).
* **Later Decision Influenced:** Implementation of an in-memory job queue or event-driven publish-subscribe pattern to handle citizen feedback out-of-process.
* **Missing Information / Trigger to Decide:** Evaluation of external communication channels (SMS, email, or push notifications) scheduled for Milestone 3.
* **Risk of Ignoring It:** System lockups and thread exhaustion during high-concurrency periods whenever external notification channels experience network delays.