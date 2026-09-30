# CivicConnect AI Usage Register

## Milestone 2 — Architecture, Technology & Initial Design Baseline


---

# Document Control

| Field | Information |
|---|---|
| Project | CivicConnect |
| Module | SEN381 |
| Document | AI Usage Register |
| Version | 2.0 |
| Milestone | Milestone 2 |
| Status | Updated Integration Baseline |


---

# 1. Purpose

This document records the use of Artificial Intelligence (AI) tools
during Milestone 2 development.

The purpose of this register is to provide transparency regarding:

- where AI assistance was used;
- what activities AI supported;
- how generated information was reviewed;
- what decisions remained the responsibility of the development team.


AI tools were used as an assistance mechanism only.

All final engineering decisions, implementation choices and submitted
artefacts were reviewed and approved by the project team.


---

# 2. AI Tool Used


| Tool | Purpose |
|---|---|
| ChatGPT | Technical guidance, documentation assistance, code review support and clarification of software engineering concepts |


---

# 3. AI Usage Summary


| Area | AI Assistance | Human/Team Responsibility |
|---|---|---|
| Architecture | Assisted with explaining architecture alternatives and structuring documentation | Team evaluated alternatives and selected the architecture |
| Technology Evaluation | Assisted with comparing technology options | Team selected the final technology stack |
| Database Design | Assisted with explaining persistence concepts and EF Core approaches | Team confirmed entities, relationships and implementation |
| Design Decisions | Assisted with documenting alternatives and trade-offs | Team made final design decisions |
| Implementation | Assisted with debugging, code explanations and development guidance | Team reviewed, tested and integrated code |
| Testing | Assisted with identifying possible test scenarios | Team verified tests and evidence |
| Documentation | Assisted with formatting and structuring documents | Team validated final documentation |


---

# 4. Detailed AI Usage Records


## Record 001 — Architecture Documentation


### Activity

Creating and improving architecture documentation.


### AI Assistance

AI was used to:

- explain architecture concepts;
- compare architectural approaches;
- improve documentation structure.


### Human Validation

The team reviewed architecture alternatives and selected the
modular monolith architecture based on CivicConnect requirements.


### Evidence

Related artefacts:

```
ADR-001-modular-monolith-architecture.md

Architecture Evaluation

PED v2.0
```


---

# Record 002 — Technology Stack Evaluation


### Activity

Evaluating possible technologies for CivicConnect.


### AI Assistance

AI was used to:

- explain technology differences;
- assist with comparison structure;
- improve technical descriptions.


### Human Validation

The team selected:

```
C#

.NET

ASP.NET Core

Entity Framework Core

SQL Server

xUnit
```

based on project requirements and constraints.


### Evidence

Related artefacts:

```
ADR-002-technology-stack-selection.md

Technology Stack Evaluation
```


---

# Record 003 — Persistence and Database Design


### Activity

Developing the persistence baseline.


### AI Assistance

AI was used to:

- explain EF Core concepts;
- assist with database design discussions;
- suggest testing approaches.


### Human Validation

The team reviewed:

- entity relationships;
- database structure;
- persistence implementation.


### Evidence

Related artefacts:

```
CivicConnectDbContext

Entity Configurations

EF Core Migration

Persistence ADR
```


---

# Record 004 — Lifecycle Governance Implementation


### Activity

Implementing controlled service-request lifecycle transitions.


### AI Assistance

AI was used to:

- explain state-transition approaches;
- assist with implementation structure;
- suggest test cases.


### Human Validation

The team decided:

- transition rules;
- allowed transitions;
- exception handling;
- verification approach.


### Evidence

Implementation:

```
IStatusTransitionPolicy

StatusTransitionPolicy

ServiceRequestLifecycleService

PATCH /api/ServiceRequests/{id}/status
```


Testing:

```
StatusTransitionPolicyTests

ServiceRequestLifecycleServiceTests
```


---

# Record 005 — Notification Architecture


### Activity

Designing notification handling.


### AI Assistance

AI was used to:

- explain event-driven approaches;
- assist with documentation.


### Human Validation

The team selected event-driven notification handling.


### Evidence

```
ADR-004-notification-feedback-architecture.md

ServiceRequestStatusChangedEvent

ServiceRequestStatusChangedHandler

NotificationService
```


---

# Record 006 — External Location Provider Boundary


### Activity

Designing an external integration boundary.


### AI Assistance

AI was used to:

- explain adapter pattern concepts;
- assist with documentation.


### Human Validation

The team selected:

```
ILocationResolver
```

as the application abstraction.


### Evidence

```
ADR-005-external-location-provider-boundary.md

ILocationResolver

ExternalLocationResolver
```


---

# Record 007 — Debugging and Development Support


### Activity

Resolving implementation issues during development.


### AI Assistance

AI was used for:

- explaining compiler errors;
- debugging guidance;
- suggesting possible causes of issues.


### Human Validation

Developers tested suggested solutions and only accepted changes that
worked correctly within the CivicConnect project.


---

# 5. AI Limitations


AI-generated suggestions were treated as recommendations and were not
accepted automatically.

The team verified:

- correctness;
- compatibility with the project architecture;
- alignment with requirements;
- implementation behaviour.


AI was not used to:

- make final project decisions;
- replace engineering evaluation;
- replace testing;
- replace peer review.


---

# 6. Team Responsibility Statement


The CivicConnect team remains responsible for:

- all submitted code;
- architecture decisions;
- design decisions;
- documentation;
- testing evidence;
- final milestone submission.


AI assistance was used only to support productivity,
understanding and documentation quality.

---

# 7. Milestone 2 Status

AI usage has been documented for:

✅ Architecture evaluation  
✅ Technology evaluation  
✅ Persistence design  
✅ Design decisions  
✅ Implementation support  
✅ Testing support  
✅ Documentation improvement  

This register will continue to evolve in future milestones.
