# CivicConnect External Location Provider Boundary Evaluation

**Milestone:** Milestone 2  
**Document Type:** Design Evaluation  
**Project:** CivicConnect  
**Status:** Proposed – Pending Team Review  
**Owner:** Member 3  
**Design Problem:** External Location Provider Boundary  

---

# 1. Purpose

The purpose of this document is to evaluate and select an appropriate
architectural boundary between CivicConnect and an external location or
geocoding provider.

CivicConnect may require external location functionality to translate or
resolve location information used by service requests.

Directly coupling the CivicConnect Domain or Application layers to a specific
external provider would introduce infrastructure-specific dependencies into
the core application.

This evaluation therefore determines how CivicConnect should isolate the
external location provider while preserving:

- architectural boundaries;
- maintainability;
- testability;
- provider independence;
- failure isolation;
- future replaceability.

The evaluation uses:

- Milestone 1 requirements and engineering concerns;
- Assignment 2 research;
- Member 1's Modular Monolith architecture;
- Clean/Hexagonal-style boundaries;
- the M2 technology stack;
- the M2 persistence decisions.

---

# 2. Problem Statement

CivicConnect may need to communicate with an external location service.

A direct implementation could look like:

Application
     |
     v
Specific External Location SDK/API
     |
     v
External Provider

This creates a direct dependency between CivicConnect application logic and a
specific external provider.

Provider-specific concepts could then spread throughout the application.

For example:

- provider request objects;
- provider response objects;
- provider error codes;
- API-specific configuration;
- API-specific authentication;
- provider-specific coordinates or address formats.

This creates a risk of vendor coupling.

The design problem is therefore:

> How should CivicConnect access external location functionality while keeping
> provider-specific implementation details outside the Domain and Application
> core?

---

# 3. Assignment 2 Research Input

Assignment 2 investigated alternative approaches for isolating external
location/geocoding providers.

The alternatives considered were:

1. Adapter;
2. Facade / Gateway;
3. Direct Wrapper.

The Assignment 2 research indicated that a small Adapter behind an
`ILocationResolver` abstraction provides a strong candidate approach.

However, Assignment 2 research is input to the M2 decision.

The final decision must be evaluated against the actual CivicConnect
architecture and technology stack.

---

# 4. Design Drivers

The external location boundary should support the following engineering
drivers.

## 4.1 Provider Independence

Core CivicConnect logic should not depend directly on one external location
provider.

---

## 4.2 Maintainability

Changing the external provider should require changes primarily within the
Infrastructure implementation rather than throughout the application.

---

## 4.3 Testability

Application behaviour should be testable without making real external API
requests.

---

## 4.4 Architecture Compatibility

The solution must respect the selected architecture:

Presentation
      |
      v
Application
      |
      v
Domain

Infrastructure handles external technical dependencies.

---

## 4.5 Failure Isolation

External providers may:

- become unavailable;
- respond slowly;
- return invalid data;
- change their API;
- enforce rate limits;
- return provider-specific errors.

These concerns should not unnecessarily leak into the Domain model.

---

## 4.6 Security

External provider credentials must not be hard-coded or committed to GitHub.

---

## 4.7 Performance

External calls may increase request latency.

The design must therefore allow provider latency to be handled without
embedding provider-specific behaviour throughout CivicConnect.

---

# 5. Option A — Adapter Pattern

The Adapter pattern places an application-defined abstraction between
CivicConnect and the external provider.

Conceptually:

Application
     |
     v
ILocationResolver
     ^
     |
LocationProviderAdapter
     |
     v
External Provider
```

The Application depends on:

`ILocationResolver`

rather than:

`GoogleMapsClient`

or:

`SomeSpecificGeocodingSDK`

The Infrastructure layer contains the provider-specific Adapter.

---

# 6. Example Adapter Structure

Conceptually:

```text
CivicConnect.Application
|
+-- Abstractions/
    |
    +-- ILocationResolver


CivicConnect.Infrastructure
|
+-- Location/
    |
    +-- ExternalLocationProviderAdapter
```

The interface defines what CivicConnect requires.

The Adapter translates that application-level request into the format expected
by the external provider.

It then translates the provider response back into a CivicConnect-owned model.

---

# 7. Example Application Abstraction

The design may conceptually use an interface such as:

```csharp
public interface ILocationResolver
{
    Task<LocationResult?> ResolveAsync(
        string address,
        CancellationToken cancellationToken = default);
}
```

This is illustrative design code.

It is not implementation evidence until the corresponding code is actually
committed to the project.

---

# 8. Example Application Model

Instead of exposing an external provider's response type to the Application,
CivicConnect can own its own result model.

Conceptually:

```csharp
public sealed record LocationResult(
    decimal Latitude,
    decimal Longitude);
```

Therefore:

```text
Application
     |
     | CivicConnect types
     v
ILocationResolver
     |
     v
Adapter
     |
     | translates
     v
Provider-specific types
```

---

# 9. Advantages of Adapter

## Provider Isolation

The external provider is isolated behind a CivicConnect-owned abstraction.

## Replaceability

If the provider changes:

```text
Provider A
   ↓
Provider B
```

the Application does not necessarily need to change.

Instead:

```text
ILocationResolver
       ^
       |
OldProviderAdapter
```

can become:

```text
ILocationResolver
       ^
       |
NewProviderAdapter
```

## Testability

Tests can replace the real provider with:

```text
FakeLocationResolver
```

or a mock implementation.

No real external API request is required for most Application tests.

## Clean Architecture Alignment

The Application defines the capability it needs.

Infrastructure implements that capability.

This follows dependency inversion.

---

# 10. Limitations of Adapter

The Adapter introduces additional:

- interface definitions;
- mapping code;
- Infrastructure classes;
- error translation;
- testing responsibilities.

If CivicConnect only required one extremely simple external call, this may
appear more complex than calling the provider directly.

However, the separation becomes valuable when external dependencies change or
must be tested independently.

---

# 11. Option B — Facade / Gateway

Another alternative is a broader location Gateway or Facade.

For example:

```text
Application
     |
     v
LocationGateway
     |
     +---- Geocoding
     |
     +---- Reverse Geocoding
     |
     +---- Distance Calculation
     |
     +---- Provider API
```

A Gateway can provide one entry point for several related external
capabilities.

---

# 12. Advantages of Facade / Gateway

A Gateway can:

- simplify access to several related location operations;
- hide multiple external APIs;
- centralise external integration;
- provide a single application-facing entry point.

This can be useful when CivicConnect has many related location capabilities.

---

# 13. Limitations of Facade / Gateway

A broad Gateway can become unnecessarily large if CivicConnect currently
requires only a small location capability.

For example:

```text
ILocationGateway

ResolveAddress()
ReverseGeocode()
CalculateDistance()
FindNearestOffice()
ValidateRegion()
GetRoute()
```

would be unnecessary if the application currently only needs location
resolution.

This could violate interface-segregation principles by making the application
depend on capabilities it does not require.

For the current M2 scope, a small focused abstraction is preferable.

---

# 14. Option C — Direct Wrapper / Direct Provider Integration

The simplest alternative is to create a provider-specific wrapper or call the
provider directly.

For example:

```text
ServiceRequestService
        |
GoogleMapsService
        |
Google API
```

or another provider-specific implementation.

---

# 15. Advantages of Direct Integration

- simple initial implementation;
- fewer abstractions;
- fewer classes;
- easy for a small prototype.

---

# 16. Limitations of Direct Integration

Direct integration introduces stronger provider coupling.

Application code may become dependent on:

- provider classes;
- provider response models;
- provider exceptions;
- provider configuration;
- provider-specific terminology.

Changing providers could therefore require modifications across several
application components.

Testing may also require more provider-specific mocking.

For CivicConnect's selected architecture, this provides weaker separation than
the Adapter approach.

---

# 17. Comparison

| Criterion | Adapter | Facade / Gateway | Direct Wrapper |
|---|---|---|---|
| Provider isolation | Strong | Strong | Weak/Moderate |
| Application-owned abstraction | Strong | Strong | Limited |
| Replaceability | Strong | Strong | Weaker |
| Testability | Strong | Strong | Moderate |
| Suitable for one focused capability | Strong | Can be excessive | Strong |
| Suitable for many external location operations | Moderate | Strong | Weak |
| Clean Architecture compatibility | Strong | Strong | Weaker |
| Initial complexity | Moderate | Moderate/High | Low |
| Prevents provider types leaking inward | Strong | Strong | Depends on implementation |
| Fit for current CivicConnect scope | Strong | Possible but broader than needed | Limited |

---

# 18. Selected Approach

## Decision

**Use a focused Adapter behind an application-owned `ILocationResolver`
abstraction.**

The CivicConnect Application layer will depend on the capability it requires
rather than a specific external location provider.

The Infrastructure layer will implement the abstraction using the selected
provider.

Conceptually:

```text
CivicConnect.Application
          |
   ILocationResolver
          ^
          |
CivicConnect.Infrastructure
          |
LocationProviderAdapter
          |
External Location Provider
```

---

# 19. Why Adapter Was Selected

Adapter provides the strongest fit for the current problem because
CivicConnect requires a focused external-provider boundary rather than a broad
location subsystem.

The application needs to express:

> Resolve this location.

It should not need to express:

> Call provider X using provider X's request type and interpret provider X's
> response.

The Adapter handles that translation.

This preserves CivicConnect-owned interfaces and models inside the Application
boundary.

---

# 20. Why Facade / Gateway Was Not Selected as the Primary Approach

Facade/Gateway is a valid architectural approach.

However, the current CivicConnect scope does not provide sufficient evidence
that a broad location Gateway is necessary.

Creating a large Gateway before those capabilities exist could introduce
unnecessary complexity.

If CivicConnect later requires several location capabilities or several
external location systems, the boundary can be reconsidered.

---

# 21. Why Direct Integration Was Not Selected

Direct provider integration provides the least architectural separation.

It risks allowing provider-specific concepts to spread into Application code.

For example:

```text
Application
     |
ProviderSDK.GeocodeRequest
```

creates a dependency on the provider's API model.

The preferred approach is:

```text
Application
     |
     v
ILocationResolver
```

with provider translation occurring in Infrastructure.

---

# 22. Proposed M2 Design

The proposed design is:

```text
┌─────────────────────────────┐
│       Application           │
│                             │
│ Uses location capability    │
└─────────────┬───────────────┘
              │
              │ depends on
              ▼
┌─────────────────────────────┐
│     ILocationResolver       │
│                             │
│ Application-owned port      │
└─────────────▲───────────────┘
              │
              │ implements
┌─────────────┴───────────────┐
│       Infrastructure        │
│                             │
│ LocationProviderAdapter     │
└─────────────┬───────────────┘
              │
              │ external request
              ▼
┌─────────────────────────────┐
│ External Location Provider  │
└─────────────────────────────┘
```

---

# 23. Layer Responsibilities

## Domain

The Domain should contain business concepts relevant to CivicConnect.

The Domain should not:

- call external location APIs;
- reference provider SDKs;
- contain API keys;
- understand provider-specific response objects.

---

## Application

The Application defines the capability required by CivicConnect.

For example:

```text
ILocationResolver
```

The Application may also own the models required to represent the result.

The Application does not need to know which external provider implements the
interface.

---

## Infrastructure

Infrastructure contains:

- provider-specific HTTP/API communication;
- provider authentication;
- request translation;
- response translation;
- provider-specific error handling;
- Adapter implementation.

---

# 24. Dependency Direction

The dependency direction is important.

Incorrect:

```text
Application
    |
External Provider SDK
```

Preferred:

```text
Application
    |
ILocationResolver
    ^
    |
Infrastructure Adapter
    |
External Provider
```

The Application owns the abstraction.

Infrastructure depends on and implements that abstraction.

---

# 25. Dependency Injection

ASP.NET Core dependency injection can connect the Application abstraction to
the Infrastructure implementation.

Conceptually:

```csharp
services.AddScoped<ILocationResolver, LocationProviderAdapter>();
```

This is illustrative design code.

The actual registration will become implementation evidence once committed.

---

# 26. Testing Approach

The Adapter design allows Application tests to replace the real provider.

For example:

```text
Application Test
      |
FakeLocationResolver
```

instead of:

```text
Application Test
      |
Internet
      |
External Provider
```

This makes tests:

- faster;
- more predictable;
- independent of provider availability;
- independent of external API usage limits.

---

# 27. Example Fake Resolver

Conceptually:

```csharp
public sealed class FakeLocationResolver : ILocationResolver
{
    public Task<LocationResult?> ResolveAsync(
        string address,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult<LocationResult?>(
            new LocationResult(
                Latitude: -25.0m,
                Longitude: 28.0m));
    }
}
```

The values above are illustrative test data only.

The actual test implementation may use different deterministic values.

---

# 28. Failure Handling

External location providers can fail.

Possible failures include:

- timeout;
- provider unavailable;
- invalid address;
- rate limit exceeded;
- authentication failure;
- malformed provider response.

The Adapter should translate provider-specific failure behaviour into
application-appropriate results or errors.

Provider-specific exceptions should not unnecessarily leak into the Domain.

For example:

```text
ProviderTimeoutException
```

should not become a required Domain dependency.

---

# 29. Timeout Considerations

External requests must not be allowed to wait indefinitely.

The Infrastructure implementation should use an appropriate timeout policy.

The exact timeout value should be selected based on the provider,
application behaviour and performance testing rather than invented in this
design document.

---

# 30. Retry Considerations

Some temporary provider failures may later justify retries.

However, automatic retry behaviour must be used carefully.

Retries can:

- increase latency;
- increase external API usage;
- worsen provider overload;
- affect rate limits.

The exact retry policy is therefore not selected in this M2 design unless
required by the implemented functional slice.

---

# 31. Security

External provider credentials must remain outside source-controlled code.

The following must not be committed:

```text
API keys
Provider passwords
Client secrets
Connection credentials
```

Configuration should be supplied using appropriate development/deployment
configuration mechanisms.

---

# 32. Privacy and Data Exposure

Only information required for the location operation should be sent to the
external provider.

The integration should avoid sending unrelated CivicConnect user or
service-request information.

For example, if the provider only requires an address or coordinates, the
integration should not send unrelated user account information.

---

# 33. Performance

External location resolution introduces network latency.

This may affect NFR-001 depending on where location resolution occurs in the
user flow.

The Adapter pattern does not itself solve the performance problem.

The actual implementation must be measured.

Possible future optimisation approaches could include:

- caching;
- asynchronous processing;
- provider-specific optimisation.

These should only be introduced when justified by measured behaviour and
requirements.

---

# 34. Persistence Boundary

The external provider Adapter should not directly determine CivicConnect's
database model.

For example:

```text
External Provider Response
```

should first be translated into:

```text
CivicConnect LocationResult
```

before the application decides what information, if any, should be persisted.

This prevents the database schema from becoming unnecessarily coupled to an
external provider's response structure.

---

# 35. Provider Replacement Example

With the Adapter design:

```text
TODAY

ILocationResolver
      ^
      |
ProviderAAdapter
```

can later become:

```text
FUTURE

ILocationResolver
      ^
      |
ProviderBAdapter
```

without requiring core Application use cases to directly depend on the new
provider's API.

This is one of the main reasons for selecting the Adapter approach.

---

# 36. Relationship to Member 1 Architecture

The design aligns with the selected Modular Monolith and Clean/Hexagonal-style
boundaries.

Conceptually:

```text
Presentation
      |
Application
      |
      +---- ILocationResolver
      |
Domain


Infrastructure
      |
      +---- LocationProviderAdapter
      |
External Provider
```

Provider-specific code remains outside the Domain.

---

# 37. Relationship to Member 2 Technology Decision

The design is compatible with the selected/proposed .NET stack.

C# provides:

- interfaces;
- dependency injection compatibility;
- async/await;
- test doubles;
- typed result models.

ASP.NET Core provides dependency injection and external HTTP integration
capabilities.

xUnit can test the Application and Adapter behaviour.

---

# 38. Relationship to Persistence Decision

The Adapter design does not require the Domain or Application to depend on
SQL Server or EF Core.

Location resolution and persistence remain separate concerns.

Conceptually:

```text
               ILocationResolver
                     ^
                     |
Application ----------+
     |
Persistence Abstraction
     ^
     |
Infrastructure
     |
     +---- EF Core → SQL Server
     |
     +---- Location Adapter → External Provider
```

Both are Infrastructure capabilities exposed to the Application through
appropriate boundaries.

---

# 39. Positive Consequences

The selected Adapter design provides:

- provider independence;
- clear dependency direction;
- improved testability;
- easier provider replacement;
- isolation of provider-specific models;
- isolation of provider-specific errors;
- compatibility with Clean/Hexagonal architecture;
- compatibility with ASP.NET Core dependency injection;
- a small interface focused on CivicConnect's needs.

---

# 40. Negative Consequences / Trade-Offs

The design introduces:

- an additional interface;
- an Adapter implementation;
- translation code;
- provider error mapping;
- additional tests.

There is also a risk of creating an abstraction that is too generic.

The interface should therefore remain focused on CivicConnect's actual
requirements rather than attempting to represent every capability offered by
the external provider.

---

# 41. Risks and Mitigations

| Risk | Mitigation |
|---|---|
| Provider lock-in | Application-owned interface and Adapter |
| Provider API changes | Isolate changes inside Infrastructure Adapter |
| Provider downtime | Define controlled failure behaviour |
| Slow API calls | Timeouts and performance testing |
| Rate limiting | Avoid unnecessary calls; evaluate caching if justified |
| API key exposure | Secure configuration; never commit secrets |
| Provider types leak into Application | Translate to CivicConnect-owned models |
| Difficult automated testing | Fake/mock `ILocationResolver` |
| Oversized abstraction | Keep interface focused on actual use cases |

---

# 42. Selected Decision

CivicConnect will use the **Adapter pattern** to isolate external location
providers.

The Application layer will define a focused:

`ILocationResolver`

abstraction representing the location capability required by CivicConnect.

The Infrastructure layer will provide the provider-specific Adapter.

External provider request models, response models, authentication,
communication and error handling will remain within Infrastructure.

The Application and Domain layers will not directly depend on the selected
external provider SDK or API.

---

# 43. Traceability

The design trace is:

```text
M1 Requirements / Engineering Concerns
        |
Assignment 2 Location Provider Research
        |
Adapter vs Gateway vs Direct Wrapper
        |
M2 Adapter Decision
        |
Location Provider ADR
        |
ILocationResolver
        |
Infrastructure Adapter
        |
Automated Tests
        |
RTM Evidence
```

Exact requirement/acceptance-criterion identifiers should be linked in the
RTM from the approved requirements baseline rather than invented in this
document.

---

# 44. Verification Plan

The design will later be verified through implementation and tests.

Planned verification includes:

- Application code depends on `ILocationResolver`, not a provider SDK;
- Infrastructure implements `ILocationResolver`;
- provider response objects do not leak into the Domain/Application;
- a fake resolver can replace the real Adapter in automated tests;
- successful location resolution is mapped correctly;
- invalid/unresolved locations are handled according to approved behaviour;
- provider failures are translated appropriately;
- provider credentials are not stored in source code.

Actual implementation and test paths will be added once they exist.

---

# 45. Decision Status

**Status: PROPOSED**

The Adapter approach becomes **ACCEPTED** after team review.

This document currently records a design decision only.

It must not claim that the location integration has been implemented or
verified until corresponding code and automated tests exist.

---

# 46. Next Actions

- [ ] Review the Adapter decision.
- [ ] Confirm `ILocationResolver` as the application boundary.
- [ ] Create the Location Provider ADR.
- [ ] Reconcile the final ADR number with the team's ADR register.
- [ ] Add the design decision to the PED.
- [ ] Add design evidence to the RTM.
- [ ] Implement `ILocationResolver`.
- [ ] Implement a provider Adapter or test implementation.
- [ ] Register the implementation through dependency injection.
- [ ] Add automated tests.
- [ ] Add implementation/test evidence to the RTM.
