# CivicConnect Risk Register

## Milestone 2 — Architecture, Technology & Initial Design Baseline


---

# Document Control

| Field | Information |
|---|---|
| Project | CivicConnect |
| Module | SEN381 |
| Document | Risk Register |
| Version | 2.0 |
| Milestone | Milestone 2 |
| Status | Updated Integration Baseline |


---

# 1. Purpose

This Risk Register identifies, evaluates and tracks risks that may
affect the successful delivery of CivicConnect.

The Milestone 2 Risk Register focuses on risks related to:

- architecture;
- technology;
- persistence;
- implementation;
- integration;
- verification;
- project governance.


---

# 2. Risk Rating Method


## Probability

| Rating | Meaning |
|---|---|
| Low | Unlikely to occur |
| Medium | Possible occurrence |
| High | Likely occurrence |


## Impact

| Rating | Meaning |
|---|---|
| Low | Minimal effect |
| Medium | Moderate effect |
| High | Significant effect |


## Risk Level

```
Low Probability + Low Impact = Low Risk

Medium combinations = Medium Risk

High Probability + High Impact = High Risk
```


---

# 3. Active Risk Register


| ID | Risk | Category | Probability | Impact | Mitigation | Owner | Status |
|---|---|---|---|---|---|---|---|
| R-001 | Architecture complexity may increase development difficulty | Architecture | Medium | High | Use modular monolith approach with clear boundaries | Team | Managed |
| R-002 | Incorrect lifecycle transitions may cause invalid service-request states | Design | Medium | High | Centralise transitions through StatusTransitionPolicy and automated tests | Member 3 | Managed |
| R-003 | Database relationships may not correctly represent the domain model | Data | Medium | High | Validate ERD, EF Core configurations and migration behaviour | Member 2 | Managed |
| R-004 | Technology decisions may not align with implementation requirements | Technology | Low | Medium | Evaluate alternatives and record decisions through ADRs | Member 2 | Managed |
| R-005 | Integration conflicts between member branches may overwrite important work | Project Management | Medium | High | Use integration branch and pull request workflow | Team | Managed |
| R-006 | Missing automated verification may reduce confidence in implementation | Testing | Medium | High | Create xUnit tests for lifecycle, persistence and services | Member 3 | Managed |
| R-007 | External integrations may create unnecessary system coupling | Architecture | Medium | Medium | Use abstraction boundaries such as ILocationResolver | Member 3 | Managed |
| R-008 | Authentication and authorisation are not yet implemented | Security | Medium | High | Document as future milestone work and prevent false completion claims | Team | Accepted Future Risk |
| R-009 | External notification providers are not integrated | Integration | Medium | Medium | Maintain notification service abstraction for future providers | Team | Accepted Future Risk |
| R-010 | Documentation may become inconsistent after branch integration | Governance | Medium | Medium | Perform final PED, RTM, ADR and README review before baseline approval | Team | Managed |


---

# 4. Detailed Risk Analysis


# R-001 — Architecture Complexity


## Description

Introducing multiple architectural layers may increase development
complexity.


## Impact

Developers may struggle with:

- dependency direction;
- project references;
- responsibility separation.


## Mitigation

CivicConnect uses a modular monolith architecture with:

```
Domain

Application

Infrastructure

WebApi
```

Clear responsibilities are documented in the PED.


## Status

Managed


---

# R-002 — Invalid Lifecycle Transitions


## Description

Incorrect status changes could corrupt service-request workflow.


## Impact

Examples:

```
Submitted → Closed
```

without required intermediate states.


## Mitigation

Lifecycle transitions are controlled by:


```
IStatusTransitionPolicy

        ↓

StatusTransitionPolicy
```


Invalid transitions return:

```
400 Bad Request
```


Verification:

- StatusTransitionPolicyTests;
- ServiceRequestLifecycleServiceTests.


## Status

Managed


---

# R-003 — Data Integrity Risk


## Description

Incorrect entity relationships could result in unreliable data.


## Mitigation

The project uses:

- EF Core configurations;
- relational constraints;
- migration validation;
- persistence tests.


## Status

Managed


---

# R-005 — Branch Integration Risk


## Description

Multiple team branches modifying shared documentation may create merge
conflicts.


## Mitigation

Integration process:

```
Feature Branch

      ↓

Pull Request

      ↓

Integration/m2

      ↓

Main
```


Conflicting documents are manually reviewed.


## Status

Managed


---

# R-006 — Verification Coverage Risk


## Description

Incomplete testing could reduce confidence in implementation.


## Mitigation

Automated tests were introduced for:

- lifecycle rules;
- persistence;
- notifications;
- location boundary.


## Status

Managed


---

# R-008 — Security Implementation Risk


## Description

Authentication and authorisation are not implemented during M2.


## Impact

Some requirements involving authorised access cannot yet be fully
verified.


## Mitigation

The requirement limitation is documented in:

- PED;
- RTM;
- known limitations.


Future milestones will implement:

- authentication;
- authorisation;
- role-based access.


## Status

Accepted Future Risk


---

# 5. Risk Summary


| Status | Count |
|---|---|
| Managed | 8 |
| Accepted Future Risk | 2 |


---

# 6. Milestone 2 Risk Review


The following risks were reviewed during integration:

✅ Architecture risks reviewed  
✅ Technology risks reviewed  
✅ Persistence risks reviewed  
✅ Branch integration risks reviewed  
✅ Testing risks reviewed  
✅ Documentation consistency risks reviewed  


---

# 7. Approval


This Risk Register represents the current risk baseline for the
CivicConnect Milestone 2 integration branch.

Risks will continue to be reviewed during future milestones as new
features and integrations are introduced.
