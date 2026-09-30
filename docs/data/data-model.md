# CivicConnect Initial Data Model

**Milestone:** Milestone 2  
**Document Type:** Initial Data Model  
**Project:** CivicConnect  
**Status:** Proposed – Pending Team Review  
**Owner:** Member 2  
**Related Architecture:** Modular Monolith with Clean/Hexagonal-style boundaries  

---

# 1. Purpose

The purpose of this document is to define the initial data model for
CivicConnect during Milestone 2.

The data model identifies the main information that CivicConnect needs to
store in order to support the functional and non-functional requirements
defined during Milestone 1.

The model is intended to:

- identify the main CivicConnect entities;
- define important attributes;
- identify relationships between entities;
- identify primary and foreign keys;
- support service-request lifecycle management;
- support role-based access;
- support request assignment;
- support status history and auditability;
- support requester feedback;
- provide a foundation for the relational database schema;
- provide traceability between requirements and persistent data.

This is an initial M2 model and may evolve as implementation and requirements
are refined.

---

# 2. Data Modelling Approach

CivicConnect contains structured information with clear relationships between
users, service requests, assignments and request status information.

A relational data model is therefore proposed.

The initial model separates important responsibilities rather than storing all
information in a single ServiceRequest record.

The core proposed entities are:

1. User
2. Role
3. ServiceRequest
4. RequestStatus
5. StatusHistory
6. Department
7. Assignment
8. Notification

These entities are derived from the current CivicConnect requirements and M2
engineering needs.

Some implementation details remain proposed until the database and persistence
decisions are formally accepted.

---

# 3. Requirements Affecting the Data Model

The initial data model must support the requirements established during
Milestone 1.

## 3.1 Request Feedback — FR-005

FR-005 requires the requester to receive feedback when a request is:

- submitted;
- rejected;
- updated;
- completed.

The data model must therefore support identifying the requester associated
with a service request and provide sufficient request/lifecycle information
for feedback to be generated.

The Notification entity is proposed to support notification/feedback records.

---

## 3.2 Status Updates — FR-010

FR-010 requires authorised staff to update a request's status using allowed
status changes.

The data model must therefore store the current status of a service request.

It must also support the application's lifecycle-governance rules.

The database stores the resulting state, while the Domain/Application layers
remain responsible for enforcing approved lifecycle behaviour.

---

## 3.3 Invalid Status Changes — AC-010.2

AC-010.2 requires invalid status changes to be rejected.

This is primarily a domain/business-rule concern rather than only a database
concern.

The data model must nevertheless ensure that a ServiceRequest references a
valid RequestStatus.

---

## 3.4 Valid Status Persistence — AC-010.3

AC-010.3 requires a valid status change to be saved and displayed.

The ServiceRequest entity therefore stores its current status.

StatusHistory is also proposed to preserve evidence of status changes.

---

## 3.5 Resolve / Close Request — FR-012

FR-012 requires authorised staff to resolve or close eligible requests.

The data model must therefore support the lifecycle state of a service request
and preserve relevant status-change information.

---

## 3.6 Security — NFR-002

NFR-002 requires users to access only functions and request information
allowed for their role.

The data model therefore requires a mechanism for representing user roles.

Role information supports the application's authentication and authorisation
logic.

The database structure alone does not satisfy NFR-002; access rules must also
be implemented and tested in the application.

---

## 3.7 Auditability — NFR-005

NFR-005 requires each tested status change to record:

- the user;
- the new status;
- the time of the change.

StatusHistory is therefore an important entity in the initial data model.

It provides persistent evidence of request lifecycle changes.

---

# 4. Entity Overview

The initial CivicConnect data model contains the following entities:

| Entity | Purpose |
|---|---|
| User | Represents a CivicConnect user |
| Role | Represents the user's system role |
| ServiceRequest | Represents a civic service request |
| RequestStatus | Represents a valid request status |
| StatusHistory | Records changes to request status |
| Department | Represents an operational department/group |
| Assignment | Records assignment of a request |
| Notification | Records feedback/notification information |

---

# 5. User Entity

## 5.1 Purpose

The User entity represents a person who interacts with CivicConnect.

Depending on the approved access model, users may act as requesters,
operational staff or other authorised roles.

## 5.2 Proposed Attributes

| Attribute | Type | Description |
|---|---|---|
| UserId | Unique Identifier | Primary identifier |
| FirstName | String | User's first name |
| LastName | String | User's surname |
| Email | String | User's email address |
| RoleId | Foreign Key | References the user's role |
| CreatedAt | DateTime | Date/time user record was created |

## 5.3 Primary Key

`UserId`

## 5.4 Foreign Keys

`RoleId → Role.RoleId`

## 5.5 Relationships

A User:

- has a Role;
- may submit multiple ServiceRequests;
- may perform status changes recorded in StatusHistory;
- may receive notifications;
- may be involved in request assignments depending on the approved staff model.

---

# 6. Role Entity

## 6.1 Purpose

The Role entity represents a user's role within CivicConnect.

It supports the role-based access requirements identified during M1.

## 6.2 Proposed Attributes

| Attribute | Type | Description |
|---|---|---|
| RoleId | Unique Identifier | Primary identifier |
| RoleName | String | Name of the role |
| Description | String | Description of the role |

## 6.3 Primary Key

`RoleId`

## 6.4 Relationships

One Role may be associated with many Users.

Conceptually:

Role 1 -------- * User

The exact role catalogue must come from the approved CivicConnect stakeholder
and authorisation model.

---

# 7. ServiceRequest Entity

## 7.1 Purpose

ServiceRequest is the central entity in CivicConnect.

It represents a civic service request submitted by a requester and processed
through the CivicConnect request lifecycle.

## 7.2 Proposed Attributes

| Attribute | Type | Description |
|---|---|---|
| ServiceRequestId | Unique Identifier | Primary identifier |
| RequesterId | Foreign Key | User who submitted the request |
| StatusId | Foreign Key | Current request status |
| DepartmentId | Foreign Key / Nullable | Department responsible for the request |
| Title | String | Short description of the request |
| Description | String | Detailed request information |
| CreatedAt | DateTime | Date/time request was submitted |
| UpdatedAt | DateTime | Date/time request was last updated |

Additional attributes should only be added when supported by the approved
requirements or implementation needs.

## 7.3 Primary Key

`ServiceRequestId`

## 7.4 Foreign Keys

`RequesterId → User.UserId`

`StatusId → RequestStatus.StatusId`

`DepartmentId → Department.DepartmentId`

## 7.5 Relationships

A ServiceRequest:

- belongs to one requester;
- has one current status;
- may belong to or be routed to a department;
- may have multiple status-history records;
- may have multiple assignment records;
- may result in multiple notifications.

---

# 8. RequestStatus Entity

## 8.1 Purpose

RequestStatus represents a recognised status within the CivicConnect service
request lifecycle.

## 8.2 Proposed Attributes

| Attribute | Type | Description |
|---|---|---|
| StatusId | Unique Identifier | Primary identifier |
| StatusName | String | Name of the status |
| Description | String | Meaning of the status |
| IsActive | Boolean | Indicates whether the status is currently available |

## 8.3 Primary Key

`StatusId`

## 8.4 Relationships

One RequestStatus may be referenced by many ServiceRequests.

One RequestStatus may also appear in many StatusHistory records.

## 8.5 Important Boundary

The complete allowed lifecycle transition map is not defined by this data
model.

For example, the model must not independently assume that every status may
transition to every other status.

Allowed transitions are an application/domain design concern and must be
based on the team's approved lifecycle decision.

---

# 9. StatusHistory Entity

## 9.1 Purpose

StatusHistory records changes to the lifecycle state of a ServiceRequest.

This entity directly supports the auditability requirement that status changes
record the user, new status and time.

## 9.2 Proposed Attributes

| Attribute | Type | Description |
|---|---|---|
| StatusHistoryId | Unique Identifier | Primary identifier |
| ServiceRequestId | Foreign Key | Request whose status changed |
| StatusId | Foreign Key | New status |
| ChangedByUserId | Foreign Key | User who performed the change |
| ChangedAt | DateTime | Date/time of the change |

A previous status may also be recorded if the team determines that it is
required for audit or reporting purposes.

## 9.3 Primary Key

`StatusHistoryId`

## 9.4 Foreign Keys

`ServiceRequestId → ServiceRequest.ServiceRequestId`

`StatusId → RequestStatus.StatusId`

`ChangedByUserId → User.UserId`

## 9.5 Relationships

One ServiceRequest may have many StatusHistory records.

One User may be responsible for many status changes.

---

# 10. Department Entity

## 10.1 Purpose

Department represents an operational group responsible for handling relevant
service requests.

## 10.2 Proposed Attributes

| Attribute | Type | Description |
|---|---|---|
| DepartmentId | Unique Identifier | Primary identifier |
| DepartmentName | String | Department name |
| Description | String | Department responsibility |

## 10.3 Primary Key

`DepartmentId`

## 10.4 Relationships

A Department may be associated with multiple ServiceRequests.

A Department may also participate in multiple Assignment records.

The exact department catalogue remains project/configuration data rather than
being invented by this model.

---

# 11. Assignment Entity

## 11.1 Purpose

Assignment records responsibility for handling a ServiceRequest.

Separating assignment history from the ServiceRequest itself allows the system
to preserve assignment information as requests move between responsible
parties where required.

## 11.2 Proposed Attributes

| Attribute | Type | Description |
|---|---|---|
| AssignmentId | Unique Identifier | Primary identifier |
| ServiceRequestId | Foreign Key | Assigned request |
| DepartmentId | Foreign Key | Responsible department |
| AssignedUserId | Foreign Key / Nullable | Specific assigned staff user, if applicable |
| AssignedAt | DateTime | Assignment time |
| UnassignedAt | DateTime / Nullable | End of assignment, if applicable |

## 11.3 Primary Key

`AssignmentId`

## 11.4 Foreign Keys

`ServiceRequestId → ServiceRequest.ServiceRequestId`

`DepartmentId → Department.DepartmentId`

`AssignedUserId → User.UserId`

## 11.5 Relationships

One ServiceRequest may have multiple Assignment records over its lifecycle.

One Department may have many assignments.

A User may have multiple assignments where individual staff assignment is
supported.

---

# 12. Notification Entity

## 12.1 Purpose

Notification represents feedback generated for a CivicConnect user in
response to a relevant application event.

This supports FR-005 and the notification architecture being evaluated during
Milestone 2.

## 12.2 Proposed Attributes

| Attribute | Type | Description |
|---|---|---|
| NotificationId | Unique Identifier | Primary identifier |
| UserId | Foreign Key | Notification recipient |
| ServiceRequestId | Foreign Key | Related request |
| NotificationType | String | Type/category of notification |
| Message | String | Notification content |
| CreatedAt | DateTime | Time notification was generated |
| SentAt | DateTime / Nullable | Time notification was delivered/sent |
| Status | String | Delivery/processing status |

## 12.3 Primary Key

`NotificationId`

## 12.4 Foreign Keys

`UserId → User.UserId`

`ServiceRequestId → ServiceRequest.ServiceRequestId`

## 12.5 Relationships

One User may receive many Notifications.

One ServiceRequest may generate many Notifications.

## 12.6 Design Boundary

The Notification entity does not require the Domain layer to directly send
email, SMS or push notifications.

The Member 3 notification architecture decision will determine how
notification events and external providers are decoupled.

---

# 13. Initial Relationship Model

The initial relationships can be represented conceptually as:

Role
  |
  | 1
  |
  | *
User
  |
  | 1
  |
  | *
ServiceRequest
  |
  +--------------------+
  |                    |
  | 1                  | 1
  |                    |
  | *                  | *
StatusHistory       Assignment
  |                    |
  |                    |
RequestStatus       Department


ServiceRequest
      |
      | 1
      |
      | *
Notification
      |
      | *
      |
      | 1
     User

The final ERD will provide the formal representation of these relationships.

---

# 14. Cardinality Summary

| Relationship | Cardinality |
|---|---|
| Role → User | One-to-Many |
| User → ServiceRequest | One-to-Many |
| RequestStatus → ServiceRequest | One-to-Many |
| ServiceRequest → StatusHistory | One-to-Many |
| RequestStatus → StatusHistory | One-to-Many |
| User → StatusHistory | One-to-Many |
| Department → ServiceRequest | One-to-Many |
| ServiceRequest → Assignment | One-to-Many |
| Department → Assignment | One-to-Many |
| User → Assignment | One-to-Many where individual assignment applies |
| ServiceRequest → Notification | One-to-Many |
| User → Notification | One-to-Many |

---

# 15. Data Integrity

The initial relational model should enforce appropriate integrity rules.

Examples include:

- primary keys must be unique;
- required foreign keys must reference valid records;
- a ServiceRequest must reference a valid requester;
- a ServiceRequest must reference a valid current status;
- StatusHistory must reference an existing ServiceRequest;
- StatusHistory must identify the user responsible for the change;
- timestamps required for audit evidence must not be omitted;
- invalid orphan records should be prevented through referential integrity.

Business rules that depend on lifecycle behaviour should not be moved entirely
into database constraints if doing so would bypass the Domain/Application
decision model.

---

# 16. Consistency and Transactions

Operations that change multiple related records may require a transaction.

For example, an approved status change may conceptually require:

1. updating the ServiceRequest current status;
2. inserting a StatusHistory record.

These related persistence operations should succeed or fail together where
required to preserve consistency.

The detailed transaction and concurrency approach will be documented in the
persistence evaluation and corresponding decision record.

---

# 17. Concurrency Considerations

Multiple authorised staff members may potentially interact with service
requests.

The persistence implementation must therefore consider the possibility of
concurrent updates.

The exact concurrency-control mechanism has not yet been baselined in this
data model.

The persistence decision must determine an appropriate approach based on the
selected ORM/database and the Assignment 2 concurrency research.

---

# 18. Security and Sensitive Data

User and service-request information must be handled according to the project's
security requirements.

The data model should follow the principle of storing only information required
by the system.

Sensitive credentials must never be stored as plain text.

Authentication secrets, database credentials and external API keys must not be
stored directly in source-controlled data/configuration files.

Access to stored information must be enforced through the application's
authorisation mechanisms.

---

# 19. Growth and Scalability

The model should support growth in:

- users;
- service requests;
- status-history records;
- assignments;
- notifications.

Potential high-growth tables such as StatusHistory and Notification should be
considered during database indexing and performance work.

Specific capacity assumptions should not be invented until the team has
defined an expected workload.

---

# 20. Availability and Recovery

The initial M2 data model does not itself define a production backup and
recovery solution.

The final persistence/deployment decision should consider:

- database backup;
- restoration;
- migration management;
- failure recovery;
- data integrity.

The appropriate solution will depend on the database and deployment platform
selected by the team.

---

# 21. Architecture Alignment

The data model supports Member 1's selected Modular Monolith and
Clean/Hexagonal-style architecture.

The intended separation is:

Domain
  |
  | Core entities and business behaviour
  |
Application
  |
  | Use cases and persistence abstractions
  |
Infrastructure
  |
  | ORM mappings / repositories / database
  |
Database

The Domain layer should not depend directly on Entity Framework Core or a
specific relational database product.

This allows persistence technology to remain an infrastructure concern.

---

# 22. Proposed Implementation Mapping

If the proposed C# / ASP.NET Core / Entity Framework Core stack is approved,
the model may eventually map approximately as follows:

CivicConnect.Domain/
  Entities/
    User.cs
    ServiceRequest.cs
    StatusHistory.cs
    Department.cs
    Assignment.cs
    Notification.cs

  Enums-or-Entities/
    RequestStatus.cs
    Role.cs

CivicConnect.Infrastructure/
  Persistence/
    CivicConnectDbContext.cs

    Configurations/
      UserConfiguration.cs
      ServiceRequestConfiguration.cs
      StatusHistoryConfiguration.cs
      DepartmentConfiguration.cs
      AssignmentConfiguration.cs
      NotificationConfiguration.cs

    Migrations/

This mapping is proposed implementation guidance and must be updated if the
technology/persistence decision changes.

---

# 23. Requirements Traceability

| Data Element | Main Requirement / Engineering Need |
|---|---|
| User | Stakeholder identity and access control |
| Role | NFR-002 |
| ServiceRequest | Core CivicConnect functional requirements |
| RequestStatus | FR-010, FR-012 |
| StatusHistory | NFR-005 |
| Notification | FR-005 / AC-005.1 |
| Assignment | Request assignment/operational processing |
| Department | Operational routing/ownership |

This table should be expanded with exact requirement IDs as the M2 RTM is
updated.

---

# 24. Open Decisions

The following decisions remain open:

- [ ] Confirm final database technology.
- [ ] Confirm whether RequestStatus is stored as a lookup entity or represented
      differently in the domain.
- [ ] Confirm the approved lifecycle/status dictionary.
- [ ] Confirm the complete role catalogue.
- [ ] Confirm whether assignments are department-only or can target individual
      staff members.
- [ ] Confirm whether notification records must be persisted.
- [ ] Confirm concurrency-control approach.
- [ ] Confirm indexes after expected query patterns are identified.
- [ ] Confirm production backup/recovery approach.
- [ ] Confirm data-retention requirements if applicable.

These items must not be silently assumed during implementation.

---

# 25. Initial Data Model Decision

The proposed CivicConnect initial data model uses a relational structure
centred on ServiceRequest.

The initial model separates:

- user identity and role information;
- current service-request state;
- status-change history;
- operational assignment;
- department responsibility;
- notification information.

This separation supports the current CivicConnect requirements while allowing
the application to preserve Clean/Hexagonal-style boundaries.

The model will be refined after the database and persistence technology are
formally selected and the remaining lifecycle/authorisation details are
confirmed.

---

# 26. Status

**Status: PROPOSED**

The model may move to **ACCEPTED** once:

- [ ] the team reviews the entities;
- [ ] relationships/cardinalities are confirmed;
- [ ] the database is selected;
- [ ] the persistence approach is approved;
- [ ] the ERD is created and reviewed;
- [ ] the model is reconciled with the PED and RTM.

---

# 27. Next Action

Create the CivicConnect ERD from this model and use it as input to the
persistence evaluation.

The next engineering workflow is:

Initial Data Model
        |
        v
ERD
        |
        v
Persistence Evaluation
        |
        v
Database Selection
        |
        v
ADR / Decision Record
        |
        v
EF Core / Persistence Implementation
        |
        v
Tests
