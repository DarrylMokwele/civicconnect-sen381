# Project Constraints & Architectural Trade-offs

## Baseline Constraints
* **Schedule Constraint:** Strict phased delivery milestones (M1 foundation baseline must freeze before M2 architecture/construction).
* **Resource & Cost Constraint:** 3-person development team working with zero capital budget for commercial cloud licenses or paid enterprise APIs.
* **Scope Constraint:** Zero architectural code or premature database implementation permitted in M1 baseline (foundation and requirements only).
* **Quality & Verification Constraint:** Every baselined functional requirement must possess an objective, testable acceptance criterion with 100% traceability to verification evidence in later milestones.
* **Security & Regulatory Constraint:** Protection of citizen personal information in compliance with POPIA, restricting unauthenticated exposure of issue reporter contact details.

## Constraint Interaction & Ripple Effect Trade-Off
* **Trade-off Analysis:** Schedule Constraint vs. POPIA Security Constraint.
* **Ripple Effect:** Implementing complete POPIA-compliant data masking, authentication, and encrypted document storage increases implementation complexity. Under the strict schedule constraint, the team cannot implement custom OAuth providers and enterprise directory federation from scratch. To preserve quality and schedule without violating security constraints, the team must prioritize a lightweight, standard authentication library and defer third-party social logins.