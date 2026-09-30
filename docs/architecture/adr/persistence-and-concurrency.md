# ADR — Persistence and Concurrency Strategy

**Project:** CivicConnect  
**Milestone:** Milestone 2  
**Status:** Accepted  
**Owner:** Member 2  
**Decision Type:** Data / Persistence  
**Date:** 2026-09-30  

## 1. Context

CivicConnect requires persistent storage for structured and related
information including users, service requests, statuses, status history,
departments, assignments and notifications.

The initial M2 data model and ERD show relationships that require reliable
referential integrity.

Some business operations may also modify multiple related records.

For example, a service-request status change may require:

1. changing the current request status;
2. creating an audit/status-history record.

Concurrent updates may also occur when multiple authorised users interact
with the same request.

The persistence strategy must therefore address:

- relational storage;
- data integrity;
- transactions;
- concurrency;
- schema evolution;
- architecture boundaries;
- testability.

---

## 2. Decision Drivers

The persistence solution must support:

- the initial CivicConnect ERD;
- referential integrity;
- transaction support;
- NFR-005 auditability;
- NFR-001 performance objectives;
- concurrent application usage;
- schema migrations;
- automated verification;
- Member 1's Clean/Hexagonal-style boundaries.

---

## 3. Database Alternatives

### Option A — Microsoft SQL Server

Advantages:

- mature relational database;
- strong .NET integration;
- EF Core provider;
- transaction support;
- constraints and indexes;
- concurrency mechanisms.

Limitations:

- deployment/licensing cost depends on environment;
- creates a SQL Server-specific Infrastructure dependency.

### Option B — PostgreSQL

Advantages:

- mature open-source relational database;
- ACID transaction support;
- strong constraints and indexing;
- EF Core support.

Limitations:

- requires the PostgreSQL EF Core provider;
- requires PostgreSQL-specific operational knowledge.

### Option C — SQLite

Advantages:

- simple local setup;
- lightweight;
- useful for some development/testing scenarios.

Limitations:

- not preferred as the main database for CivicConnect's expected
  multi-user transactional behaviour;
- behaviour may differ from the eventual server-based database.

---

## 4. Data Access Alternatives

### Entity Framework Core

Provides:

- object-relational mapping;
- LINQ;
- migrations;
- transactions;
- asynchronous operations;
- entity configuration;
- dependency-injection integration.

### Dapper

Provides:

- lightweight database access;
- greater direct SQL control.

However, it requires more manual SQL and mapping.

### ADO.NET

Provides low-level database control but introduces significantly more
boilerplate and manual persistence work.

---

## 5. Decision

CivicConnect will use:

**Relational Persistence**

with:

**Microsoft SQL Server**

and:

**Entity Framework Core**

for the initial M2 implementation.

Schema evolution will be managed through:

**EF Core Migrations**

Database-specific implementation will remain within:

**CivicConnect.Infrastructure**

---

## 6. Transaction Decision

Operations that require multiple related persistence changes to remain
consistent will execute within an appropriate business-operation
transactional boundary.

For example:

Change Request Status
        |
        v
Validate lifecycle rule
        |
        v
Update ServiceRequest
        |
        v
Create StatusHistory
        |
        v
Save transaction

If the required persistence operation fails, the related changes should not
leave the database in a partially updated state.

---

## 7. External Service Boundary

External notification or location-provider calls should not automatically be
included inside long-running database transactions.

For example, CivicConnect should avoid holding a database transaction open
while waiting for an external notification provider.

External-provider interaction will instead be handled through the appropriate
Application/Infrastructure boundaries defined by the M2 design decisions.

---

## 8. Concurrency Alternatives

### Option A — Optimistic Concurrency

Allows normal access but detects whether the persisted record changed before
an update is accepted.

Advantages:

- avoids unnecessarily holding long database locks;
- appropriate where conflicting writes are possible but not assumed to be
  constant;
- supported by EF Core.

Disadvantages:

- conflicts still occur;
- the application must detect and handle them correctly.

### Option B — Pessimistic Locking

Prevents conflicting modifications through database locking.

Advantages:

- provides strict control during locked operations.

Disadvantages:

- may increase blocking;
- may reduce concurrency;
- may increase complexity;
- longer locks can negatively affect performance.

---

## 9. Concurrency Decision

CivicConnect will use **optimistic concurrency** as the initial concurrency
strategy for entities where concurrent modification could affect correctness.

For relevant records, the persistence implementation may use a concurrency
token.

For example:

ServiceRequest
     |
     +-- ServiceRequestId
     +-- StatusId
     +-- ...
     +-- RowVersion

A stale update should be detected rather than silently overwriting a newer
state.

The exact conflict response will be implemented and tested during
development.

---

## 10. Data Integrity

The relational database will enforce structural integrity through appropriate:

- primary keys;
- foreign keys;
- required fields;
- constraints;
- indexes where justified.

Business rules remain the responsibility of the appropriate Domain/Application
logic.

For example, the database storing RequestStatus does not mean that every
status transition is valid.

Lifecycle transition validity must be enforced by the approved lifecycle
design.

---

## 11. Auditability

Status changes must support NFR-005.

The persistence model therefore includes StatusHistory capable of recording:

- affected service request;
- new status;
- user responsible for the change;
- time of the change.

Status changes and their required audit information should remain
transactionally consistent where applicable.

---

## 12. Schema Management

EF Core Migrations will be used to manage schema evolution.

The workflow is:

Entity/configuration change
        |
        v
Generate migration
        |
        v
Review migration
        |
        v
Commit migration
        |
        v
Apply migration

Migration files will be version-controlled.

---

## 13. Architecture Impact

The intended structure is:

Domain
   |
   v
Application
   |
   | persistence abstractions
   v
Infrastructure
   |
   | EF Core
   v
SQL Server

The Domain layer must not directly depend on EF Core or SQL Server.

---

## 14. Positive Consequences

- Supports the relational CivicConnect data model.
- Supports referential integrity.
- Supports transactional consistency.
- Supports audit-history persistence.
- Supports schema migrations.
- Supports concurrency detection.
- Integrates with the selected .NET technology direction.
- Keeps database implementation in Infrastructure.

---

## 15. Negative Consequences / Trade-Offs

- EF Core may generate inefficient queries if used poorly.
- SQL Server introduces database-specific infrastructure.
- Optimistic concurrency requires explicit conflict handling.
- Schema changes require controlled migrations.
- Transaction boundaries must be designed carefully.

---

## 16. Risks and Mitigations

| Risk | Mitigation |
|---|---|
| Lost updates | Optimistic concurrency |
| Inconsistent status/audit records | Transactional persistence |
| Inefficient queries | Query review and testing |
| Schema drift | EF Core Migrations |
| Database coupling | Keep implementation in Infrastructure |
| Long transactions | Use focused business-operation boundaries |
| External provider delays | Keep external calls outside DB transactions |
| Credential exposure | Secure configuration; do not commit secrets |

---

## 17. Verification

The persistence decision will be verified through:

- successful EF Core configuration;
- successful initial migration;
- creation and retrieval of ServiceRequests;
- persistence of StatusHistory;
- relationship tests;
- transaction tests where appropriate;
- optimistic-concurrency tests;
- integration tests against the selected database where practical.

Actual implementation/test references will be added to the RTM once they
exist.

---

## 18. Traceability

Related evidence:

- Initial Data Model
- Initial ERD
- Persistence and Database Evaluation
- Technology Stack Evaluation
- M2 Architecture
- M2 PED
- M2 RTM
- Assignment 2 concurrency/persistence research

---

## 19. Status

**PROPOSED**

This decision becomes **ACCEPTED** after team review and approval.

Implementation evidence must be added separately once the corresponding code,
migration and tests exist.
