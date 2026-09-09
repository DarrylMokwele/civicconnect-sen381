# ✅ RESOLVED — Requirements Register Gap

**Raised by:** Darryl Mokwele (Member C) — [original date]
**Resolved by:** Maqoba Mphelo (Member A) — [insert date A pushed the fix]
**Status:** Closed

## Original issue

Member B's scope, risk and forward-engineering documents referenced FR-001–FR-016, NFR-001–NFR-006 and
several AC IDs before Member A had published a requirements document defining them.

## Resolution

Member A's updated branch now includes:
- `docs/requirements/functional-requirements.md` — FR-001 to FR-016, each with source stakeholder and
  MoSCoW priority
- `docs/requirements/non-functional-requirements.md` — NFR-001 to NFR-006, each with category, measurable
  target and priority
- `docs/requirements/acceptance-criteria.md` — full AC set (e.g. AC-001.1–AC-001.4, AC-010.1–AC-010.3)
  mapped to each FR/NFR
- `docs/requirements/traceability-handover.md` — Member A's own source → requirement summary, handed to
  Member C as the basis for the formal RTM

**Verification:** the IDs, source stakeholders and priorities in Member A's final documents were checked
against every place Member B's `scope_baseline.md`, `risk_register.md`, `constraints.md` and
`forward_engineering.md` referenced an FR/NFR/AC ID. No renumbering was required — all cross-references
resolve cleanly against Member A's confirmed register. `rtm.md`, `decision-log.md` and `PED.md` have been
updated accordingly and the DRAFT status markers removed.

This closes the item raised before PED v1.0 baseline sign-off.
