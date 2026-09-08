# CivicConnect — Scope Baseline (Milestone 1)
* **Lead Author:** Jonathan Rossouw (Member B — Scope, Constraints & Risk Lead)
* **Requirements Basis:** Maqoba Mphelo (Member A — Problem, Stakeholders & Requirements Lead)
* **Document & Traceability Control:** Darryl Mokwele (Member C — Traceability, Governance & Document Control Lead)
* **Governing Standards:** ISO/IEC/IEEE 29148:2018; SEN381 Master Project Brief; Milestone 1 Specification

---

## 1. In-Scope (Milestone 1 Commitment)
The following functional and non-functional commitments establish the formal engineering baseline for CivicConnect, derived directly from the elicitation baseline established by Member A (ISO/IEC/IEEE, 2018; SEN381 Teaching Team, 2026b):

* **Citizen Service Request Submission & Categorisation:** Direct submission of municipal service requests capturing structured incident descriptions, location references, and controlled category assignment (FR-001, FR-002, NFR-003, AC-001, AC-002).
* **Requester Tracking & Feedback:** A secure citizen portal providing private tracking of submitted requests, historical ticket review, and status update notifications (FR-003, FR-004, FR-005, AC-003, AC-004, AC-005).
* **Operational Request Management:** An administrative workspace enabling municipal staff to search, multi-attribute filter, sort, assign or accept operational responsibility, and record resolution details (FR-006, FR-007, FR-008, FR-009, FR-011, FR-012).
* **Controlled Lifecycle State Transitions:** Strict lifecycle workflow governance (Submitted → Assigned → In-Progress → Resolved → Closed) preventing unauthorized state jumps or invalid state bypasses (FR-010, AC-010.2).
* **Management Oversight & Aggregated Analytics:** Operational reporting dashboards providing visibility into open, overdue, resolved, and closed counts, with multi-dimensional filtering by service category and status (FR-013, FR-014, FR-015, FR-016).
* **Role-Based Access Control & Immutable Audit Logging:** Enforcement of Role-Based Access Control (RBAC) across requester, staff, and management boundaries, with non-repudiable audit logging recording the actor, timestamp, and target state for every transition (NFR-002, NFR-005).

---

## 2. Deferred Scope (Future Milestones)
Capabilities recognized as valuable to long-term operations, but intentionally deferred past Milestone 1 to preserve baseline integrity and prevent gold-plating (Sommerville, 2016):

| Deferred Capability | Architectural & Traceability Justification | Target Lifecycle Gate |
|---|---|---|
| **Automated Departmental Dispatch via Machine Learning** | Auto-routing requests based on natural language classification requires training dataset validation and introduces non-deterministic model dependencies before baseline manual assignment mechanisms (FR-009) are stabilized (ISO/IEC/IEEE, 2018). | Milestone 2 Architecture Baseline |
| **External Push Notification Engine (SMS / WhatsApp)** | While stakeholder feedback is mandatory (FR-005), external telecommunication gateways introduce third-party subscription costs and network failure modes that breach the zero-budget constraint. Feedback remains in-app for baseline verification. | Milestone 3 Construction & Verification |
| **Public Incident Upvoting & Geospatial Deduplication** | Reduces duplicate logging in dense municipal areas, but requires complex spatial indexing and GIS clustering engines. Deferred to avoid premature database schema commitments in M1 (SEN381 Teaching Team, 2026b). | Milestone 2 Design Baseline |

---

## 3. Deliberate Scope Exclusion & Engineering Defence

* **Deliberately Excluded Capability:** **In-App Municipal Payment and Fine Settlement Gateway.**
* **Engineering Defence (Jonathan Rossouw):**
  1. **Functional Orthogonality:** As established by the business problem definition, CivicConnect is engineered to resolve service delivery communication failures, unassigned operational tasks, and lack of ticket traceability (SEN381 Teaching Team, 2026b). Monetary transactions bear no causal relationship to logging, dispatching, or verifying municipal defect repairs.
  2. **Regulatory & Compliance Overhead:** Incorporating monetary transactions subjects the project to the Payment Card Industry Data Security Standard (PCI-DSS), cryptographic key escrow liabilities, and complex financial auditing rules (PCI Security Standards Council, 2022). This introduces severe risk without advancing core service tracking.
  3. **Preservation of Core Quality Attributes:** Introducing financial settlement would divert engineering resources away from sub-3-second transaction latencies (NFR-001), strict access control (NFR-002), and complete auditability (NFR-005).