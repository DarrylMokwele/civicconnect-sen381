# CivicConnect Milestone 2 Baseline Sign-Off

## Architecture, Technology & Initial Design Baseline


---

# Document Control

| Field | Information |
|---|---|
| Project | CivicConnect |
| Module | SEN381 |
| Milestone | Milestone 2 |
| Document | Baseline Sign-Off |
| Version | 2.0 |
| Status | Integration Baseline |


---

# 1. Purpose

This document records the acceptance of the CivicConnect Milestone 2
engineering baseline.

Milestone 2 establishes the foundation required for continued
development by consolidating:

- architecture decisions;
- technology decisions;
- data and persistence decisions;
- design decisions;
- initial implementation;
- verification evidence;
- engineering documentation.


---

# 2. Baseline Contents


The Milestone 2 baseline includes:


## Architecture

Completed:

✅ Architecture evaluation  
✅ Selected architecture approach  
✅ Architectural Significant Requirements  
✅ Architecture documentation  


Selected approach:

```
Modular Monolith Architecture
```


---

## Technology

Completed:

✅ Technology evaluation  
✅ Technology selection  
✅ Technology ADR  


Selected technologies:

```
C#
.NET
ASP.NET Core Web API
Entity Framework Core
SQL Server
xUnit
Swagger/OpenAPI
```


---

## Data and Persistence

Completed:

✅ Data model  
✅ Persistence strategy  
✅ Entity Framework Core configuration  
✅ Database migration  


Persistence approach:

```
SQL Server
+
Entity Framework Core
```


---

## Design Decisions

Completed:


### Notification Architecture

Decision:

```
Event-driven notification handling
```


Evidence:

```
ServiceRequestStatusChangedEvent

ServiceRequestStatusChangedHandler

INotificationService

NotificationService
```


---


### External Location Provider Boundary


Decision:

```
ILocationResolver abstraction
```


Evidence:

```
ILocationResolver

ExternalLocationResolver
```


---

## Implementation

Completed:

✅ Domain layer  
✅ Application layer  
✅ Infrastructure layer  
✅ Web API layer  
✅ Database persistence  
✅ Lifecycle governance  
✅ Notification persistence  
✅ Automated testing  


---

# 3. Verification Evidence


The following verification evidence has been completed:


## Automated Testing

Test projects:

```
CivicConnect.Domain.Tests

CivicConnect.Application.Tests

CivicConnect.Infrastructure.Tests
```


Coverage includes:

- lifecycle transitions;
- invalid transition handling;
- persistence behaviour;
- notification creation;
- location boundary behaviour.


---

## API Verification


Verified endpoint:

```
PATCH /api/ServiceRequests/{id}/status
```


Valid transition:

```
Submitted → Assigned
```


Result:

```
204 No Content
```


Invalid transition:

```
Assigned → Closed
```


Result:

```
400 Bad Request
```


---

# 4. Traceability


The following traceability artefacts are included:


| Artefact | Status |
|-|-|
| Requirements Traceability Matrix | Updated |
| Architecture Documentation | Complete |
| ADR Register | Updated |
| Decision Log | Updated |
| AI Usage Register | Updated |
| README | Updated |


---

# 5. Known Limitations


The following items remain outside the Milestone 2 baseline:


- authentication;
- authorisation;
- complete citizen workflow;
- staff assignment workflow;
- reporting dashboards;
- external notification providers;
- live location provider integration;
- production deployment.


These items remain traceable for future milestones.


---

# 6. Team Approval


By approving this baseline, the team confirms that:

- the architecture direction has been reviewed;
- technology choices have been evaluated;
- design decisions have been documented;
- implementation evidence exists;
- verification evidence has been collected;
- remaining limitations are recorded honestly.


---

# 7. Approval Record


| Team Member | Role | Approval |
|-|-|-|
| Team Member 1 | Architecture / ASR Lead | Pending |
| Team Member 2 | Technology / Data Lead | Pending |
| Team Member 3 | Design / Implementation Lead | Pending |


---

# 8. Final Baseline Status


```
CivicConnect Milestone 2 Baseline

STATUS:
READY FOR REVIEW
```

