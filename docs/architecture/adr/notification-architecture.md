# ADR — Notification Architecture

**Project:** CivicConnect  
**Milestone:** Milestone 2  
**Status:** Proposed  
**Owner:** Member 3  
**Decision Type:** Design / Architecture  
**Related Concern:** FEC-06  
**Date:** 2026-09-30  

---

## 1. Context

CivicConnect must provide feedback to requesters when relevant events occur
during the lifecycle of a service request.

The Milestone 1 requirements identify requester feedback for events such as
submission, rejection, update and completion.

The relevant traceability chain is:

FR-005
    |
AC-005.1
    |
NFR-001
    |
FEC-06
    |
Notification Architecture

FEC-06 identified notification behaviour as a forward-engineering concern
requiring a more detailed design decision.

The notification architecture must prevent core service-request business logic
from becoming tightly coupled to external notification technologies.

Assignment 2 investigated several alternatives, including:

- Observer / event-driven notification;
- Strategy + Factory;
- direct notification helpers/services.

These alternatives were evaluated against the actual CivicConnect M2
architecture and technology decisions in:

`docs/design/notification-architecture-evaluation.md`

---

## 2. Decision Drivers

The notification architecture should:

1. keep notification-provider logic outside the Domain;
2. reduce coupling between service-request lifecycle behaviour and
   communication infrastructure;
3. support automated testing;
4. allow notification implementations to be replaced;
5. support future notification channels;
6. align with the Modular Monolith architecture;
7. preserve Clean/Hexagonal-style boundaries;
8. support NFR-001 performance objectives;
9. avoid unnecessary external-provider work inside database transactions;
10. maintain traceability from requirement to implementation.

---

## 3. Alternatives Considered

### Option A — Observer / Event-Driven Approach

Important service-request lifecycle occurrences generate events.

Application-level handlers react to those events and coordinate notification
behaviour.

Conceptually:

ServiceRequest
      |
Domain Event
      |
Notification Handler
      |
INotificationService
      |
Infrastructure Provider

Advantages:

- strong separation of concerns;
- good testability;
- supports multiple event handlers;
- reduces provider coupling;
- aligns with the selected architecture;
- supports future extension.

Disadvantages:

- introduces event classes and handlers;
- requires clear event dispatch behaviour;
- introduces additional application flow.

---

### Option B — Strategy + Factory

Notification channels implement a common strategy interface and a factory
selects the required implementation.

Advantages:

- strong support for interchangeable notification channels;
- useful when selecting between email, SMS, push or other strategies;
- testable.

Disadvantages:

- primarily solves which notification implementation should be used;
- does not by itself decouple the occurrence of a business event from
  notification orchestration.

Strategy may still be useful inside the notification subsystem in the future.

---

### Option C — Direct Notification Service

Service-request processing directly invokes a notification service.

Advantages:

- simple;
- fewer classes;
- fast to implement.

Disadvantages:

- stronger coupling;
- lifecycle logic becomes aware of notification processing;
- harder to extend;
- weaker separation between business behaviour and infrastructure.

---

## 4. Decision

CivicConnect will use an **Observer-style event-driven approach** for
notification triggering.

Meaningful service-request lifecycle events will be represented as events
where required by the approved requirements.

Application-level handlers will react to those events and coordinate
notification processing.

External notification delivery will be accessed through an abstraction such
as:

`INotificationService`

Provider-specific implementations will remain in Infrastructure.

---

## 5. Proposed Architecture

The intended design is:

Domain
|
| ServiceRequest lifecycle behaviour
|
v
Domain Event
|
| e.g. ServiceRequestStatusChanged
|
v
Application Handler
|
v
INotificationService
^
|
Infrastructure
|
v
Notification Provider

The Domain must not directly depend on:

- email providers;
- SMS providers;
- push-notification providers;
- provider SDKs;
- provider credentials.

---

## 6. Layer Responsibilities

### Domain

Responsible for representing meaningful business events.

Potential events may include:

- ServiceRequestSubmittedEvent;
- ServiceRequestRejectedEvent;
- ServiceRequestStatusChangedEvent;
- ServiceRequestCompletedEvent.

Only events required by the approved implementation should actually be
introduced.

### Application

Responsible for reacting to the event and coordinating the notification use
case.

Application code may depend on:

`INotificationService`

but should not depend directly on a specific provider.

### Infrastructure

Responsible for implementing external notification delivery.

Infrastructure may contain:

- email implementation;
- SMS implementation;
- push-notification implementation;
- provider SDK integration;
- external API configuration.

---

## 7. Transaction Boundary

External notification calls should not unnecessarily remain inside the
database transaction responsible for saving core service-request state.

The preferred conceptual flow is:

Validate business operation
        |
Update ServiceRequest
        |
Create required audit information
        |
Persist core state
        |
Dispatch/process relevant event
        |
Notification handling

The implementation must ensure that notification processing does not
incorrectly report a successful state change that failed to persist.

---

## 8. Performance Impact

NFR-001 requires at least 95% of normal user actions to complete within
3 seconds under the workload agreed by the team.

External provider latency may affect this requirement.

The event-driven architecture provides a boundary through which notification
processing can be separated from core business processing.

However, event-driven architecture alone does not guarantee compliance with
NFR-001.

Actual performance must be verified through testing.

---

## 9. Failure Handling

A temporary external notification-provider failure should not automatically
corrupt successfully persisted service-request state.

Provider failures should remain isolated behind the notification abstraction.

Detailed retry, queueing or guaranteed-delivery mechanisms are not selected
by this ADR unless required by the current M2 implementation.

They may be evaluated later if the project requirements justify them.

---

## 10. Positive Consequences

The decision provides:

- strong separation between lifecycle logic and notification infrastructure;
- improved testability;
- reduced provider coupling;
- easier provider replacement;
- support for additional event handlers;
- compatibility with the Modular Monolith architecture;
- compatibility with Clean/Hexagonal-style boundaries;
- compatibility with ASP.NET Core dependency injection.

---

## 11. Negative Consequences / Trade-Offs

The decision introduces:

- event definitions;
- event dispatching;
- event handlers;
- notification abstractions;
- additional application flow.

The team must also define clear event ownership to avoid unnecessary or
duplicate events.

---

## 12. Risks and Mitigations

| Risk | Mitigation |
|---|---|
| Excessive event complexity | Create only meaningful events |
| Provider coupling | Use `INotificationService` |
| Slow external provider | Keep provider concerns outside core transaction where appropriate |
| Provider failure | Isolate failure in Infrastructure |
| Duplicate notifications | Define clear handler ownership |
| Difficult testing | Use fake/mock notification implementations |
| Missing traceability | Link event, handler and tests through RTM |

---

## 13. Verification

The decision will be verified through implementation evidence demonstrating:

- required lifecycle events are generated;
- notification handlers respond to the correct events;
- Application/Domain code does not directly depend on a provider;
- `INotificationService` can be replaced by a fake implementation;
- automated tests verify event and handler behaviour;
- relevant implementation and test evidence is linked in the RTM.

---

## 14. Traceability

Decision trace:

FR-005
   |
AC-005.1
   |
NFR-001
   |
FEC-06
   |
Assignment 2 Research
   |
Notification Architecture Evaluation
   |
This ADR
   |
Implementation
   |
Automated Tests
   |
RTM Evidence

Related document:

`docs/design/notification-architecture-evaluation.md`

---

## 15. Decision Status

**PROPOSED**

This ADR becomes **ACCEPTED** after team review and approval.

Implementation and test evidence must be added separately once the
corresponding code exists.
