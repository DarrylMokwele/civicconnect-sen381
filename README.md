# CivicConnect

## SEN381 — Milestone 2

### Architecture, Technology & Initial Design Baseline

---

# 1. Project Overview

CivicConnect is a civic service-request management platform designed to
connect citizens with municipal service departments.

The system allows citizens to submit service requests while enabling
operational staff to manage, update and track request progress through
controlled lifecycle processes.

The Milestone 2 implementation establishes the initial engineering
baseline by introducing:

- modular monolith architecture;
- ASP.NET Core Web API;
- Entity Framework Core persistence;
- SQL Server database support;
- service-request lifecycle governance;
- notification architecture;
- external integration boundaries;
- automated verification.

---

# 2. Technology Stack

## Backend

| Technology | Purpose |
|---|---|
| C# | Programming language |
| .NET | Application platform |
| ASP.NET Core Web API | REST API |
| Entity Framework Core | Object relational mapping |
| SQL Server | Relational persistence |
| Swagger/OpenAPI | API documentation |

---

## Testing

| Technology | Purpose |
|---|---|
| xUnit | Automated testing |
| EF Core InMemory Database | Persistence testing |

---

## Development Tools

| Tool | Purpose |
|---|---|
| Visual Studio | Application development |
| GitHub | Source control and project management |
| GitHub Projects | Task tracking |
| GitHub Pull Requests | Code review |

---

# 3. Solution Architecture

CivicConnect follows a modular monolith architecture.

The solution separates responsibilities into four main layers:

```
CivicConnect

│
├── Domain
│
├── Application
│
├── Infrastructure
│
└── WebApi
```

---

# 4. Project Structure

## Domain

Responsible for:

- business entities;
- domain events;
- domain exceptions;
- core business rules.

Examples:

```
ServiceRequest
RequestStatus
StatusHistory
Notification
InvalidStatusTransitionException
```

---

## Application

Responsible for:

- application workflows;
- abstractions;
- business use cases.

Examples:

```
IServiceRequestLifecycleService

IStatusTransitionPolicy

INotificationService

ILocationResolver
```

---

## Infrastructure

Responsible for:

- database access;
- EF Core configurations;
- external integrations.

Examples:

```
CivicConnectDbContext

NotificationService

ExternalLocationResolver
```

---

## Web API

Responsible for:

- HTTP endpoints;
- API controllers;
- dependency injection configuration.

Example:

```
ServiceRequestsController
```

---

# 5. Database Setup

## Database Technology

CivicConnect uses:

```
SQL Server
+
Entity Framework Core
```

The database contains entities including:

```
Users

Roles

ServiceRequests

RequestStatuses

StatusHistories

Notifications

Assignments

Departments
```

---

# 6. Configuration

The database connection string is configured in:

```
WebApi/appsettings.json
```

Example:

```json
{
  "ConnectionStrings": {
    "CivicConnectDatabase":
    "Server=(localdb)\\MSSQLLocalDB;Database=CivicConnectDb;Trusted_Connection=True;"
  }
}
```

---

# 7. Running the Application

## Requirements

Install:

- Visual Studio 2022+
- .NET SDK
- SQL Server LocalDB

---

## Clone Repository

```bash
git clone <repository-url>
```

---

## Open Solution

Open:

```
CivicConnect.sln
```

---

## Restore Packages

Visual Studio automatically restores packages.

Alternatively:

```bash
dotnet restore
```

---

## Apply Database Migration

Open Package Manager Console:

Run:

```powershell
Update-Database
```

---

## Run Application

Set:

```
WebApi
```

as the startup project.

Run:

```
Ctrl + F5
```

Swagger should open.

---

# 8. API Documentation

Swagger/OpenAPI is available when running in development mode.

Available endpoint:

## Update Service Request Status

```
PATCH

/api/ServiceRequests/{id}/status
```

Request:

```json
{
  "newStatusId":
  "44444444-4444-4444-4444-444444444444",

  "changedByUserId":
  "22222222-2222-2222-2222-222222222222"
}
```

---

# 9. Lifecycle Governance

CivicConnect prevents invalid service-request transitions.

The lifecycle flow is controlled through:

```
IStatusTransitionPolicy

        ↓

StatusTransitionPolicy

        ↓

ServiceRequestLifecycleService
```

Example:

Allowed:

```
Submitted
     |
     v
Assigned
```

Rejected:

```
Assigned
     |
     X
     v
Closed
```

Invalid transitions return:

```
400 Bad Request
```

---

# 10. Notification Architecture

Notifications are separated from request lifecycle logic.

Flow:

```
ServiceRequest Status Change

        ↓

ServiceRequestStatusChangedEvent

        ↓

ServiceRequestStatusChangedHandler

        ↓

INotificationService

        ↓

NotificationService

        ↓

Notifications Table
```

The architecture allows future support for:

- email notifications;
- SMS notifications;
- push notifications.

---

# 11. External Location Provider Boundary

CivicConnect isolates external location providers behind:

```
ILocationResolver
```

Implementation:

```
Application

ILocationResolver

        ↑

Infrastructure

ExternalLocationResolver
```

This prevents direct dependency on a specific mapping provider.

Live geocoding integration remains future work.

---

# 12. Testing

The solution contains:

```
CivicConnect.Domain.Tests

CivicConnect.Application.Tests

CivicConnect.Infrastructure.Tests
```

Tests cover:

- lifecycle transition rules;
- invalid transition rejection;
- persistence behaviour;
- notification creation;
- location provider boundary.

Run tests:

Visual Studio:

```
Test
 ↓
Test Explorer
 ↓
Run All Tests
```

or:

```bash
dotnet test
```

---

# 13. Milestone 2 Verification Evidence

The following behaviour has been demonstrated:

## Valid Transition

```
Submitted → Assigned
```

Result:

```
204 No Content
```

Evidence:

- API response;
- updated ServiceRequest;
- StatusHistory record;
- Notification record.

---

## Invalid Transition

```
Assigned → Closed
```

Result:

```
400 Bad Request
```

Evidence:

- transition rejected;
- request unchanged;
- no additional history;
- no additional notification.

---

# 14. Repository Structure

```
CivicConnect

│

├── Domain

├── Application

├── Infrastructure

├── WebApi

├── Tests

├── docs

│   ├── architecture

│   ├── decisions

│   ├── governance

│   ├── requirements

│   └── technology

│

├── README.md

└── CivicConnect.sln
```

---

# 15. Known Limitations

The following features remain future work:

- authentication and authorisation;
- complete citizen request submission workflow;
- staff assignment workflows;
- search and filtering;
- reporting dashboards;
- external notification providers;
- live external geolocation provider;
- production deployment.

---

# 16. Contribution Workflow

Development follows GitHub-based collaboration:

```
Feature Branch

        ↓

Pull Request

        ↓

Review

        ↓

Integration Branch

        ↓

Main Branch
```

---

# 17. Milestone 2 Status

Current baseline includes:

✅ Architecture baseline  
✅ Technology selection  
✅ Persistence strategy  
✅ Design decisions  
✅ Initial implementation  
✅ API endpoint  
✅ Lifecycle governance  
✅ Notification architecture  
✅ Automated verification  

The system will continue evolving in future milestones.
