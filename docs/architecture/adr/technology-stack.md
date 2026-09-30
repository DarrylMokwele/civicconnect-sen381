# ADR — CivicConnect Technology Stack

**Project:** CivicConnect  
**Milestone:** Milestone 2  
**Status:** Proposed  
**Owner:** Member 2  
**Decision Type:** Technology  
**Date:** 2026-09-30  

## 1. Context

CivicConnect requires a technology stack capable of implementing the
Modular Monolith with Clean/Hexagonal-style boundaries established during
Milestone 2.

The selected technologies must support the separation of:

- Presentation;
- Application;
- Domain;
- Infrastructure.

The technology stack must also support the functional and non-functional
requirements established during Milestone 1, including performance,
security, auditability, maintainability and automated verification.

The M1 engineering baseline deferred the final backend, persistence and
deployment technology decisions to Milestone 2.

The following M2 evidence was considered:

- Member 1's architecture baseline;
- M1 functional and non-functional requirements;
- `docs/technology/technology-stack-evaluation.md`;
- `docs/data/data-model.md`;
- `docs/data/initial-erd.md`;
- `docs/data/persistence-evaluation.md`.

---

## 2. Decision Drivers

The technology stack should:

1. support the selected Modular Monolith architecture;
2. preserve Clean/Hexagonal-style boundaries;
3. support REST API development;
4. support dependency injection;
5. support relational persistence;
6. support automated testing;
7. support asynchronous operations where appropriate;
8. support maintainable separation between business logic and infrastructure;
9. fit the team's current development capability;
10. avoid unnecessary licensing/development costs.

NFR-001 also requires at least 95% of normal user actions to complete within
3 seconds under the workload agreed by the team.

Technology selection alone does not guarantee this requirement; performance
must later be verified.

---

## 3. Alternatives Considered

### Option A — ASP.NET Core / C#

Advantages:

- strong support for layered applications;
- built-in dependency injection;
- REST API support;
- asynchronous programming support;
- integration with EF Core;
- strong automated-testing ecosystem.

Disadvantages:

- developers must maintain correct architectural boundaries;
- framework choice alone does not guarantee performance or maintainability.

### Option B — Node.js / JavaScript

Advantages:

- mature web ecosystem;
- strong API support;
- asynchronous I/O model.

Disadvantages:

- would introduce a different backend direction from the .NET-oriented
  architecture and persistence direction already being developed;
- would require additional stack alignment work.

### Option C — Java / Spring Boot

Advantages:

- mature enterprise framework;
- dependency injection;
- strong persistence and testing ecosystem.

Disadvantages:

- introduces another ecosystem;
- increases implementation and learning overhead for the current project.

---

## 4. Decision

CivicConnect will use the following initial application technology stack:

| Area | Technology |
|---|---|
| Language | C# |
| Backend | ASP.NET Core |
| API | ASP.NET Core Web API |
| ORM | Entity Framework Core |
| Database | Microsoft SQL Server |
| Testing | xUnit |
| Dependency Management | NuGet |
| Source Control | Git |
| Repository | GitHub |
| Project Management | GitHub Projects |

The application will be organised around the architecture established during
M2.

Proposed solution structure:

CivicConnect.Domain  
CivicConnect.Application  
CivicConnect.Infrastructure  
CivicConnect.WebApi

Test projects will be maintained separately.

---

## 5. Rationale

ASP.NET Core and C# provide the capabilities required to implement the
selected CivicConnect architecture while supporting dependency injection,
web APIs, asynchronous operations and automated testing.

Entity Framework Core provides the proposed persistence mechanism and
integrates with the selected relational database.

SQL Server provides relational storage, transactions, constraints and
concurrency mechanisms required by the initial CivicConnect data model.

xUnit provides automated verification for Domain, Application and
Infrastructure behaviour.

The combination allows infrastructure concerns to remain separated from
core business behaviour.

---

## 6. Architectural Impact

The intended dependency direction is:

Presentation
     |
     v
Application
     |
     v
Domain

Infrastructure provides implementations required by the application and
interacts with external technologies such as SQL Server.

The Domain layer must not directly depend on:

- SQL Server;
- EF Core;
- external APIs;
- presentation technologies.

---

## 7. Positive Consequences

- Clear compatibility with Member 1's architecture.
- Strong .NET ecosystem integration.
- Dependency injection support.
- Relational persistence support.
- Automated testing support.
- EF Core migration support.
- Asynchronous API/database operations can be implemented where appropriate.
- Infrastructure concerns can remain isolated.

---

## 8. Negative Consequences / Trade-Offs

- The team must understand ASP.NET Core and EF Core.
- Incorrect dependency references could violate the intended architecture.
- Poor EF Core queries could cause performance problems.
- SQL Server introduces a database-specific Infrastructure dependency.
- Production hosting/database cost still depends on the eventual deployment
  environment.

---

## 9. Verification

This decision will be verified through application evidence including:

- successful solution build;
- correct project dependency structure;
- working ASP.NET Core API;
- EF Core persistence;
- database migration;
- automated xUnit tests;
- implementation of at least one meaningful CivicConnect functional path.

NFR-001 performance compliance must be measured rather than inferred from
technology selection.

---

## 10. Traceability

Related evidence:

- M1 requirements baseline
- M2 architecture baseline
- `docs/technology/technology-stack-evaluation.md`
- `docs/data/data-model.md`
- `docs/data/initial-erd.md`
- `docs/data/persistence-evaluation.md`
- M2 PED
- M2 RTM

Implementation and test paths will be added to the RTM when they exist.

---

## 11. Status

**PROPOSED**

The decision becomes **ACCEPTED** after team review and approval.

Implementation evidence does not yet exist and must not be claimed until the
corresponding application code is committed.
