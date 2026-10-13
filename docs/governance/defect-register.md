# Defect Register - M3 Member 2 additions
| ID | Basis | Severity / impact | Status | Fix / next step | Regression evidence |
|---|---|---|---|---|---|
| M2-SEC-01 | FR-010 AC-010.1; NFR-002; R-008 | High: anonymous status mutation | Open; source confirmed, runtime pending | Implement real authentication and staff/resource policy; block external release | SEC-03 characterisation pending; then 401/403/allowed-role cases |
| M2-SEC-02 | NFR-005; NFR-002 | High: caller can attribute changes to another user | Open | Derive actor from trusted principal; verify permissions | Actor-spoofing and unknown-actor cases required |
| M2-VAL-01 | API validation contract | Medium: empty IDs previously accepted by model binding | Fix implemented; verification pending | IValidatableObject non-empty ID validation | SEC-01/02 pending |
| M2-INT-01 | FR-005 AC-005.1 | Medium/high: notification failure follows committed status/history | Open | Team transaction/outbox decision and failure regression | No failure execution yet |
| M2-DOC-01 | RTM/requirements conformance | Medium: RTM NFR-004/006 summaries disagree with acceptance criteria | Open | Team reconcile baseline through change control; do not silently renumber | Documentation review required |
