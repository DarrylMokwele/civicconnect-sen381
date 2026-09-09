# CivicConnect — AI Usage Register — Milestone 1

* **Owner:** Darryl Mokwele (Member C — Traceability, Governance & Document Control)
* **Purpose:** Record material AI-assisted work and the human verification applied. AI output is not a
  source and does not replace credible referencing or human verification (Section 9.1, M1 brief).

| Entry ID | Date | Team Member | Artefact | AI Tool Used | What AI Was Asked | What Was Verified / Changed / Rejected | Verified By |
|---|---|---|---|---|---|---|---|
| AI-001 | [insert date] | [Member A/B/C] | [e.g. stakeholder-analysis.md] | [e.g. ChatGPT/Claude] | [e.g. "Suggest wording for stakeholder conflict descriptions"] | [e.g. "Rejected generic wording; rewrote with CivicConnect-specific detail; verified stakeholder list matches problem statement"] | [Name] |
| AI-002 | [insert date] | [Member A/B/C] | [e.g. risk_register.md] | [tool] | [e.g. "Draft initial risk descriptions from constraints"] | [e.g. "Changed probability/impact scores after team discussion; verified mitigation strategies are project-specific, not generic"] | [Name] |
| AI-003 | [insert date] | Darryl Mokwele | rtm.md, decision-log.md, PED.md (initial draft) | Claude | "Reconstruct a requirements register and RTM structure from cross-references found in already-written scope, risk and constraints documents, ahead of Member A's formal requirements register." | Flagged explicitly as DRAFT with a gap-flag note; every reconstructed FR/NFR wording and TBD acceptance criterion was withheld from being treated as final until Member A confirmed it. | Darryl Mokwele |
| AI-004 | [insert date] | Maqoba Mphelo | functional-requirements.md, non-functional-requirements.md, acceptance-criteria.md, traceability-handover.md | [tool, if used] | [e.g. "Help structure NFR targets as measurable statements"] | [e.g. "Adjusted NFR-004 target from a vague usability goal to a concrete 4/5 users test; confirmed all IDs matched what Jonathan's docs already referenced before publishing"] | Maqoba Mphelo |
| AI-005 | [insert date] | Darryl Mokwele | rtm.md, decision-log.md, PED.md (finalised) | Claude | "Update the RTM, decision log and PED against Member A's now-confirmed requirements register; verify no ID or AC reference in downstream docs was left unresolved." | Cross-checked every FR/NFR/AC ID cited in Jonathan's scope/risk/constraints/forward-engineering docs against Maqoba's final register; corrected DEC-001's AC reference from an invented interpretation to the actual confirmed AC-003.2/AC-006.2 wording. | Darryl Mokwele |

## Worked example (kept from M1 — real instance)

**Entry AI-003 → AI-005 — RTM and Decision Log drafting and finalisation**
- **What AI produced initially:** A reconstructed requirements register and RTM based purely on
  cross-references found in already-written scope, risk and constraints documents, before Member A's
  formal requirements register existed.
- **What the human (Member C) verified:** Confirmed the IDs matched exactly what appeared in
  `scope_baseline.md`, `risk_register.md`, `forward_engineering.md` and `constraints.md`; explicitly did
  **not** treat AI-inferred requirement wording as authoritative — flagged it as a draft requiring Member
  A's sign-off before merge.
- **What was rejected/changed on finalisation:** Once Member A's confirmed register arrived, Member C
  re-checked every AC reference in the RTM and decision log against the real AC IDs (e.g. corrected the
  DEC-001 rationale to cite the actual AC-003.2 and AC-006.2 wording rather than an earlier guess) and
  removed all "TBD"/"DRAFT" markers.
- **Why this matters for accountability:** demonstrates the team used AI to accelerate structuring and
  cross-checking work, not to replace engineering judgement or requirements ownership — directly answering
  Indicative Defence Question 9 ("Show one AI-assisted contribution. What did you verify, reject or
  change?").

> Replace remaining placeholder rows (AI-001, AI-002, AI-004) with the team's actual usage details as
> confirmed by Members A and B — this register must stay live and progressively updated.
