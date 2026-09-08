# Initial Risk Register

| Risk ID | Description | Prob (1-5) | Imp (1-5) | Priority | Mitigation Strategy | Contingency Plan | Owner | Status |
|---|---|---|---|---|---|---|---|---|
| **RSK-01** | Scope Creep & Gold-Plating during M2 design shifts focus away from core ticket resolution workflows. | 4 | 4 | **High** | Enforce formal change-control review on GitHub PRs; verify every proposed feature against M1 Scope Baseline. | Defer unapproved features directly to the deferred scope backlog; strip non-essential UI. | Person 2 | Active |
| **RSK-02** | Milestone 1 Baseline Gate rejection due to missing requirement-to-acceptance-criteria traceability. | 2 | 5 | **High** | Conduct a full team RTM walk-through prior to final submission. | Convene emergency refactoring session to repair orphaned requirements before defence. | Person 3 | Active |
| **RSK-03** | Cloud hosting or external map API budget overrun beyond academic zero-cost limit. | 3 | 4 | **Medium** | Standardize on open-source solutions (e.g., Leaflet / OpenStreetMap) and local containerization during development. | Fall back to mock map services and local containerized databases. | Person 2 | Active |
| **RSK-04** | Asynchronous team integration failure resulting in last-minute unreviewed PR merges. | 3 | 4 | **Medium** | Enforce protected `main` branch rules requiring two peer approvals per PR. | Revert broken merges immediately; schedule synchronous pairing sessions. | Person 1 | Active |

## Top Risk Defence: RSK-01 (Scope Creep)
* **Defence:** In civic engagement systems, it is common to attempt building advanced features early (e.g., AI issue triage, complex GIS layers, automated municipality dispatch). If unconstrained, development effort fragments, resulting in half-implemented modules, failed acceptance criteria, and missed deadlines. RSK-01 is prioritized as the most critical risk because preserving system boundaries is prerequisite to completing a functioning, verifiable baseline.