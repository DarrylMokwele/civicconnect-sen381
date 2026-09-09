# CivicConnect — Initial Risk Register (Milestone 1)
* **Lead Author:** Jonathan Rossouw (Member B — Scope, Constraints & Risk Lead)
* **Governance Lead:** Darryl Mokwele (Member C)
* **Requirements Lead:** Maqoba Mphelo (Member A)
* **Methodological Reference:** ISO 31000:2018 Risk Management Guidelines

---

## 1. Risk Register

| Risk ID | Risk Description & Source | Prob (1-5) | Imp (1-5) | Severity (P x I) | Mitigation Strategy | Contingency Plan | Owner | Status |
|---|---|---|---|---|---|---|---|---|
| **RSK-01** | **Scope Creep & Premature Implementation:** Team implements complex non-baseline features (e.g., mapping APIs, ML routing) prior to requirements freeze (FR-009; SEN381 Teaching Team, 2026b). | 4 | 4 | **16 (High)** | Enforce branch protection on GitHub requiring two peer code reviews prior to merging; immediately reject unapproved feature branches (Sommerville, 2016). | Revert unauthorized commits to main; reallocate development capacity to unfulfilled baseline verification items. | **Jonathan Rossouw** | Active |
| **RSK-02** | **NFR-001 Latency Breach via Synchronous Audit Overhead:** Synchronous writes to persistent storage for every status transition (FR-010, NFR-005) cause response times to exceed the 3-second limit (NFR-001) (Bass et al., 2021). | 3 | 4 | **12 (High)** | Design audit logging to execute asynchronously via background messaging queues or lightweight persistence interceptors in M2. | Implement composite database indexing on foreign keys; profile bottleneck queries; offload historical audit reports to offline batch jobs. | **Jonathan Rossouw** | Active |
| **RSK-03** | **Unauthorised Citizen PII Disclosure:** Flawed authorization logic exposes private citizen identifying information to other requesters or unauthorized staff (NFR-002, AC-003.2; POPIA, 2013). | 2 | 5 | **10 (Medium)** | Construct automated security test suites executing negative authorization assertions (verifying HTTP 403 Forbidden across user contexts). | Immediately revoke active session tokens; purge sensitive citizen identifiers from public query response payloads. | **Maqoba Mphelo** | Active |
| **RSK-04** | **Milestone Gate Traceability Breakdown:** Baselined functional requirements (FR-001 to FR-016) lack verified bidirectional links to testable acceptance criteria in the RTM (ISO/IEC/IEEE, 2018). | 2 | 4 | **8 (Medium)** | Conduct formal bidirectional traceability walkthroughs during document integration, verifying that every requirement maps to a unique AC ID. | Convene an emergency baseline remediation session prior to the individual oral defence gate. | **Darryl Mokwele** | Active |
| **RSK-05** | **Zero-Budget Infrastructure Resource Exhaustion:** Team consumes free-tier cloud credits or exceeds memory limits during integration performance testing (NFR-001, NFR-006; SEN381 Teaching Team, 2026a). | 3 | 2 | **6 (Low)** | Mandate Docker containerization for local execution; build automated testing environments that do not rely on paid external cloud hosting. | Revert deployment topology to local self-hosted container environments for evaluation. | **Jonathan Rossouw** | Active |

---

## 2. Deep-Dive Defence of Highest Priority Risk: RSK-01 (Scope Creep & Premature Construction)

* **Justification and Lifecycle Impact (Jonathan Rossouw Defence):**
  In software engineering baselines, premature implementation represents the primary driver of project failure and technical debt (Sommerville, 2016). In the context of CivicConnect, the natural inclination is to immediately code mapping interfaces, third-party citizen messaging, or machine learning dispatchers to address operational bottlenecks (SEN381 Teaching Team, 2026b). 
  
  Executing premature construction before functional requirements (FR-001 through FR-016) and non-functional requirements (NFR-001 through NFR-006) are baselined produces unverified code, fragments development effort, and breaches Section 5 of the Milestone 1 brief: *"Thinking ahead is not premature implementation"* (SEN381 Teaching Team, 2026b). Mitigating RSK-01 via strict branch protection and two-reviewer pull request approvals guarantees that engineering effort remains focused on delivering a traceable, verifiable baseline.