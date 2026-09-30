# CivicConnect Technology Stack Evaluation

**Milestone:** Milestone 2  
**Document Type:** Technology Stack Evaluation  
**Project:** CivicConnect  
**Status:** Proposed – Pending Team Approval  
**Owner:** Member 2  
**Related Architecture:** Modular Monolith with Clean/Hexagonal-style boundaries  
**Related M1 Decision:** DEC-007 – Backend Technology, Persistence and Deployment deferred to M2  

---

## 1. Purpose

The purpose of this document is to evaluate and select an appropriate
technology stack for the CivicConnect application during Milestone 2.

The technology stack must support the architecture selected for CivicConnect,
the functional and non-functional requirements established during Milestone 1,
the data and persistence requirements of the system, and the design decisions
that will be implemented during Milestone 2.

The technology selection must also consider:

- compatibility with the selected architecture;
- team development capability;
- maintainability;
- testability;
- security;
- performance;
- cost and licensing;
- dependency and ecosystem support;
- deployment compatibility;
- future extensibility.

The selected technologies will be used to establish the initial CivicConnect
application structure and provide meaningful implementation evidence during
Milestone 2.

---

# 2. Architectural Context

The Milestone 2 architecture work defines CivicConnect as a Modular Monolith
using Clean/Hexagonal-style boundaries.

The application is separated into the following major areas:

1. Presentation
2. Application
3. Domain
4. Infrastructure

Conceptually:

Presentation
     |
Application
     |
Domain

Infrastructure provides implementations for external technical concerns such
as persistence and external services while depending on abstractions defined
by the inner application boundaries.

The technology stack must therefore support clear separation between these
areas.

The Domain layer should remain independent of database, user-interface and
external-provider technologies.

---

# 3. Relevant Requirements and Constraints

The technology stack must support the requirements established during
Milestone 1.

## 3.1 Performance

Non-Functional Requirement(NFR)-001 requires at least 95% of normal user actions to complete within
3 seconds under the workload agreed by the team.

The selected backend, persistence technology and database must therefore
support efficient request processing and data access.

Technology selection alone does not guarantee compliance with NFR-001.
Performance must later be verified through testing.

## 3.2 Security

NFR-002 requires users to access only the functionality and request
information permitted for their role.

The technology stack must therefore support authentication, authorisation and
role-based access control.

## 3.3 Auditability

NFR-005 requires status changes to record the user, new status and time of the
change.

The selected persistence technology must support reliable storage of this
audit information.

## 3.4 Maintainability and Testability

The technology stack should allow the architecture to maintain separation
between Domain, Application, Infrastructure and Presentation concerns.

It should also support automated testing so that business rules and design
decisions can be verified independently where possible.

## 3.5 Cost

The project should avoid unnecessary paid infrastructure and licensing where
suitable free development and deployment options are available.

The final deployment platform must therefore be evaluated separately against
the project's cost constraints before production deployment is selected.

---

# 4. Backend / Runtime Evaluation

The backend is responsible for application use cases, request processing,
business-rule coordination, security integration and communication with
infrastructure services.

The following alternatives were considered:

| Technology | Advantages | Limitations |
|---|---|---|
| ASP.NET Core / C# | Strong support for layered applications, dependency injection, REST APIs, asynchronous programming and automated testing. Integrates naturally with .NET libraries and EF Core. | Requires the team to maintain knowledge of the .NET ecosystem and appropriate project structure. |
| Node.js / JavaScript | Familiar JavaScript ecosystem, strong support for web APIs and asynchronous I/O. | Would introduce a different backend direction from the .NET-oriented architecture and persistence work already being developed for CivicConnect. |
| Java / Spring Boot | Mature enterprise ecosystem with strong dependency injection, persistence and testing support. | Introduces another ecosystem and additional learning/implementation overhead for the current project. |

## 4.1 Proposed Selection

**ASP.NET Core with C#**

## 4.2 Rationale

ASP.NET Core is proposed because it can directly support the Modular Monolith
and Clean/Hexagonal-style boundaries selected for CivicConnect.

The solution can be separated into projects such as:

- CivicConnect.Domain
- CivicConnect.Application
- CivicConnect.Infrastructure
- CivicConnect.WebApi

This allows the Domain project to remain independent of infrastructure
implementations while the Application layer coordinates use cases and the
Infrastructure layer implements persistence and external-service concerns.

ASP.NET Core also provides built-in dependency injection, which supports
dependency inversion between application abstractions and infrastructure
implementations.

C# also provides strong support for asynchronous operations using Task and
async/await, which may be useful for database and external-provider
operations.

## 4.3 Trade-Off

The team must maintain clear project boundaries. Simply using ASP.NET Core
does not automatically produce Clean Architecture. Incorrect dependencies
between projects could still create tight coupling.

---

# 5. API / Presentation Technology

For the initial Milestone 2 implementation, CivicConnect requires a mechanism
through which application functionality can be demonstrated.

## Proposed Selection

**ASP.NET Core Web API**

## Rationale

A Web API allows the team to expose meaningful CivicConnect functionality
without requiring the complete final user interface during Milestone 2.

An initial API can demonstrate functionality such as:

- creating a service request;
- retrieving a service request;
- updating a service-request status;
- exercising application and domain rules;
- persisting data;
- triggering domain/application events.

This provides meaningful development evidence while keeping the initial M2
implementation manageable.

## Trade-Off

A Web API does not represent the complete final CivicConnect user experience.
Additional presentation/UI work may therefore still be required in later
development.

---

# 6. Persistence / ORM Evaluation

CivicConnect requires persistent storage for service requests and related
information.

The persistence mechanism must support the Infrastructure boundary defined by
the architecture while preventing database-specific concerns from being
embedded directly in the Domain layer.

The following approaches were considered:

| Approach | Advantages | Limitations |
|---|---|---|
| Entity Framework Core | Integrates with .NET, supports entity mapping, migrations, transactions, LINQ and dependency injection. | Incorrect use can couple application/domain models too closely to persistence concerns. ORM-generated queries must still be monitored. |
| Dapper | Lightweight and gives developers greater control over SQL. | Requires more manual SQL and mapping code and places more persistence responsibility on the development team. |
| Direct ADO.NET | Provides low-level database control with minimal abstraction. | Requires considerably more boilerplate connection, command and mapping code for the current project. |

## 6.1 Proposed Selection

**Entity Framework Core**

## 6.2 Rationale

Entity Framework Core is proposed because it integrates directly with the
proposed ASP.NET Core backend and supports the initial persistence needs of
CivicConnect.

EF Core provides:

- object-relational mapping;
- database migrations;
- LINQ-based queries;
- transaction support;
- asynchronous database operations;
- dependency-injection integration;
- configurable entity mappings.

Persistence-specific configuration can remain within
CivicConnect.Infrastructure while the core Domain model remains focused on
business behaviour.

## 6.3 Trade-Off

EF Core introduces an ORM abstraction and does not remove the need to
understand database behaviour.

Queries, transactions and entity tracking must still be designed carefully,
particularly where concurrency or performance is important.

---

# 7. Database Evaluation

CivicConnect contains structured and related information such as users,
service requests, status information and audit/history information.

A relational database is therefore proposed for the initial implementation.

Possible relational database technologies include:

- SQL Server;
- PostgreSQL.

Both are compatible with EF Core.

## Current Decision Status

**Database technology: Pending team confirmation**

A final database should not be recorded as selected until the team confirms
the database and evaluates it against:

- EF Core compatibility;
- development environment;
- deployment options;
- cost;
- team familiarity;
- backup/recovery needs;
- security;
- expected project scale.

Until that decision is approved, the architecture should depend on
persistence abstractions rather than database-specific behaviour.

---

# 8. Testing Technology

Automated testing is required to provide verification evidence for the
application and its engineering decisions.

The following .NET testing frameworks were considered:

| Framework | Advantages | Limitations |
|---|---|---|
| xUnit | Widely used in .NET projects, lightweight and suitable for unit/integration testing. | Requires the team to structure tests and test data appropriately. |
| NUnit | Mature .NET testing framework with extensive features. | Introduces an alternative testing convention when the project already has familiarity with xUnit. |
| MSTest | Integrated with the Microsoft ecosystem. | The team has stronger existing experience with xUnit. |

## 8.1 Proposed Selection

**xUnit**

## 8.2 Rationale

xUnit is proposed as the initial automated testing framework because it is
compatible with .NET and supports the types of automated verification required
for CivicConnect.

Initial tests can verify:

- domain business rules;
- valid and invalid status transitions once the lifecycle rules are approved;
- domain-event generation;
- notification handlers;
- location-provider abstractions;
- application services;
- persistence behaviour through integration tests where appropriate.

xUnit also aligns with existing team experience.

---

# 9. Dependency Management

## Proposed Selection

**NuGet**

## Rationale

NuGet is the standard package-management mechanism for .NET applications and
will be used to manage external dependencies required by the CivicConnect
solution.

Dependencies should be added only where they provide a justified engineering
benefit.

Important package versions must be recorded and kept consistent across the
solution.

---

# 10. Source Control and Project Management

## Selected Technologies

**Source Control:** Git and GitHub

**Project Management:** GitHub Projects

The repository is already managed using GitHub and Milestone 2 work is tracked
through GitHub issues/project items.

Development work should follow a traceable workflow:

Issue
  |
Branch
  |
Commits
  |
Pull Request
  |
Review
  |
Merge

This provides repository evidence linking engineering work to implementation.

---

# 11. Proposed Solution Structure

If the proposed .NET technology stack is approved, the initial solution should
follow the architecture established during M2.

Proposed structure:

CivicConnect/
|
+-- src/
|   |
|   +-- CivicConnect.Domain/
|   |
|   +-- CivicConnect.Application/
|   |
|   +-- CivicConnect.Infrastructure/
|   |
|   +-- CivicConnect.WebApi/
|
+-- tests/
|   |
|   +-- CivicConnect.Domain.Tests/
|   |
|   +-- CivicConnect.Application.Tests/
|   |
|   +-- CivicConnect.Infrastructure.Tests/
|
+-- docs/

## CivicConnect.Domain

Contains core business concepts and business rules.

It must not directly depend on EF Core, the Web API, or external service
providers.

Potential content includes:

- domain entities;
- value objects;
- domain events;
- business rules;
- lifecycle rules once formally approved.

## CivicConnect.Application

Contains application use cases and abstractions required by those use cases.

Potential content includes:

- commands/use cases;
- queries;
- application services;
- repository abstractions;
- external-service abstractions;
- event handlers.

## CivicConnect.Infrastructure

Contains technical implementations.

Potential content includes:

- EF Core DbContext;
- entity configurations;
- repository implementations;
- migrations;
- external location-provider adapters;
- notification-provider implementations.

## CivicConnect.WebApi

Provides the initial presentation/API boundary.

Potential content includes:

- API endpoints/controllers;
- authentication/authorisation configuration;
- dependency-injection composition;
- application configuration.

---

# 12. Compatibility with Member 1 Architecture

The proposed stack supports the selected Modular Monolith architecture.

The intended dependency direction is:

WebApi
   |
Application
   |
Domain

Infrastructure
   |
   +---- implements abstractions required by the application
   |
   +---- interacts with the database and external providers

The Domain layer remains independent from infrastructure technologies.

This allows the team to change infrastructure implementations without
requiring the core business model to directly depend on those technologies.

---

# 13. Compatibility with Member 3 Design Work

The technology stack must also support the M2 design decisions.

## 13.1 Notification Architecture

ASP.NET Core dependency injection and C# interfaces can support a decoupled
event/handler approach.

For example:

ServiceRequest
      |
StatusChangedEvent
      |
Application Handler
      |
Notification Abstraction
      |
Infrastructure Provider

The final notification implementation remains subject to the approved
Member 3 design decision.

## 13.2 External Location Provider

C# interfaces and dependency injection can support an external-provider
boundary such as:

Application
     |
ILocationResolver
     |
Infrastructure Adapter
     |
External Location Provider

This allows application logic to depend on an abstraction rather than directly
on an external API.

The final approach remains subject to the approved M2 design decision.

---

# 14. Security Considerations

The proposed stack must support the NFR-002 access-control requirement.

ASP.NET Core provides mechanisms for authentication and role/policy-based
authorisation.

However, the selection of ASP.NET Core does not by itself satisfy NFR-002.

The application must still implement and test the project's approved access
rules.

Sensitive configuration such as database credentials, API keys and external
provider secrets must not be committed directly to the Git repository.

Development and deployment environments should use appropriate configuration
or secret-management mechanisms.

---

# 15. Performance Considerations

ASP.NET Core and EF Core support asynchronous request and database operations.

This can help avoid unnecessary blocking during database and external-service
operations.

However, the technology stack does not guarantee compliance with NFR-001.

Performance must later be verified against the agreed workload to determine
whether at least 95% of normal user actions complete within 3 seconds.

Database queries, external-provider calls and notification processing must be
considered during performance testing.

---

# 16. Maintainability and Testability

The proposed technologies support separation of concerns through:

- separate .NET projects;
- interfaces;
- dependency injection;
- repository/persistence abstractions;
- domain events;
- test projects;
- mock/fake infrastructure implementations.

This supports independent testing of domain and application behaviour without
requiring every test to communicate with a real database or external provider.

---

# 17. Cost and Licensing

The proposed stack uses technologies that can be used without requiring the
team to purchase development licences for the initial project implementation.

However, infrastructure and deployment costs depend on the database,
hosting platform and external services eventually selected.

The final deployment decision must therefore be evaluated separately against
the project's cost constraints.

No production hosting cost should be claimed until a deployment platform and
usage assumptions have been selected and verified.

---

# 18. Risks and Trade-Offs

## 18.1 ORM Complexity

EF Core simplifies persistence implementation but can hide expensive database
operations if queries are poorly designed.

Mitigation:

- review generated queries where necessary;
- use appropriate projections and indexing;
- use integration tests for important persistence behaviour.

## 18.2 Architectural Boundary Violations

Using separate projects does not automatically guarantee clean boundaries.

Mitigation:

- Domain must not reference Infrastructure;
- infrastructure-specific code should remain in Infrastructure;
- application use cases should depend on abstractions where appropriate.

## 18.3 External Provider Latency

Location and notification providers may introduce latency or failures.

Mitigation:

- use provider abstractions;
- avoid embedding provider-specific logic in the Domain layer;
- define appropriate failure-handling behaviour;
- evaluate asynchronous processing where justified.

## 18.4 Dependency Risk

Third-party packages can introduce compatibility, maintenance and security
risks.

Mitigation:

- minimise unnecessary dependencies;
- record important package versions;
- review dependency updates;
- avoid abandoned packages.

## 18.5 Database Decision Still Outstanding

The relational database product has not yet been formally confirmed.

Mitigation:

- keep persistence boundaries database-independent where practical;
- complete the persistence/database evaluation before implementation is
  baselined.

---

# 19. Technology Decision Summary

The following technology stack is proposed for the initial CivicConnect
Milestone 2 implementation:

| Area | Technology | Status |
|---|---|---|
| Architecture | Modular Monolith + Clean/Hexagonal boundaries | Selected |
| Language | C# | Selected |
| Backend | ASP.NET Core | Selected |
| API | ASP.NET Core Web API | Selected |
| Persistence ORM | Entity Framework Core | Selected |
| Database | Microsoft SQL Server | Selected |
| Automated Testing | xUnit | Selected |
| Dependency Management | NuGet | Selected |
| Source Control | Git | Existing |
| Repository | GitHub | Existing |
| Project Management | GitHub Projects | Existing |

---

# 20. Recommended M2 Technology Decision

Based on the selected CivicConnect architecture, existing project direction,
team capability and Milestone 2 implementation needs, the proposed application
technology stack is:

**C# + ASP.NET Core Web API + Entity Framework Core + xUnit**

with Git/GitHub used for version control and GitHub Projects used for project
management.

A relational database will be used for persistence, but the specific database
product must be confirmed before the persistence decision is marked Accepted.

This stack supports the selected Modular Monolith architecture and allows
CivicConnect to separate Domain, Application, Infrastructure and Presentation
responsibilities.

The stack also provides the mechanisms required to begin meaningful M2
development, including dependency injection, API development, relational
persistence, asynchronous operations and automated testing.

---

# 21. Decision Status

**Status: PROPOSED**

Before this document is changed to **ACCEPTED**, the following must be
completed:

- [ ] Team confirms C# / ASP.NET Core.
- [ ] Team confirms Entity Framework Core.
- [ ] Database technology is selected.
- [ ] Significant framework/package versions are recorded.
- [ ] Compatibility with the final data model is confirmed.
- [ ] ADR/decision record is created.
- [ ] Initial application implementation begins.
- [ ] Initial automated tests are added.
