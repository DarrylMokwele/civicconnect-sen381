# ADR — External Location Provider Boundary

**Project:** CivicConnect  
**Milestone:** Milestone 2  
**Status:** Proposed  
**Owner:** Member 3  
**Decision Type:** Design / Integration  
**Date:** 2026-09-30  

---

## 1. Context

CivicConnect may require an external location or geocoding provider to resolve
location information associated with service requests.

Directly coupling the Application or Domain to a specific external provider
would introduce provider-specific dependencies into the core application.

These dependencies could include:

- provider SDK classes;
- provider request models;
- provider response models;
- provider error types;
- provider authentication;
- provider-specific terminology.

Assignment 2 investigated approaches for isolating external location
providers.

The alternatives included:

- Adapter;
- Facade / Gateway;
- Direct Wrapper / direct provider integration.

These alternatives were evaluated against the CivicConnect M2 architecture
in:

`docs/design/location-provider-evaluation.md`

---

## 2. Decision Drivers

The location-provider boundary should:

1. isolate provider-specific implementation;
2. preserve Clean/Hexagonal-style architecture;
3. support provider replacement;
4. support automated testing without external API calls;
5. prevent provider models from leaking into core application code;
6. support controlled provider failure handling;
7. protect provider credentials;
8. remain small and focused on actual CivicConnect requirements;
9. support ASP.NET Core dependency injection.

---

## 3. Alternatives Considered

### Option A — Adapter

The Application defines a focused abstraction representing the capability it
requires.

For example:

`ILocationResolver`

Infrastructure implements that interface using the selected provider.

Conceptually:

Application
      |
ILocationResolver
      ^
      |
Infrastructure Adapter
      |
External Provider

Advantages:

- strong provider isolation;
- provider replacement;
- strong testability;
- prevents provider-specific models from leaking inward;
- aligns with dependency inversion;
- fits Clean/Hexagonal-style architecture.

Disadvantages:

- requires an additional interface;
- requires mapping between application and provider models;
- introduces Adapter implementation code.

---

### Option B — Facade / Gateway

A broader Gateway provides several location-related capabilities.

For example:

LocationGateway
     |
     +-- ResolveAddress
     +-- ReverseGeocode
     +-- CalculateDistance
     +-- Other operations

Advantages:

- useful when several external location capabilities exist;
- centralises external integration;
- can hide multiple provider systems.

Disadvantages:

- broader than the currently demonstrated CivicConnect requirement;
- may introduce unnecessary methods and complexity;
- risks creating a large general-purpose interface.

---

### Option C — Direct Provider Integration

Application code directly uses a provider-specific service, wrapper or SDK.

Advantages:

- simplest initial implementation;
- fewer abstractions.

Disadvantages:

- stronger provider coupling;
- provider models may leak into Application code;
- provider replacement becomes more difficult;
- testing becomes more provider-specific;
- weaker alignment with the selected architecture.

---

## 4. Decision

CivicConnect will use the **Adapter pattern** for the external location
provider boundary.

The Application layer will own a focused abstraction:

`ILocationResolver`

Infrastructure will provide the provider-specific implementation.

Conceptually:

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

---

## 5. Application-Owned Boundary

The abstraction must represent what CivicConnect needs rather than copying
the external provider's API.

Conceptually:

```csharp
public interface ILocationResolver
{
    Task<LocationResult?> ResolveAsync(
        string address,
        CancellationToken cancellationToken = default);
}
