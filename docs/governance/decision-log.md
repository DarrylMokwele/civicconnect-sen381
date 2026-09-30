# CivicConnect Engineering Decision Log

## Milestone 2 — Architecture, Technology & Initial Design Baseline

**Project:** CivicConnect  
**Module:** SEN381  
**Version:** 2.0  
**Status:** Updated for Milestone 2 Integration Baseline


---

# 1. Purpose

This Engineering Decision Log records important technical decisions
made throughout the CivicConnect development process.

The decision log provides traceability between:

```
Problem
   ↓
Alternative Options
   ↓
Selected Decision
   ↓
ADR
   ↓
Implementation Evidence
```

Milestone 2 resolves decisions that were intentionally deferred during
Milestone 1.


---

# 2. Decision Summary


| Decision ID | Decision | Status | Related ADR |
|---|---|---|---|
| DEC-001 | Modular Monolith Architecture | Accepted | ADR-001 |
| DEC-002 | Technology Stack Selection | Accepted | ADR-002 |
| DEC-003 | Persistence and Database Strategy | Accepted | ADR-003 |
| DEC-004 | Notification Feedback Architecture | Accepted | ADR-004 |
| DEC-005 | External Location Provider Boundary | Accepted | ADR-005 |


---

# 3. DEC-001 — Modular Monolith Architecture


## Context

CivicConnect requires an architecture that supports maintainability,
testing and future growth without introducing unnecessary complexity.

During Milestone 1, architecture selection was identified as a future
decision requiring evaluation.


## Alternatives Considered


### Alternative 1 — Modular Monolith

Advantages:

- clear separation of responsibilities;
- simpler deployment;
- suitable for current project scale;
- easier testing.


### Alternative 2 — Microservices Architecture

Advantages:

- independent deployment;
- high scalability.

Trade-offs:

- increased operational complexity;
- additional deployment infrastructure.


### Alternative 3 — Traditional Layered Monolith

Advantages:

- simple structure.

Trade-offs:

- weaker module boundaries.


## Decision

CivicConnect will use a modular monolith architecture.


## Evidence

ADR:

```
ADR-001-modular-monolith-architecture
```

Implementation structure:

```
Domain

Application

Infrastructure

WebApi
```


## Status

Accepted


---

# 4. DEC-002 — Technology Stack Selection


## Context

CivicConnect requires a technology stack capable of supporting:

- Web API development;
- relational persistence;
- automated testing;
- maintainable software design.


## Alternatives Considered


### Alternative 1 — .NET / ASP.NET Core

Advantages:

- strong object-oriented support;
- mature ecosystem;
- integrated tooling.


### Alternative 2 — Java Spring Boot

Advantages:

- enterprise adoption.

Trade-off:

- different development ecosystem.


### Alternative 3 — Node.js

Advantages:

- rapid web development.

Trade-off:

- different architectural approach.


## Decision

The selected technology stack is:

```
C#

.NET

ASP.NET Core Web API

Entity Framework Core

SQL Server

xUnit

Swagger/OpenAPI
```


## Evidence

ADR:

```
ADR-002-technology-stack-selection
```


## Status

Accepted


---

# 5. DEC-003 — Persistence and Database Strategy


## Context

CivicConnect requires reliable storage for:

- service requests;
- users;
- statuses;
- assignments;
- history;
- notifications.


## Alternatives Considered


### Relational Database

Advantages:

- structured relationships;
- transactional consistency;
- strong querying capability.


### Document Database

Advantages:

- flexible document storage.

Trade-off:

- weaker fit for highly related civic-service entities.


## Decision

CivicConnect will use:

```
SQL Server

+

Entity Framework Core
```


## Evidence

Implementation:

```
CivicConnectDbContext

Entity Configurations

EF Core Migration
```


ADR:

```
ADR-003-persistence-and-concurrency-strategy
```


## Status

Accepted


---

# 6. DEC-004 — Notification Feedback Architecture


## Context

Service request lifecycle changes require feedback to users.

Directly embedding notification logic inside lifecycle code would create
tight coupling.


## Alternatives Considered


### Direct Notification Calls

Example:

```
Lifecycle Service
        |
        ↓
Notification Service
```

Trade-off:

- tightly coupled.


### Event Driven Notification

Example:

```
Lifecycle Event

      ↓

Notification Handler

      ↓

Notification Service
```

Advantages:

- separation of concerns;
- future extensibility.


## Decision

CivicConnect will use event-driven notification handling.


## Evidence


Implementation:

```
ServiceRequestStatusChangedEvent

ServiceRequestStatusChangedHandler

INotificationService

NotificationService
```


ADR:

```
ADR-004-notification-feedback-architecture
```


## Status

Accepted


---

# 7. DEC-005 — External Location Provider Boundary


## Context

Future CivicConnect versions may require external location services.

Direct dependency on a specific provider could make replacement
difficult.


## Alternatives Considered


### Direct Provider Integration

Advantages:

- simpler initial implementation.

Trade-off:

- strong dependency on provider.


### Adapter Boundary

Advantages:

- provider independence;
- easier replacement.


## Decision

CivicConnect will isolate external location providers through:


```
ILocationResolver
```


Implementation:


```
ExternalLocationResolver
```


## Evidence


Application:

```
ILocationResolver
```


Infrastructure:

```
ExternalLocationResolver
```


ADR:

```
ADR-005-external-location-provider-boundary
```


## Status

Accepted


---

# 8. Milestone 1 Deferred Decisions Resolved


| Previous Deferred Decision | Resolution |
|---|---|
| Architecture approach | Resolved through DEC-001 |
| Technology selection | Resolved through DEC-002 |
| Persistence technology | Resolved through DEC-003 |
| Notification approach | Resolved through DEC-004 |
| External integration boundary | Resolved through DEC-005 |


---

# 9. Outstanding Future Decisions


The following decisions remain for future milestones:


| Decision Area | Status |
|---|---|
| Authentication strategy | Future milestone |
| Authorisation model | Future milestone |
| Production deployment strategy | Future milestone |
| External notification provider | Future milestone |
| Live geolocation provider | Future milestone |


---

# 10. Approval


This decision log represents the accepted engineering decisions for the
Milestone 2 integration baseline.

Further decisions must be recorded using the same controlled process.
