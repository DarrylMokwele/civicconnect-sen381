# CivicConnect — Project Engineering Document (PED) v1.0
### Milestone 1 — Engineering Foundation & Requirements Baseline

* **Team:** Maqoba Mphelo (A — Problem, Stakeholders & Requirements), Jonathan Rossouw (B — Scope,
  Constraints & Risk), Darryl Mokwele (C — Traceability, Governance & Document Control)
* **Version:** v1.0 — [insert date]
* **Status:** Ready for baseline sign-off — requirements register gap resolved (see `00-gap-flag-RESOLVED.md`)

---

## How this document is organised

This PED integrates the controlled artefacts produced across the team into one coherent baseline. Each
section below points to the authoritative source file in the repository rather than duplicating content,
so there is a single place each artefact is maintained.

## 1. Problem & Business Need
See: `docs/requirements/problem-and-business-need.md`

CivicConnect consolidates fragmented community service-request channels (email, phone, WhatsApp,
spreadsheets, paper) into one controlled system, giving requesters visibility, staff clear ownership, and
management reliable oversight of service delivery.

## 2. Stakeholder Analysis
See: `docs/requirements/stakeholder-analysis.md`

Three stakeholder groups identified — STK-001 Requesters, STK-002 Operational Staff, STK-003 Management/
Oversight — with three documented conflicts (quick submission vs. sufficient information; visibility vs.
access control; management information vs. staff workload), each with a stated requirement effect.

## 3. Scope Baseline
See: `docs/scope_baseline.md`

In-scope commitments, three deferred capabilities with lifecycle-gate targets, and one deliberately excluded
capability (in-app payment/fine settlement) with a three-part engineering defence.

## 4. Requirements & Acceptance Criteria
See: `docs/requirements/functional-requirements.md`, `non-functional-requirements.md`, `acceptance-criteria.md`

16 functional requirements (FR-001–FR-016) and 6 non-functional requirements (NFR-001–NFR-006), each sourced
to a stakeholder group, prioritised (MoSCoW), and paired with measurable, testable acceptance criteria
(e.g. AC-001.1–AC-001.4, AC-010.1–AC-010.3). **Confirmed and owned by Member A.**

## 5. Constraints
See: `docs/constraints.md`

Five constraint categories (scope, schedule, cost/resource, quality/verification, security/regulatory) with
a worked ripple-effect trade-off analysis connecting visibility, access control and latency.

## 6. Initial RTM
See: `docs/rtm.md`

Full source → requirement → acceptance-criteria table built directly from Member A's confirmed register,
with FR-010 traced end-to-end from stakeholder need through to acceptance criteria and the forward-engineering
item it informs.

## 7. Initial Risk Register
See: `docs/risk_register.md`

Five risks (RSK-01 to RSK-05) with probability/impact scoring; RSK-01 (Scope Creep & Premature
Implementation) carries the highest severity and is defended in-document.

## 8. Forward Engineering Considerations
See: `docs/forward_engineering.md`

Six forward-looking concerns (FEC-01 to FEC-06) covering testability, POPIA-compliant data architecture,
audit trail design, lifecycle state-machine design, zero-budget deployment, and asynchronous notification
handling — each stating why it matters now, what it influences later, what's missing, and the risk of
ignoring it.

## 9. Engineering Decision Log
See: `docs/decision-log.md`

Eight entries: two firm M1 decisions (RBAC model; anonymised reporting views) and six justified deferments,
each tied to the confirmed requirement, risk or constraint that drove it, and cross-referenced to real AC
IDs.

## 10. GitHub & Team Governance
See: `CODEOWNERS`, repository Settings (branch protection), Issues/Projects board

Protected `main`, two-reviewer approval enforced via branch protection rules, CODEOWNERS routing reviews by
artefact area, and issue-tracked work packages for every substantive artefact change.

## 11. AI Usage Register
See: `docs/ai-usage-register.md`

Live register of AI-assisted contributions with human verification recorded against each entry, including
the reconstruction-then-verification cycle used to resolve the requirements register gap.

## 12. Baseline Sign-Off
See: `docs/baseline-signoff.md`

Team review checklist and formal gate decision — ready to be completed now that the requirements register
gap is closed.

---

## Resolved item

The requirements register referenced throughout Sections 4 and 6 was initially a Member C reconstruction
pending Member A's authorship. Member A has since published and confirmed the full FR/NFR/AC set with no ID
conflicts against Member B's downstream references. See `00-gap-flag-RESOLVED.md` for the full resolution
record. PED v1.0 is now ready for team review and baseline sign-off.
