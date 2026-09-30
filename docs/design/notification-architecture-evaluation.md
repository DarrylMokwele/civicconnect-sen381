# CivicConnect Notification Architecture Evaluation

**Milestone:** Milestone 2  
**Document Type:** Design Evaluation  
**Project:** CivicConnect  
**Status:** Accepted
**Owner:** Member 3  
**Related Forward Engineering Concern:** FEC-06  
**Design Problem:** Notification / Feedback Architecture  

---

# 1. Purpose

The purpose of this document is to evaluate and select an appropriate
notification architecture for CivicConnect.

Milestone 1 identified notification and feedback behaviour as an area requiring
further engineering work.

CivicConnect must provide feedback to requesters when important events occur
during the service-request lifecycle.

The notification mechanism must therefore be designed so that notification
logic does not become tightly coupled to the core ServiceRequest business
logic.

This document evaluates alternative notification approaches using:

- Milestone 1 requirements;
- FEC-06;
- Assignment 2 research;
- the M2 Modular Monolith architecture;
- Clean/Hexagonal-style boundaries;
- the proposed C# / ASP.NET Core technology stack;
- the proposed persistence strategy.

The selected approach will later be implemented and verified through code and
automated tests.

---

# 2. Problem Statement

CivicConnect needs to notify or provide feedback to requesters when relevant
service-request lifecycle events occur.

A poor implementation could place notification code directly inside request
processing logic.

For example:

ServiceRequest
    |
    +-- Change status
    |
    +-- Save request
    |
    +-- Send email
    |
    +-- Send SMS
    |
    +-- Send push notification

This would tightly couple core business behaviour to communication
technologies.

It could also make the system harder to:

- maintain;
- test;
- extend;
- replace notification providers;
- handle notification failures;
- support additional notification channels.

The design problem is therefore:

> How should CivicConnect trigger and process requester notifications while
> keeping notification concerns decoupled from core service-request business
> logic?

---

# 3. M1 Traceability

The notification design originates from the requirements and forward
engineering work established during Milestone 1.

The primary traceability chain is:

FR-005
   |
   v
AC-005.1
   |
   v
NFR-001
   |
   v
FEC-06
   |
   v
M2 Notification Architecture

FEC-06 identified notification behaviour as a concern requiring a more
detailed engineering decision during forward engineering.

---

# 4. Functional Requirement — FR-005

FR-005 requires CivicConnect to provide requester feedback when relevant
service-request events occur.

The identified lifecycle events include:

- submission;
- rejection;
- update;
- completion.

The notification architecture must therefore support multiple event types
without requiring the ServiceRequest business logic to contain direct
provider-specific communication code.

---

# 5. Acceptance Criterion — AC-005.1

AC-005.1 provides the acceptance evidence associated with requester feedback.

The M2 notification design must support implementation and testing of this
behaviour.

The exact wording and verification evidence for AC-005.1 should remain
consistent with the approved requirements baseline and RTM.

---

# 6. Non-Functional Requirement — NFR-001

NFR-001 requires at least 95% of normal user actions to complete within
3 seconds under the workload agreed by the team.

Notification processing can affect this requirement if external provider calls
are performed directly during the user's request.

For example:

User action
    |
    v
Update Service Request
    |
    v
Call external notification provider
    |
    v
WAIT
    |
    v
Provider responds
    |
    v
User receives response

If the external provider is slow, the normal CivicConnect operation could also
be delayed.

The notification design should therefore avoid unnecessary coupling between
the core user operation and slow external-provider communication.

The exact performance effect must later be measured rather than assumed.

---

# 7. Forward Engineering Concern — FEC-06

FEC-06 identified the need to determine how notification behaviour should be
structured during forward engineering.

The concern is not simply:

> Which email or SMS service should CivicConnect use?

The architectural concern is:

> How should the application separate the business event that requires
> notification from the infrastructure responsible for delivering the
> notification?

This distinction is important because external communication technology may
change while the underlying business event remains the same.

---

# 8. Assignment 2 Research Input

Assignment 2 investigated alternative approaches for notification-related
behaviour.

The main alternatives considered were:

1. Observer / event-driven notification;
2. Strategy + Factory;
3. Direct helper/service calls.

These alternatives are now evaluated against the actual CivicConnect M2
architecture and technology decisions.

Assignment 2 research is used as input to the decision rather than being
treated as the final project decision automatically.

---

# 9. Design Drivers

The notification architecture should satisfy the following design drivers.

## 9.1 Decoupling

ServiceRequest business logic should not directly depend on email, SMS, push
notification or another external delivery provider.

## 9.2 Maintainability

Notification behaviour should be changeable without requiring unnecessary
changes to core domain logic.

## 9.3 Testability

The team should be able to test:

- whether the correct business event occurs;
- whether the correct handler reacts;
- whether a notification service is invoked;

without requiring a real external provider for every automated test.

## 9.4 Extensibility

Future notification channels should be addable without rewriting unrelated
business logic.

## 9.5 Performance

External notification processing should not unnecessarily increase the
response time of normal user operations.

## 9.6 Architecture Compatibility

The solution must fit the selected:

- Modular Monolith;
- Clean/Hexagonal-style boundaries;
- Domain/Application/Infrastructure separation.

## 9.7 Technology Compatibility

The approach should be implementable using the selected/proposed:

- C#;
- ASP.NET Core;
- dependency injection;
- xUnit;
- EF Core where persistence is required.

---

# 10. Option A — Observer / Event-Driven Approach

The first alternative is an Observer-style/event-driven approach.

When an important business event occurs, the responsible business/application
logic raises an event.

One or more handlers react to the event.

Conceptually:

ServiceRequest
      |
      | lifecycle change
      v
Domain Event
      |
      +----------+
      |          |
      v          v
Notification   Other Handler
Handler        if required
      |
      v
Notification Service
```

Example event:

```text
ServiceRequestStatusChanged
```

or:

```text
ServiceRequestCompleted
```

A notification handler can then react to the event.

---

# 11. Advantages of Option A

## Decoupling

The object responsible for the business operation does not need to know how
the notification is delivered.

For example:

```text
ServiceRequest

does NOT need to know about:

EmailService
SmsService
PushNotificationService
```

Instead, it only represents the relevant business event.

## Extensibility

Additional handlers may later react to the same event.

For example:

```text
RequestCompletedEvent
        |
        +----> NotificationHandler
        |
        +----> AuditHandler
        |
        +----> ReportingHandler
```

The exact handlers should only be introduced when required.

## Testability

The event-generation behaviour and notification handling can be tested
separately.

## Architecture Compatibility

Events fit naturally across the Domain/Application boundary while
provider-specific delivery remains in Infrastructure.

---

# 12. Limitations of Option A

The event-driven approach introduces additional concepts such as:

- event definitions;
- event dispatching;
- event handlers;
- provider abstractions.

This can make the system more complex than direct method calls.

Event processing also requires clear decisions about:

- synchronous vs asynchronous handling;
- failure handling;
- transaction boundaries;
- duplicate processing if asynchronous delivery is introduced later.

The team should therefore avoid creating unnecessary event infrastructure for
simple behaviour.

---

# 13. Option B — Strategy + Factory

The second alternative is Strategy combined with Factory.

Different notification channels can implement a common interface.

For example:

INotificationStrategy
       |
       +---- EmailNotificationStrategy
       |
       +---- SmsNotificationStrategy
       |
       +---- PushNotificationStrategy
```

A factory can select the appropriate implementation.

For example:

```text
NotificationFactory
       |
       | NotificationType = Email
       v
EmailNotificationStrategy
```

---

# 14. Advantages of Option B

Strategy is useful when CivicConnect must choose between multiple algorithms or
delivery channels.

It can provide:

- replaceable notification mechanisms;
- provider/channel separation;
- straightforward unit testing;
- extension with additional strategies.

A Factory can centralise the selection of the appropriate strategy.

---

# 15. Limitations of Option B

Strategy + Factory primarily solves:

> Which notification implementation should be used?

It does not by itself solve:

> How does the rest of CivicConnect become aware that a business event
> requiring notification occurred?

For example, the request lifecycle logic could still end up doing:

```text
ChangeStatus()

NotificationFactory.GetStrategy()

SendNotification()
```

This still couples lifecycle processing to notification orchestration.

Strategy may therefore be useful **inside** the notification subsystem, but it
does not provide the strongest solution to the overall FEC-06 decoupling
problem by itself.

---

# 16. Option C — Direct Notification Service / Helper

The simplest approach is to directly call a notification service.

For example:

```text
ChangeStatus()
{
    UpdateStatus();

    notificationService.Send(...);
}
```

---

# 17. Advantages of Option C

- simple;
- easy to understand;
- minimal supporting infrastructure;
- fast to implement for a small prototype.

---

# 18. Limitations of Option C

Direct calls introduce stronger coupling between request processing and
notification behaviour.

This can create problems when:

- notification channels change;
- multiple handlers are required;
- external providers fail;
- tests need to isolate business logic;
- notification behaviour expands;
- notification processing becomes asynchronous.

For CivicConnect, this approach provides weaker separation than the
event-driven alternative.

---

# 19. Comparison

| Criterion | Observer / Events | Strategy + Factory | Direct Service |
|---|---|---|---|
| Business/notification decoupling | Strong | Moderate | Weak |
| Supports multiple event subscribers | Strong | Not its main purpose | Weak |
| Supports multiple delivery strategies | Can be combined with Strategy | Strong | Moderate |
| Testability | Strong | Strong | Moderate |
| Extensibility | Strong | Strong | Limited |
| Initial simplicity | Moderate | Moderate | Strong |
| Clean Architecture compatibility | Strong | Strong | Possible but more coupled |
| Suitable for FEC-06 | Strong | Useful supporting pattern | Limited |

---

# 20. Selected Approach

## Decision

**Use an Observer-style event-driven approach for notification triggering.**

CivicConnect will represent important service-request lifecycle occurrences as
events.

Notification handlers will respond to relevant events and coordinate
notification processing.

Provider-specific delivery logic will remain behind abstractions and be
implemented within the appropriate Infrastructure boundary.

---

# 21. Why This Approach Was Selected

The event-driven approach provides the strongest fit for the actual FEC-06
problem.

The main problem is not only selecting between email, SMS or push
notifications.

The main problem is preventing core service-request lifecycle behaviour from
becoming tightly coupled to notification infrastructure.

The selected approach separates:

```text
Something happened
```

from:

```text
What should happen because of it?
```

For example:

```text
ServiceRequest completed
          |
          v
RequestCompletedEvent
          |
          v
RequesterNotificationHandler
          |
          v
Notification abstraction
          |
          v
Infrastructure implementation
```

The ServiceRequest does not need to know which external communication provider
will eventually be used.

---

# 22. Proposed M2 Design

The proposed M2 design is:

```text
┌──────────────────────────┐
│      ServiceRequest      │
│         Domain           │
└────────────┬─────────────┘
             │
             │ lifecycle event
             ▼
┌──────────────────────────┐
│       Domain Event       │
│ StatusChanged / Created  │
│ Rejected / Completed     │
└────────────┬─────────────┘
             │
             ▼
┌──────────────────────────┐
│       Application        │
│  Notification Handler    │
└────────────┬─────────────┘
             │
             │ depends on
             ▼
┌──────────────────────────┐
│ INotificationService     │
│      Abstraction         │
└────────────┬─────────────┘
             │
             │ implemented by
             ▼
┌──────────────────────────┐
│      Infrastructure      │
│ Notification Provider    │
└──────────────────────────┘
```

---

# 23. Layer Responsibilities

## Domain

The Domain represents the business event.

Potential examples:

```text
ServiceRequestSubmittedEvent

ServiceRequestRejectedEvent

ServiceRequestStatusChangedEvent

ServiceRequestCompletedEvent
```

Only events required by the approved requirements should actually be created.

The Domain must not:

- send email;
- send SMS;
- call external APIs;
- depend on provider SDKs.

---

## Application

The Application layer contains handlers/use cases responsible for reacting to
the event.

For example:

```text
ServiceRequestStatusChangedEvent
        |
        v
SendRequesterStatusNotificationHandler
```

The handler depends on an abstraction such as:

```text
INotificationService
```

---

## Infrastructure

Infrastructure implements external communication behaviour.

For example:

```text
INotificationService
        ^
        |
EmailNotificationService
```

or another provider implementation selected later.

The core Application/Domain layers should not depend directly on the provider.

---

# 24. Proposed Interface

The exact implementation will be finalised during coding, but the boundary may
conceptually resemble:

```csharp
public interface INotificationService
{
    Task SendAsync(
        NotificationMessage message,
        CancellationToken cancellationToken = default);
}
```

This is an architectural example rather than current implementation evidence.

---

# 25. Example Event

A lifecycle event may conceptually resemble:

```csharp
public sealed record ServiceRequestStatusChangedEvent(
    Guid ServiceRequestId,
    Guid RequesterId,
    string NewStatus);
```

Again, this represents the proposed design.

The final event structure must match the actual Domain model implemented by
the team.

---

# 26. Example Handler

Conceptually:

```csharp
public sealed class ServiceRequestStatusChangedHandler
{
    private readonly INotificationService _notificationService;

    public ServiceRequestStatusChangedHandler(
        INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task HandleAsync(
        ServiceRequestStatusChangedEvent domainEvent,
        CancellationToken cancellationToken)
    {
        // Build the appropriate requester notification.

        // Delegate delivery to the notification abstraction.

        await Task.CompletedTask;
    }
}
```

This code is illustrative only.

The actual implementation will be committed during the M2 development phase.

---

# 27. Relationship with Persistence

Notification delivery should not unnecessarily extend the transaction used to
save core service-request state.

The preferred conceptual flow is:

```text
Validate lifecycle operation
        |
        v
Update ServiceRequest
        |
        v
Create required StatusHistory
        |
        v
Persist core state
        |
        v
Process resulting notification behaviour
```

The exact event-dispatch timing must be implemented carefully so that
notification behaviour does not claim a successful state change that failed
to persist.

For M2, the team should keep the implementation simple while preserving this
boundary.

---

# 28. Failure Handling

An external notification failure should not automatically corrupt the
ServiceRequest state.

For example:

```text
ServiceRequest successfully updated
             |
             v
Notification attempted
             |
             X
Provider unavailable
```

The request should not silently revert simply because an external
communication provider is temporarily unavailable unless an approved business
requirement explicitly requires that behaviour.

The detailed retry/recovery mechanism is not yet selected.

This remains a future implementation concern unless required by the current
M2 scope.

---

# 29. Notification Persistence

The initial data model proposes a Notification entity.

If notification records are persisted, they may contain information such as:

- NotificationId;
- UserId;
- ServiceRequestId;
- NotificationType;
- Message;
- CreatedAt;
- SentAt;
- Status.

Whether every notification attempt must be persisted remains subject to the
approved persistence and business requirements.

The event-driven design does not require the Domain layer to depend on the
Notification database table.

---

# 30. Security Considerations

Notification content must not expose information that the recipient is not
authorised to access.

The notification implementation must therefore respect the access-control
requirements established for CivicConnect.

External provider credentials must not be:

- hard-coded;
- committed to GitHub;
- stored as ordinary source-controlled configuration.

Provider credentials should be supplied through appropriate secure
configuration mechanisms.

---

# 31. Performance Considerations

The notification architecture is designed to avoid unnecessary coupling
between external communication latency and core user operations.

However, simply using events does not guarantee NFR-001.

Performance verification must measure the actual implementation.

The team should test the relevant user operation under the agreed workload and
record the resulting evidence in the RTM.

---

# 32. Testability

The selected design supports independent automated testing.

Examples include:

## Test 1

Verify that the correct event is created when the relevant service-request
lifecycle operation succeeds.

## Test 2

Verify that an invalid lifecycle operation does not produce an inappropriate
notification event.

## Test 3

Verify that the notification handler invokes the notification abstraction.

## Test 4

Verify that the correct recipient/request information is supplied to the
notification abstraction.

## Test 5

Verify that the Application/Domain tests can use a fake notification service
instead of a real external provider.

Actual test references will be added to the RTM once the tests exist.

---

# 33. Architecture Compatibility

The selected design aligns with the Modular Monolith and Clean/Hexagonal-style
architecture.

```text
DOMAIN
ServiceRequest
     |
     v
Domain Event

APPLICATION
     |
     v
Notification Handler
     |
     v
INotificationService

INFRASTRUCTURE
     |
     v
Notification Provider
```

Dependencies continue to point toward the inner application/domain
boundaries rather than from the Domain toward external infrastructure.

---

# 34. Technology Compatibility

The design can be implemented using the M2 .NET technology direction.

C# supports:

- interfaces;
- records/classes for events;
- dependency injection;
- asynchronous handlers;
- test doubles.

ASP.NET Core provides dependency-injection infrastructure.

xUnit can verify the event and handler behaviour.

No external notification provider is required simply to prove the design
during the initial M2 implementation.

---

# 35. Positive Consequences

The selected design provides:

- reduced coupling;
- clearer separation of concerns;
- improved testability;
- easier extension;
- provider independence;
- compatibility with Member 1's architecture;
- compatibility with the selected .NET technology stack;
- a clear path from FEC-06 to implementation.

---

# 36. Negative Consequences / Trade-Offs

The design introduces:

- additional event classes;
- event handlers;
- notification abstractions;
- additional application flow to understand;
- decisions around event dispatch and failure handling.

For a very small application, direct calls would be simpler.

However, CivicConnect's lifecycle-driven notification requirements justify
the additional separation.

---

# 37. Risks and Mitigations

| Risk | Mitigation |
|---|---|
| Excessive event complexity | Create events only for meaningful business occurrences |
| Notification provider failure | Isolate provider behind abstraction |
| Slow external calls | Avoid unnecessarily coupling provider calls to core DB transaction |
| Duplicate notifications | Define clear handler ownership and later add idempotency if required |
| Domain coupled to provider | Keep provider implementation in Infrastructure |
| Difficult testing | Use interfaces/fakes and handler tests |
| Lost traceability | Link event, handler and tests through RTM |

---

# 38. Decision

CivicConnect will use an **Observer-style event-driven approach** for
notification triggering.

Service-request lifecycle behaviour will produce meaningful events where
required by the approved requirements.

Application-level handlers will react to those events and coordinate
notification behaviour.

External notification providers will be accessed through abstractions rather
than directly from the Domain layer.

Strategy may later be used inside the notification subsystem if CivicConnect
needs to dynamically select between multiple delivery channels.

Strategy + Factory is therefore considered a possible supporting pattern, not
the primary solution to FEC-06.

Direct provider/helper calls from core lifecycle logic are not selected
because they create stronger coupling.

---

# 39. Traceability

The current design trace is:

```text
FR-005
   |
   v
AC-005.1
   |
   v
NFR-001
   |
   v
FEC-06
   |
   v
Assignment 2 Notification Research
   |
   v
Observer / Event-Driven Decision
   |
   v
M2 Notification ADR
   |
   v
Implementation
   |
   v
Automated Tests
   |
   v
RTM Evidence
```

The implementation and test references will be added once they actually
exist.

---

# 40. Decision Status

**Status: PROPOSED**

The design becomes **ACCEPTED** after team review.

The document currently records the design decision only.

It must not claim that the notification architecture has been implemented or
verified until the corresponding code and automated tests exist.

---

# 41. Next Actions

- [ ] Review the notification architecture.
- [ ] Confirm the event-driven approach.
- [ ] Create the Notification Architecture ADR.
- [ ] Reconcile the ADR number with the team's ADR register.
- [ ] Update the PED with the selected design.
- [ ] Update the RTM with the design evidence.
- [ ] Implement the required domain event(s).
- [ ] Implement the notification handler.
- [ ] Implement `INotificationService`.
- [ ] Add a fake/test notification implementation.
- [ ] Add automated tests.
- [ ] Add implementation/test evidence to the RTM.
