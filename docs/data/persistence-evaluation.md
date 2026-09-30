# CivicConnect Persistence and Database Evaluation

**Milestone:** Milestone 2  
**Document Type:** Persistence and Database Evaluation  
**Project:** CivicConnect  
**Status:** Proposed  
**Owner:** Member 2  
**Related Documents:**  
- `docs/data/data-model.md`
- `docs/data/initial-erd.md`
- `docs/technology/technology-stack-evaluation.md`

---

# 1. Purpose

The purpose of this document is to evaluate and select an appropriate
persistence approach for CivicConnect.

The initial M2 data model and ERD identified structured and related information
including:

- users;
- roles;
- service requests;
- request statuses;
- status history;
- departments;
- assignments;
- notifications.

CivicConnect therefore requires a persistence solution capable of maintaining
relationships, data integrity and transactional consistency.

This evaluation considers:

- database type;
- database technology;
- ORM/persistence mechanism;
- transactions;
- concurrency;
- data integrity;
- migrations;
- performance;
- security;
- maintainability;
- backup and recovery;
- compatibility with the selected M2 architecture;
- compatibility with the proposed technology stack.

---

# 2. Inputs to the Decision

This persistence decision is not made independently.

It is based on the following M2 engineering evidence:

1. M1 requirements and constraints;
2. Member 1's Modular Monolith architecture;
3. Clean/Hexagonal-style architectural boundaries;
4. the initial M2 data model;
5. the initial CivicConnect ERD;
6. Assignment 2 persistence/concurrency research;
7. the proposed ASP.NET Core technology stack.

The persistence solution must fit these existing decisions rather than forcing
the architecture to depend directly on a database technology.

---

# 3. Requirements Affecting Persistence

Several CivicConnect requirements influence the persistence design.

## 3.1 Performance — NFR-001

NFR-001 requires at least 95% of normal user actions to complete within
3 seconds under the workload agreed by the team.

Persistence can directly affect response time because many normal application
operations will require database access.

The persistence implementation must therefore support:

- efficient queries;
- asynchronous database access where appropriate;
- suitable indexing;
- controlled loading of related data;
- appropriate transaction boundaries.

The database technology alone does not guarantee NFR-001 compliance.

Performance must later be measured using representative tests.

---

# 4. Security — NFR-002

NFR-002 requires users to access only functionality and information permitted
for their role.

The database will store information used by the application to support this
behaviour, including user and role information.

However, authorisation must primarily be enforced through the application.

The database should not be treated as the only security mechanism.

---

# 5. Auditability — NFR-005

NFR-005 requires status changes to record:

- the user;
- the new status;
- the time of the change.

The proposed `StatusHistory` entity supports this requirement.

A status update may therefore involve at least two persistence operations:

1. update the current status of the ServiceRequest;
2. create the corresponding StatusHistory record.

These related operations must remain consistent.

---

# 6. Persistence Requirements

Based on the current CivicConnect model, the persistence solution should
support:

- relational data;
- primary and foreign keys;
- referential integrity;
- transactions;
- schema migrations;
- asynchronous operations;
- querying;
- indexing;
- concurrency handling;
- automated integration testing;
- backup and recovery mechanisms at deployment level.

---

# 7. Database Model Evaluation

Three broad persistence approaches were considered.

| Approach | Advantages | Limitations |
|---|---|---|
| Relational Database | Strong relationships, constraints, transactions, referential integrity and structured querying | Schema changes require controlled migration |
| Document Database | Flexible document structures and reduced need for relational joins in document-oriented workloads | CivicConnect contains several strongly related entities and audit relationships |
| File-Based Storage | Simple for prototypes and small demonstrations | Poor fit for concurrent multi-user transactional application data |

## Decision

**Relational database**

## Rationale

CivicConnect contains strongly related information.

For example:

```text
User
  |
  v
ServiceRequest
  |
  +----> StatusHistory
  |
  +----> Assignment
  |
  +----> Notification
```

These relationships benefit from:

- foreign keys;
- referential integrity;
- transactional operations;
- structured querying.

A relational database therefore provides the strongest fit for the current
CivicConnect data model.

---

# 8. Relational Database Alternatives

The following database technologies were considered for the initial
implementation.

## Option A — Microsoft SQL Server

### Advantages

- mature relational database platform;
- strong transaction support;
- strong integration with .NET;
- official EF Core provider;
- supports constraints, indexes and concurrency mechanisms;
- widely used with ASP.NET Core applications;
- suitable tooling is available for development.

### Limitations

- deployment cost/licensing depends on the selected edition and hosting
  environment;
- deployment options must be checked against CivicConnect's cost constraints;
- using SQL Server-specific behaviour could reduce database portability if
  infrastructure code becomes tightly coupled to it.

---

## Option B — PostgreSQL

### Advantages

- mature relational database;
- open-source;
- strong ACID transaction support;
- strong constraints and indexing capabilities;
- EF Core support through the Npgsql provider;
- suitable for production web applications;
- avoids proprietary database licensing costs.

### Limitations

- requires an additional EF Core provider;
- introduces PostgreSQL-specific operational knowledge;
- team familiarity must be considered.

---

## Option C — SQLite

### Advantages

- lightweight;
- easy local setup;
- no separate database server required;
- useful for prototypes and certain automated tests.

### Limitations

- not the preferred option for demonstrating the expected behaviour of a
  concurrent multi-user production-style CivicConnect database;
- some database behaviours differ from server-based relational databases;
- using SQLite for testing does not necessarily reproduce the behaviour of
  the eventual production database.

---

# 9. Database Comparison

| Criterion | SQL Server | PostgreSQL | SQLite |
|---|---|---|---|
| Relational model | Strong | Strong | Yes |
| Transactions | Strong | Strong | Supported |
| EF Core integration | Excellent | Excellent through provider | Excellent |
| Referential integrity | Strong | Strong | Supported |
| Multi-user application suitability | Strong | Strong | More limited |
| Development setup | Moderate | Moderate | Very easy |
| Production scalability | Strong | Strong | Limited for this use case |
| Licensing/cost flexibility | Depends on environment | Strong | Strong |
| Team/.NET ecosystem alignment | Strong | Strong | Strong |
| Suitable as main CivicConnect database | Yes | Yes | Not preferred |

---

# 10. Proposed Database Selection

## Proposed Decision

**Microsoft SQL Server**

## Rationale

SQL Server is proposed as the initial CivicConnect relational database because
it integrates directly with the proposed .NET and Entity Framework Core
technology stack.

It provides the relational and transactional features required by the current
data model, including:

- primary keys;
- foreign keys;
- constraints;
- transactions;
- indexes;
- concurrency mechanisms;
- reliable relational querying.

It also provides a straightforward development path for the team when using
ASP.NET Core and EF Core.

The selection does not mean that CivicConnect Domain or Application code
should directly depend on SQL Server.

SQL Server remains an Infrastructure concern.

## Important Constraint

The final deployment environment and database edition must still be evaluated
against the project's cost constraints.

Therefore:

**SQL Server is proposed for M2 implementation, while the production hosting
and database deployment decision remains separate.**

---

# 11. ORM / Data Access Evaluation

Three data-access approaches were considered.

## Option A — Entity Framework Core

EF Core is Microsoft's object-relational mapper for .NET.

### Advantages

- integrates with ASP.NET Core;
- supports LINQ;
- supports migrations;
- supports transactions;
- supports asynchronous database operations;
- supports entity configuration;
- supports dependency injection;
- reduces repetitive mapping/data-access code.

### Limitations

- poor query design can still cause performance problems;
- developers must understand entity tracking and loading behaviour;
- ORM abstractions can hide expensive SQL operations;
- careless use can couple domain models to persistence concerns.

---

## Option B — Dapper

Dapper is a lightweight .NET data-access library.

### Advantages

- gives developers direct control over SQL;
- lightweight;
- useful where precise SQL control is required.

### Limitations

- requires more manually written SQL;
- requires more manual object mapping;
- schema evolution requires additional tooling/management;
- creates more persistence implementation work for the team.

---

## Option C — ADO.NET

ADO.NET provides lower-level database access.

### Advantages

- maximum control over database interaction;
- minimal ORM abstraction.

### Limitations

- significantly more boilerplate;
- manual command/query management;
- manual mapping;
- increased implementation effort;
- unnecessary complexity for the current M2 scope.

---

# 12. ORM Decision

## Proposed Decision

**Entity Framework Core**

## Rationale

EF Core provides the best fit for the proposed ASP.NET Core stack and the
current M2 implementation scope.

It allows the team to implement the initial relational model while retaining
separation between the Domain and Infrastructure layers.

The intended structure is:

```text
Domain
   |
   | Business entities/rules
   v

Application
   |
   | Persistence abstractions
   v

Infrastructure
   |
   | EF Core
   v

SQL Server
```

Database-specific configuration should remain within Infrastructure.

---

# 13. Proposed Persistence Architecture

The persistence design should follow Member 1's architectural boundaries.

```text
CivicConnect.Domain
        |
        | ServiceRequest
        | StatusHistory
        | User
        | etc.
        |
        v
CivicConnect.Application
        |
        | Repository / persistence abstractions
        |
        v
CivicConnect.Infrastructure
        |
        | EF Core DbContext
        | Entity configurations
        | Repository implementations
        |
        v
SQL Server
```

The Domain project must not directly reference:

- SQL Server;
- EF Core database providers;
- database connection strings.

---

# 14. DbContext

If EF Core is accepted, Infrastructure will contain a CivicConnect
`DbContext`.

Conceptually:

```text
CivicConnectDbContext
 |
 +-- Users
 +-- Roles
 +-- ServiceRequests
 +-- RequestStatuses
 +-- StatusHistory
 +-- Departments
 +-- Assignments
 +-- Notifications
```

The DbContext acts as the EF Core persistence boundary for the initial
relational model.

The actual implementation becomes application evidence once the M2 coding
phase begins.

---

# 15. Entity Configuration

Database mappings should be configured within Infrastructure.

Proposed structure:

```text
CivicConnect.Infrastructure/
|
+-- Persistence/
    |
    +-- CivicConnectDbContext.cs
    |
    +-- Configurations/
        |
        +-- UserConfiguration.cs
        +-- RoleConfiguration.cs
        +-- ServiceRequestConfiguration.cs
        +-- RequestStatusConfiguration.cs
        +-- StatusHistoryConfiguration.cs
        +-- DepartmentConfiguration.cs
        +-- AssignmentConfiguration.cs
        +-- NotificationConfiguration.cs
```

Configuration should define relevant:

- table relationships;
- primary keys;
- foreign keys;
- required properties;
- maximum lengths where justified;
- indexes where justified;
- delete behaviour.

---

# 16. Transaction Strategy

Transactions are important where one business operation changes multiple
persistent records.

A service-request status update is the main current example.

Conceptually:

```text
BEGIN TRANSACTION

        |
        +---- UPDATE ServiceRequest.Status
        |
        +---- INSERT StatusHistory
        |
        +---- Save changes

COMMIT
```

If one required operation fails:

```text
ROLLBACK
```

This prevents the request state and audit history from becoming inconsistent.

---

# 17. Transaction Boundary Decision

## Proposed Decision

A business operation that requires multiple related persistence changes to
remain consistent should execute within one transactional boundary.

For example:

```text
ChangeRequestStatus
        |
        +-- Validate transition
        |
        +-- Change current status
        |
        +-- Add status history
        |
        +-- Save
```

The transaction should represent the business operation rather than creating
unnecessarily broad transactions across unrelated operations.

---

# 18. External Notifications and Transactions

External notification delivery should not automatically be placed inside a
long-running database transaction.

For example, this should be avoided:

```text
BEGIN DATABASE TRANSACTION
        |
        +-- Update status
        |
        +-- Call external email service
        |
        +-- Wait for external provider
        |
        +-- Commit
```

External services can be slow or unavailable.

Holding a database transaction open while waiting for an external provider may
increase latency and lock duration.

The Member 3 notification design must determine how notification processing is
decoupled from the core transaction.

The M2 design currently being evaluated uses domain/application events as the
candidate boundary.

---

# 19. Concurrency Problem

CivicConnect may encounter concurrent modification.

Example:

```text
10:00
Staff A reads Request #101
Status = Submitted

10:01
Staff B reads Request #101
Status = Submitted

10:02
Staff A changes status

10:03
Staff B attempts a change based on stale data
```

Without concurrency control, Staff B may overwrite or conflict with the newer
state.

This could affect:

- lifecycle correctness;
- audit history;
- assignments;
- operational consistency.

---

# 20. Concurrency Alternatives

## Option A — Optimistic Concurrency

The application allows normal concurrent access but detects whether a record
has changed before accepting an update.

### Advantages

- suitable where conflicting writes are not expected constantly;
- avoids holding database locks while users are working;
- supported by EF Core.

### Limitation

The application must handle concurrency conflicts correctly.

---

## Option B — Pessimistic Locking

The database locks data to prevent conflicting modification.

### Advantages

- can provide strict control during sensitive operations.

### Limitations

- longer locks can reduce concurrency;
- can introduce blocking;
- can increase complexity;
- inappropriate use may negatively affect performance.

---

# 21. Proposed Concurrency Decision

## Proposed Decision

**Optimistic concurrency**

## Rationale

CivicConnect is expected to allow multiple users to interact with service
requests, but continuous simultaneous modification of the exact same request
should not be assumed as the normal case.

Optimistic concurrency therefore provides a suitable initial strategy.

The application should detect when a record has changed since it was read and
reject or safely resolve the conflicting update rather than silently
overwriting newer data.

The exact EF Core implementation will be defined during development.

---

# 22. Proposed Concurrency Token

If EF Core and SQL Server are accepted, a concurrency token may be introduced
for records where concurrent modification matters.

For example, `ServiceRequest` may contain a version value conceptually similar
to:

```text
ServiceRequest
|
+-- ServiceRequestId
+-- StatusId
+-- ...
+-- RowVersion
```

When an update occurs:

```text
Original RowVersion
        |
        v
Database compares version
        |
        +---- same ----> UPDATE
        |
        +---- changed -> CONCURRENCY CONFLICT
```

The exact implementation should be added only when the persistence code is
created.

---

# 23. Schema Migration Strategy

The database schema will evolve during development.

If EF Core is accepted, **EF Core Migrations** are proposed for controlled
schema evolution.

The workflow should be:

```text
Change entity/configuration
        |
        v
Create migration
        |
        v
Review migration
        |
        v
Commit migration
        |
        v
Apply migration
```

Migration files should be version-controlled.

Developers should not make undocumented manual production schema changes that
cannot be reproduced from the repository.

---

# 24. Data Integrity Strategy

The database should enforce structural integrity where appropriate.

Examples include:

### Primary Keys

Each persisted entity must have a unique identifier.

### Foreign Keys

Relationships must reference valid parent records.

### Required Values

Fields required by the approved data model should not accept invalid null
values.

### Unique Constraints

Values that are required to be unique should be protected where justified by
the requirements.

### Domain Rules

Not every business rule belongs in the database.

For example:

```text
Submitted → Closed
```

being invalid is primarily a lifecycle/domain rule.

The Domain/Application layer should determine whether the transition is
allowed.

The database should preserve valid persisted state rather than replacing the
Domain layer's business logic.

---

# 25. Delete Behaviour

Cascade deletion should not be enabled without considering audit and history
requirements.

For example, deleting a ServiceRequest and automatically deleting all
StatusHistory records could remove important audit evidence.

Therefore, deletion behaviour must be explicitly configured.

For important historical/audit entities, restrictive deletion or another
approved retention approach should be considered instead of uncontrolled
cascade deletion.

---

# 26. Indexing Strategy

Indexes should be introduced based on expected query patterns.

Initial candidates for evaluation include:

```text
ServiceRequest.RequesterId

ServiceRequest.StatusId

ServiceRequest.DepartmentId

ServiceRequest.CreatedAt

StatusHistory.ServiceRequestId

Assignment.ServiceRequestId

Notification.UserId

Notification.ServiceRequestId
```

Indexes can improve reads but also introduce storage and write overhead.

Therefore, indexes should be justified rather than added to every field.

---

# 27. Security

Database security must be considered together with application security.

The persistence implementation should follow these principles:

- connection strings must not be hard-coded in source code;
- passwords must not be committed to GitHub;
- API keys must not be stored in the database unless there is an approved
  secure mechanism;
- production credentials must be managed through environment/deployment
  configuration;
- application queries should use the ORM/query parameterisation rather than
  constructing unsafe SQL;
- database access should follow least-privilege principles.

---

# 28. Sensitive User Data

CivicConnect should store only user information necessary for approved system
functionality.

The current initial model proposes fields such as:

```text
FirstName
LastName
Email
```

Additional personal information should not be added without a project
requirement.

Authentication credentials, if CivicConnect manages them directly, require a
separate approved security design and must never be stored as plain-text
passwords.

---

# 29. Performance

The persistence layer affects NFR-001.

Potential database performance risks include:

- unnecessary queries;
- loading excessive related data;
- missing indexes;
- large result sets;
- long-running transactions;
- external calls inside transactions;
- inefficient ORM queries.

Mitigation includes:

- asynchronous database operations where appropriate;
- query projections;
- pagination where required;
- justified indexing;
- short transaction boundaries;
- performance testing.

NFR-001 must be measured rather than assumed to be satisfied because SQL
Server and EF Core were selected.

---

# 30. Backup and Recovery

The final production database solution must provide an appropriate backup and
recovery strategy.

The exact mechanism depends on the eventual deployment environment.

The production decision should consider:

- backup frequency;
- restoration;
- database failure;
- accidental data loss;
- migration rollback/recovery;
- retention requirements.

A specific production backup schedule is not defined in this M2 document
because the deployment platform has not yet been selected.

---

# 31. Testing Strategy

Persistence behaviour should be tested using appropriate integration tests.

Important tests include:

1. creating a ServiceRequest;
2. retrieving a ServiceRequest;
3. updating a ServiceRequest;
4. storing StatusHistory;
5. verifying important relationships;
6. testing transaction rollback where practical;
7. testing concurrency behaviour once implemented.

Domain logic should still be unit tested independently from the real database
where possible.

---

# 32. Architecture Compatibility

The selected persistence approach supports the Modular Monolith architecture.

```text
Presentation
      |
      v
Application
      |
      v
Domain

      ^

Infrastructure
      |
      v
EF Core
      |
      v
SQL Server
```

Infrastructure depends on the inner application/domain concepts where
necessary.

The Domain does not depend on SQL Server.

This preserves the architectural boundary established by Member 1.

---

# 33. Technology Compatibility

The proposed persistence stack is:

```text
ASP.NET Core
      |
      v
Entity Framework Core
      |
      v
SQL Server
```

These technologies are compatible within the .NET ecosystem.

EF Core provides the mapping between the application's data model and the
relational database.

---

# 34. Relationship to Member 3 Notification Design

The persistence decision must not tightly couple notification delivery to the
database transaction.

The intended separation is:

```text
Change Service Request
        |
        v
Validate Domain Rules
        |
        v
Persist State + Audit History
        |
        v
Domain/Application Event
        |
        v
Notification Handling
```

The final notification behaviour will be determined by the Member 3 design
decision.

---

# 35. Relationship to External Location Provider Design

External location-provider calls should also remain outside the core Domain
and database implementation.

The intended boundary is:

```text
Application
      |
      v
ILocationResolver
      ^
      |
Infrastructure Adapter
      |
      v
External Location Service
```

Location-provider implementation details should therefore not become database
or Domain dependencies.

---

# 36. Proposed Persistence Stack

The proposed persistence stack for CivicConnect M2 is:

| Area | Proposed Selection |
|---|---|
| Data Model | Relational |
| Database | Microsoft SQL Server |
| ORM | Entity Framework Core |
| Schema Management | EF Core Migrations |
| Transaction Approach | Business-operation transaction boundaries |
| Concurrency | Optimistic concurrency |
| Concurrency Token | To be implemented where required |
| Data Access Location | Infrastructure Layer |
| Testing | Integration tests + domain/application unit tests |

---

# 37. Decision Rationale

The proposed approach was selected because it:

1. supports CivicConnect's relational data model;
2. provides referential integrity;
3. supports transactional consistency;
4. aligns with the proposed ASP.NET Core stack;
5. supports Member 1's architecture;
6. allows database-specific implementation to remain in Infrastructure;
7. supports migrations;
8. supports asynchronous database operations;
9. supports concurrency control;
10. supports meaningful automated persistence testing.

---

# 38. Trade-Offs

The selected approach introduces several trade-offs.

### EF Core abstraction

EF Core reduces repetitive persistence code but may hide inefficient queries
if developers do not understand the generated database behaviour.

### SQL Server dependency

Using SQL Server creates an infrastructure dependency on a specific database
provider.

This is mitigated by preventing SQL Server-specific behaviour from spreading
into Domain/Application code.

### Optimistic concurrency

Optimistic concurrency does not prevent conflicts from occurring.

Instead, it detects them and requires the application to respond correctly.

### Relational schema

The relational schema requires controlled migrations when the data model
changes.

This is acceptable because the CivicConnect data model contains structured
relationships that benefit from relational integrity.

---

# 39. Risks and Mitigations

| Risk | Mitigation |
|---|---|
| Inefficient EF Core queries | Review queries and use performance testing |
| Long database transactions | Keep transaction boundaries focused |
| Concurrent request updates | Use optimistic concurrency |
| Lost audit information | Persist StatusHistory with lifecycle changes |
| Schema inconsistency | Use version-controlled EF Core migrations |
| Hard-coded credentials | Use secure configuration/environment variables |
| Excessive database coupling | Keep persistence implementation in Infrastructure |
| External service delays | Keep provider calls outside core DB transactions |
| Database deployment cost | Evaluate hosting/edition before production selection |

---

# 40. Proposed Decision

CivicConnect will use a **relational persistence model**.

The proposed M2 implementation uses:

**Microsoft SQL Server**

with:

**Entity Framework Core**

as the object-relational mapping and persistence technology.

EF Core Migrations will manage schema evolution.

Business operations that require multiple related database changes to remain
consistent will use appropriate transactional boundaries.

Optimistic concurrency will be used as the initial concurrency strategy for
data where conflicting updates could affect correctness.

Database-specific implementation will remain within the Infrastructure layer
to preserve the Modular Monolith / Clean Architecture boundaries established
during M2.

---

# 41. Decision Status

**Status: PROPOSED**

Before this decision becomes ACCEPTED:

- [ ] SQL Server selection is reviewed.
- [ ] EF Core selection is reviewed.
- [ ] Initial ERD is reviewed.
- [ ] Transaction approach is reviewed.
- [ ] Optimistic concurrency approach is reviewed.
- [ ] Significant technology versions are recorded.
- [ ] ADR/decision record is created.
- [ ] Initial persistence implementation is created.
- [ ] Persistence tests are added.
