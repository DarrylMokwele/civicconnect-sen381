# CivicConnect — Requirements Traceability Matrix (RTM) — Milestone 1

* **Owner:** Darryl Mokwele (Member C — Traceability, Governance & Document Control)
* **Status:** Baselined for M1 — will extend to Design, Test and Release evidence from Milestone 2 onward
* **Built from:** `docs/requirements/functional-requirements.md`, `non-functional-requirements.md`,
  `acceptance-criteria.md` and `traceability-handover.md` (Member A, confirmed)

## Traceability Table

| Source | Requirement ID | Requirement Summary | Priority | Acceptance Criteria | Design Evidence (M2+) | Test Evidence (M3+) | Status |
|---|---|---|---|---|---|---|---|
| STK-001 | FR-001 | Submit a service request with required information | Must | AC-001.1–AC-001.4 | — | — | Baselined |
| STK-001 | FR-002 | Select a category for a service request | Must | AC-002.1–AC-002.2 | — | — | Baselined |
| STK-001 | FR-003 | View current status of own requests | Must | AC-003.1–AC-003.2 | — | — | Baselined |
| STK-001 | FR-004 | View previous requests | Must | AC-004.1–AC-004.2 | — | — | Baselined |
| STK-001 | FR-005 | Receive feedback on submit/reject/update/complete | Must | AC-005.1 | — | — | Baselined |
| STK-002 | FR-006 | Authorised staff view requests relevant to them | Must | AC-006.1–AC-006.2 | — | — | Baselined |
| STK-002 | FR-007 | Search, filter and sort requests | Must | AC-007.1–AC-007.3 | — | — | Baselined |
| STK-002 | FR-008 | View request details | Must | AC-008.1 | — | — | Baselined |
| STK-002 | FR-009 | Take/be assigned responsibility for a request | Must | AC-009.1–AC-009.2 | — | — | Baselined |
| STK-002 | FR-010 | Update request status via allowed transitions | Must | AC-010.1–AC-010.3 | — | — | Baselined |
| STK-002 | FR-011 | Record actions/comments/resolution details | Must | AC-011.1–AC-011.2 | — | — | Baselined |
| STK-002 | FR-012 | Resolve or close a request | Must | AC-012.1–AC-012.2 | — | — | Baselined |
| STK-003 | FR-013 | View service request activity | Must | AC-013.1 | — | — | Baselined |
| STK-003 | FR-014 | Identify open/overdue/resolved/closed requests | Must | AC-014.1–AC-014.3 | — | — | Baselined |
| STK-003 | FR-015 | View request info by category and status | Must | AC-015.1–AC-015.2 | — | — | Baselined |
| STK-003 | FR-016 | Provide info for performance/accountability checks | Must | AC-016.1–AC-016.2 | — | — | Baselined |
| All stakeholders | NFR-001 | ≥95% of normal actions complete within 3 seconds | Must | AC per NFR-001 | — | — | Baselined |
| All stakeholders | NFR-002 | Users access only functions/data allowed for their role | Must | AC per NFR-002 | — | — | Baselined |
| STK-001 | NFR-003 | Request stored before confirming success | Must | AC per NFR-003 | — | — | Baselined |
| STK-001 | NFR-004 | ≥4/5 test users submit a valid request unassisted | Should | AC per NFR-004 | — | — | Baselined |
| STK-002 / STK-003 | NFR-005 | Status change records user, status and time | Must | AC per NFR-005 | — | — | Baselined |
| All stakeholders | NFR-006 | ≥99% availability during agreed operational hours | Should | AC per NFR-006 | — | — | Baselined |

## End-to-End Trace (worked example for the defence)

**STK-002 (Operational Staff)** need to *"take responsibility, update and resolve requests"*
(`traceability-handover.md`) →
**FR-010**: *"The system shall allow authorised staff to update a request's status using allowed status
changes"* (`functional-requirements.md`) →
**AC-010.1 / AC-010.2 / AC-010.3**: *"Only authorised staff can change status; invalid status changes are
rejected; a valid status change is saved and displayed"* (`acceptance-criteria.md`) →
**Design evidence (M2, planned)**: a Finite State Machine governing allowed status transitions — already
anticipated in Forward Engineering Consideration FEC-04 (lifecycle state-machine design) →
**Test evidence (M3, planned)**: not yet available — structure is in place to add it once verification
artefacts exist.

This is a complete, ID-consistent chain from stakeholder need through to a testable requirement, with a
clear, honest stopping point at the current lifecycle stage (no premature design/test claims at M1).

## Notes

- All FR/NFR/AC IDs are used exactly as published by Member A — no renumbering was needed once the
  requirements register was finalised.
- Design and Test Evidence columns are intentionally empty at M1 (correct per the M1 boundary in Section 5
  of the brief) — the column structure exists now so M2/M3 evidence can be added without restructuring the
  RTM.
