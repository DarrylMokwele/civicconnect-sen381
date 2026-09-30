# CivicConnect — Requirements Traceability Matrix (RTM) — Milestone 2

## Document Control

- **Project:** CivicConnect
- **Module:** SEN381
- **Milestone:** Milestone 2 — Architecture, Technology & Initial Design Baseline
- **Document:** Requirements Traceability Matrix (RTM)
- **Status:** Updated for Milestone 2 — architecture, data, design, technology, implementation and initial verification evidence added where available.

---

## Purpose

This Requirements Traceability Matrix maintains traceability between CivicConnect stakeholder needs, functional and non-functional requirements, acceptance criteria, architecture and design decisions, data and technology decisions, implementation artefacts, verification evidence and ADR/decision evidence.

The Milestone 2 RTM evolves the Milestone 1 requirements baseline without renumbering or replacing approved requirement identifiers.

Requirements that do not yet have implementation evidence remain baselined for future milestones.

---

## Traceability Status Definitions

The following status values are used throughout this RTM:

- **Baselined / Future Implementation** — Requirement remains part of the approved baseline but has not yet been implemented.
- **Architecture / Design Established** — Supporting architectural or design decisions exist, but the requirement itself has not yet been fully implemented.
- **Persistence Mechanism Implemented / Requirement Not Yet Fully Verified** — Supporting persistence infrastructure exists, but the complete requirement flow has not yet been implemented and verified.
- **Partially Implemented / Initially Verified** — Some acceptance criteria or behaviour have implementation and verification evidence, but the complete requirement is not yet satisfied.
- **Implemented / Initially Verified** — The current Milestone 2 implementation provides direct implementation and initial verification evidence for the requirement.

---

# Requirements Traceability Matrix

| Source | Requirement ID | Requirement Summary | Priority | Acceptance Criteria | Architecture / Design Evidence | Data / Technology Evidence | Implementation Evidence | Verification Evidence | ADR / Decision Evidence | Status |
|---|---|---|---|---|---|---|---|---|---|---|
| STK-001 | FR-001 | Submit a service request with required information | Must | Refer to approved FR-001 acceptance criteria | CivicConnect architecture provides separation between API, Application, Domain and Infrastructure concerns | `ServiceRequest` entity, EF Core mapping and SQL Server persistence structure established | Full service-request submission flow not yet implemented | Persistence infrastructure has initial automated verification; full FR-001 flow not yet verified | Architecture and persistence decisions | Baselined / Future Implementation |
| STK-001 | FR-002 | Select a category for a service request | Must | Refer to approved FR-002 acceptance criteria | Supported by the general CivicConnect architecture baseline | Data/design work remains subject to the approved category model | — | — | — | Baselined / Future Implementation |
| STK-001 | FR-003 | View current status of own requests | Must | AC-003.1–AC-003.2 | Architecture supports service-request querying and status separation | `ServiceRequest` and `RequestStatus` persistence model established | Status information exists in the persistence model, but citizen request-view functionality is not yet implemented | — | Persistence/data-model decision | Baselined / Future Implementation |
| STK-001 | FR-004 | View previous service requests | Must | Refer to approved FR-004 acceptance criteria | Architecture supports future request-query use cases | `ServiceRequest` persistence model established | — | — | — | Baselined / Future Implementation |
| STK-001 | FR-005 | Receive feedback on submit/reject/update/complete | Must | AC-005.1 | Notification/Feedback Architecture separates lifecycle behaviour from notification handling using `ServiceRequestStatusChangedEvent`, `ServiceRequestStatusChangedHandler` and `INotificationService` | `Notification` entity, EF Core `NotificationConfiguration`, `CivicConnectDbContext` and SQL Server `Notifications` table | `ServiceRequestStatusChangedHandler`; `INotificationService`; `NotificationService` | `ServiceRequestStatusChangedHandlerTests`; `NotificationServiceTests`; database evidence confirms a `StatusChanged` notification is created after a valid status transition | Notification / Feedback Architecture ADR | Partially Implemented / Initially Verified |
| STK-002 | FR-006 | View service requests relevant to staff responsibilities | Must | Refer to approved FR-006 acceptance criteria | Architecture supports future staff-facing application use cases | Service-request persistence baseline exists | — | — | — | Baselined / Future Implementation |
| STK-002 | FR-007 | Search, filter and sort service requests | Must | Refer to approved FR-007 acceptance criteria | Architecture supports future query/application services | Relational persistence baseline supports future query implementation | — | — | — | Baselined / Future Implementation |
| STK-002 | FR-008 | View service request details | Must | Refer to approved FR-008 acceptance criteria | Architecture supports service-request detail use cases | `ServiceRequest`, `RequestStatus`, `Department`, `Assignment`, `StatusHistory` and related entities establish the data baseline | — | — | — | Baselined / Future Implementation |
| STK-002 | FR-009 | Assign service requests | Must | Refer to approved FR-009 acceptance criteria | Architecture provides Application/Domain/Infrastructure separation for future assignment behaviour | `Assignment` and `Department` entities and EF Core configurations established | Assignment functionality is not yet exposed through the application/API | — | Data/persistence decision | Baselined / Future Implementation |
| STK-002 | FR-010 | Update request status via allowed transitions | Must | AC-010.1–AC-010.3 | Centralised lifecycle governance through `IStatusTransitionPolicy`, `StatusTransitionPolicy`, `IServiceRequestLifecycleService` and `ServiceRequestLifecycleService`; invalid transitions raise `InvalidStatusTransitionException` | `ServiceRequest`, `RequestStatus` and `StatusHistory` entities persisted using EF Core and SQL Server | `IServiceRequestLifecycleService`; `ServiceRequestLifecycleService`; `ServiceRequestsController`; `PATCH /api/ServiceRequests/{id}/status` | `StatusTransitionPolicyTests`; `ServiceRequestLifecycleServiceTests`; Swagger valid transition returns `204 No Content`; invalid `Assigned → Closed` transition returns `400 Bad Request` | Lifecycle governance / FEC-04 related design evidence | Partially Implemented / Initially Verified |
| STK-002 | FR-011 | Record actions, comments or resolution details on service requests | Must | Refer to approved FR-011 acceptance criteria | General architecture supports future request-action use cases | Current persistence baseline includes request lifecycle/history data but does not establish the complete FR-011 implementation | — | — | — | Baselined / Future Implementation |
| STK-003 | FR-012 | Maintain controlled request lifecycle information | Must | Refer to approved FR-012 acceptance criteria | Lifecycle rules are centralised through the transition policy rather than distributed through controllers | `RequestStatus`, `ServiceRequest` and `StatusHistory` data structures support lifecycle persistence | Initial lifecycle transition implementation exists | Lifecycle policy and lifecycle service automated tests | Lifecycle governance decision | Partially Implemented / Initially Verified |
| STK-003 | FR-013 | View request activity/history | Must | Refer to approved FR-013 acceptance criteria | Architecture supports future request-history querying | `StatusHistory` persistence model established and populated during valid status transitions | History is persisted but a complete activity-view interface has not yet been implemented | Database and lifecycle tests verify history persistence | Persistence/lifecycle decision | Baselined / Future Implementation |
| STK-003 | FR-014 | View open, overdue, resolved and closed request reporting | Should | Refer to approved FR-014 acceptance criteria | Architecture provides a foundation for future reporting/query services | Relational persistence baseline established | — | — | — | Baselined / Future Implementation |
| STK-003 | FR-015 | View information by category and status | Should | Refer to approved FR-015 acceptance criteria | Architecture provides a foundation for future reporting/query services | `RequestStatus` persistence exists; complete reporting data flow not yet implemented | — | — | — | Baselined / Future Implementation |
| STK-003 | FR-016 | View performance and accountability information | Should | Refer to approved FR-016 acceptance criteria | Architecture supports future reporting/accountability components | Audit-history persistence provides part of the future evidence base | — | — | — | Baselined / Future Implementation |
| STK-001 / STK-002 | NFR-001 | Maintain appropriate system usability and responsiveness | Must | Refer to approved NFR-001 acceptance criteria | Architecture baseline separates concerns to support maintainability and future performance work | Technology stack selected for the CivicConnect baseline | Initial implementation exists but NFR-specific performance/usability verification is outstanding | — | Architecture / technology decisions | Architecture / Design Established |
| STK-001 / STK-002 | NFR-002 | Maintain appropriate security and access control | Must | Refer to approved NFR-002 acceptance criteria | Architecture provides boundaries where authentication/authorisation can be introduced | Current technology baseline supports future ASP.NET Core security integration | Full authentication/authorisation implementation is not yet complete | — | Architecture / technology decisions | Baselined / Future Implementation |
| STK-001 | NFR-003 | Request stored before confirming success | Must | AC per NFR-003 | Persistence concerns are isolated within Infrastructure through EF Core | `ServiceRequest` entity; `CivicConnectDbContext`; EF Core configurations; migration; SQL Server `ServiceRequests` table | Persistence infrastructure is implemented; full request-submission flow remains outstanding | `CivicConnectDbContextTests.CanSaveAndRetrieveServiceRequest` provides initial persistence verification | Persistence ADR | Persistence Mechanism Implemented / Requirement Not Yet Fully Verified |
| STK-002 / STK-003 | NFR-004 | Maintain reliable and consistent service-request data | Must | Refer to approved NFR-004 acceptance criteria | Centralised lifecycle policy reduces uncontrolled lifecycle updates | Relational SQL Server persistence, EF Core mappings and foreign-key relationships established | Lifecycle persistence path implemented for status changes | Lifecycle and persistence tests provide initial evidence | Persistence and lifecycle decisions | Partially Implemented / Initially Verified |
| STK-002 / STK-003 | NFR-005 | Status change records user, status and time | Must | AC per NFR-005 | Lifecycle governance records status-history information as part of the valid transition flow | `StatusHistory` entity and EF Core mapping; SQL Server `StatusHistories` table | `ServiceRequestLifecycleService` creates a `StatusHistory` containing `ServiceRequestId`, `StatusId`, `ChangedByUserId` and `ChangedAt` | `ServiceRequestLifecycleServiceTests`; SQL Server evidence confirms user, new status and timestamp are persisted | Persistence / lifecycle decision evidence | Implemented / Initially Verified |
| STK-003 | NFR-006 | Support maintainable and replaceable external integrations | Should | Refer to approved NFR-006 acceptance criteria where applicable | External location-provider boundary uses `ILocationResolver` to prevent Application logic from depending directly on a specific third-party provider | Provider-specific implementation is isolated in Infrastructure; live third-party geocoding remains future work | `ILocationResolver`; `LocationResult`; `ExternalLocationResolver`; dependency-injection registration | `ExternalLocationResolverTests` verify the current adapter-boundary behaviour | External Location Provider Boundary ADR | Architecture / Design Established |

---

# Milestone 2 Design Traceability

## Design Problem 1 — Notification / Feedback Architecture

### Problem

CivicConnect requires feedback and notification behaviour without coupling core service-request lifecycle logic directly to persistence or future notification-delivery technologies.

### Selected Design Direction

The Milestone 2 implementation uses an event-driven notification boundary.

### Trace

```text
Service Request Status Change
        ↓
ServiceRequestStatusChangedEvent
        ↓
ServiceRequestStatusChangedHandler
        ↓
INotificationService
        ↓
NotificationService
        ↓
CivicConnectDbContext
        ↓
Notifications table
```

### Implementation Evidence

- `ServiceRequestStatusChangedEvent`
- `ServiceRequestStatusChangedHandler`
- `INotificationService`
- `NotificationService`
- `Notification`
- `NotificationConfiguration`
- `CivicConnectDbContext`

### Verification Evidence

- `ServiceRequestStatusChangedHandlerTests`
- `NotificationServiceTests`
- Lifecycle integration test
- SQL Server `Notifications` table evidence

### Current Limitation

The current Milestone 2 implementation creates and persists notification records.

A live external email, SMS or push-notification provider has not yet been integrated.

---

## Design Problem 2 — External Location Provider Boundary

### Problem

CivicConnect may require external location-resolution capability without coupling Application logic directly to a specific third-party mapping or geocoding provider.

### Selected Design Direction

A provider-independent adapter boundary is established through `ILocationResolver`.

### Trace

```text
Application
        ↓
ILocationResolver
        ↑
Infrastructure
        ↓
ExternalLocationResolver
        ↓
Future External Geocoding Provider
```

### Implementation Evidence

- `LocationResult`
- `ILocationResolver`
- `ExternalLocationResolver`
- Dependency-injection registration in the Web API composition root

### Verification Evidence

- `ExternalLocationResolverTests`

### Current Limitation

The provider boundary is implemented, but a live third-party geocoding provider has not yet been integrated.

The current implementation establishes the replaceable integration boundary required by the selected design approach.

---

# End-to-End Trace — Milestone 2 Worked Example

## 1. Stakeholder Need

STK-002 (Operational Staff) requires the ability to manage and update service requests through controlled operational processes.

↓

## 2. Functional Requirement

**FR-010**

The system shall allow authorised staff to update a request's status using allowed status changes.

↓

## 3. Acceptance Criteria

**AC-010.1**

Only authorised staff can change status.

**AC-010.2**

Invalid status changes are rejected.

**AC-010.3**

A valid status change is saved and displayed.

↓

## 4. Architecture / Design

Lifecycle governance is centralised through:

- `IStatusTransitionPolicy`
- `StatusTransitionPolicy`
- `IServiceRequestLifecycleService`
- `ServiceRequestLifecycleService`
- `InvalidStatusTransitionException`

Transition rules are therefore not scattered throughout controllers or infrastructure components.

↓

## 5. Data / Persistence

The lifecycle implementation uses:

- `ServiceRequest`
- `RequestStatus`
- `StatusHistory`
- `Notification`
- `CivicConnectDbContext`
- Entity Framework Core
- SQL Server

↓

## 6. Application Interface

The lifecycle functionality is exposed through:

```text
PATCH /api/ServiceRequests/{id}/status
```

The request includes:

```text
NewStatusId
ChangedByUserId
```

↓

## 7. Valid Transition Evidence

The Milestone 2 implementation demonstrated:

```text
Submitted
    ↓
Assigned
```

Result:

```text
204 No Content
```

The successful operation:

1. validates the requested transition;
2. updates the service request;
3. persists the new status;
4. creates a `StatusHistory` record;
5. creates notification handling through the notification architecture.

↓

## 8. Invalid Transition Evidence

The implementation also demonstrated:

```text
Assigned
    ↓
Closed
```

This transition is not permitted by the current transition policy.

Result:

```text
400 Bad Request
```

with an error equivalent to:

```text
Transition from 'Assigned' to 'Closed' is not allowed.
```

The rejected transition does not:

- change the service request to Closed;
- create an additional status-history record;
- create an additional notification record.

↓

## 9. Auditability Evidence

A valid transition creates a `StatusHistory` record containing:

- `ServiceRequestId`
- `StatusId`
- `ChangedByUserId`
- `ChangedAt`

This provides direct initial implementation evidence for **NFR-005**.

↓

## 10. Notification Evidence

A successful status transition produces:

```text
ServiceRequestStatusChangedEvent
        ↓
ServiceRequestStatusChangedHandler
        ↓
INotificationService
        ↓
NotificationService
        ↓
Notifications table
```

The resulting notification contains the related service request and user and is initially stored with a pending status.

↓

## 11. Automated Verification Evidence

Automated verification includes:

- `StatusTransitionPolicyTests`
- `ServiceRequestLifecycleServiceTests`
- `ServiceRequestStatusChangedHandlerTests`
- `NotificationServiceTests`
- `CivicConnectDbContextTests`
- `ExternalLocationResolverTests`

↓

## 12. Manual / Application Verification Evidence

Manual Milestone 2 verification includes:

- Swagger valid-transition response — `204 No Content`
- Swagger invalid-transition response — `400 Bad Request`
- SQL Server `ServiceRequests` evidence
- SQL Server `StatusHistories` evidence
- SQL Server `Notifications` evidence
- successful Web API startup
- successful Swagger/OpenAPI startup

↓

## 13. Current Traceability Status

The current implementation provides initial evidence for:

- **AC-010.2** — invalid lifecycle transitions are rejected;
- the persistence aspect of **AC-010.3** — valid lifecycle transitions are saved;
- **NFR-005** — status changes record the responsible user, new status and timestamp.

**AC-010.1 remains outstanding** because staff authentication/authorisation has not yet been implemented and verified.

FR-010 is therefore:

**Partially Implemented / Initially Verified**

rather than fully complete.

---

# Persistence Traceability

The Milestone 2 persistence implementation follows the architecture boundary established for CivicConnect.

```text
Domain Entities
        ↓
Infrastructure Persistence
        ↓
CivicConnectDbContext
        ↓
Entity Framework Core
        ↓
SQL Server
```

The initial persistence baseline includes:

- `Role`
- `User`
- `RequestStatus`
- `Department`
- `ServiceRequest`
- `StatusHistory`
- `Assignment`
- `Notification`

The EF Core implementation includes:

- `CivicConnectDbContext`
- explicit entity configurations;
- `InitialCreate` migration;
- SQL Server database creation;
- automated persistence verification.

Persistence-specific behaviour remains subject to the individual requirements and acceptance criteria. The existence of the database infrastructure does not automatically mark every persistence-related requirement as complete.

---

# Technology Traceability

The Milestone 2 implementation currently demonstrates the selected technology direction through:

- C# / .NET
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server / SQL Server LocalDB for development
- xUnit automated testing
- Swagger / OpenAPI for initial API verification
- Git and GitHub for source control and project evidence

Technology selection evidence is maintained in the Member 2 technology evaluation and associated ADR documentation.

---

# Architecture Traceability

The current solution structure reflects separation between major responsibilities:

```text
CivicConnect
│
├── Domain
│   ├── Entities
│   ├── Events
│   └── Exceptions
│
├── Application
│   ├── Abstractions
│   ├── Notifications
│   ├── ServiceRequests
│   └── Locations
│
├── Infrastructure
│   ├── Persistence
│   ├── Notifications
│   ├── ServiceRequests
│   └── Locations
│
└── WebApi
    └── Controllers
```

The implementation therefore provides initial forward-engineering evidence from the Milestone 2 architecture baseline into application code.

---

# Milestone 2 Verification Summary

Current initial verification evidence includes:

| Area | Evidence | Status |
|---|---|---|
| Domain model | Domain project and entities compile successfully | Verified |
| Persistence | `CivicConnectDbContextTests` | Initially Verified |
| Notification handler | `ServiceRequestStatusChangedHandlerTests` | Initially Verified |
| Notification persistence | `NotificationServiceTests` | Initially Verified |
| Transition policy | `StatusTransitionPolicyTests` | Initially Verified |
| Lifecycle service | `ServiceRequestLifecycleServiceTests` | Initially Verified |
| Valid lifecycle transition | Swagger `204 No Content` | Initially Verified |
| Invalid lifecycle transition | Swagger `400 Bad Request` | Initially Verified |
| Audit history | SQL Server `StatusHistories` evidence | Initially Verified |
| Notification record | SQL Server `Notifications` evidence | Initially Verified |
| Location-provider boundary | `ExternalLocationResolverTests` | Initially Verified |
| Web API startup | ASP.NET Core application starts successfully | Verified |
| API documentation | Swagger loads successfully | Verified |

---

# Outstanding Work

The Milestone 2 implementation is an initial architectural and design baseline and is not the complete CivicConnect product.

Outstanding work includes, but is not limited to:

- authentication and authorisation;
- verification of AC-010.1;
- complete citizen service-request submission;
- citizen request-history views;
- staff request queues;
- assignment functionality;
- search, filtering and sorting;
- comments/actions/resolution functionality;
- reporting and analytics;
- complete notification delivery channels;
- live external geocoding provider integration;
- complete performance verification;
- complete security verification;
- later-milestone acceptance and system testing.

These outstanding items remain traceable through the approved requirements baseline.

---

# Notes

- All FR, NFR and AC identifiers remain unchanged from the approved requirements baseline.

- Milestone 2 adds architecture, data, design, technology, implementation and initial verification evidence without renumbering the Milestone 1 requirements.

- A requirement is not marked complete merely because supporting architecture, design or persistence infrastructure exists.

- Requirement status reflects the implementation and verification evidence currently available.

- FR-005 remains partially implemented because initial status-change notification behaviour exists, while the complete submit/reject/update/complete feedback flow has not yet been demonstrated.

- FR-010 remains partially implemented because lifecycle-transition validation and persistence are implemented, while staff authorisation under AC-010.1 remains outstanding.

- NFR-003 has persistence-mechanism evidence, but the complete service-request submission flow has not yet been implemented and verified.

- NFR-005 has direct initial implementation and verification evidence through `StatusHistory` persistence.

- The External Location Provider Boundary establishes a provider-independent integration point. A live external geocoding provider remains future work.

- Requirements without direct implementation evidence remain baselined for future milestones.

---

# Milestone 2 RTM Baseline Statement

This RTM represents the evolved CivicConnect requirements traceability baseline for Milestone 2.

It connects the approved requirements baseline to the architecture, data model, technology decisions, design decisions, initial application implementation and initial verification evidence available at this milestone.

The matrix will continue to evolve in subsequent milestones as additional CivicConnect requirements are implemented, tested and prepared for release.
