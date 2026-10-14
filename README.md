# CivicConnect

## SEN381 — Milestone 3 Release Candidate

CivicConnect is a civic service-request management platform that connects citizens with service departments and operational staff. Citizens can submit and track service requests, while authorised staff and administrators can manage request lifecycles, assignments, notifications and operational activity.

This repository contains the integrated Milestone 3 release candidate, including the backend API, SQL Server persistence, lifecycle governance, automated testing, frontend, staff/admin authentication, operational readiness tooling and supporting governance/evidence documents.

---

# 1. System Overview

CivicConnect is implemented as a modular monolith:

```text
CivicConnect
│
├── Domain
├── Application
├── Infrastructure
└── WebApi
```

The application provides three user experiences:

### Citizen

Citizens do **not** need to log in.

They can:

- View the citizen portal.
- View request information available through the portal.
- Submit service requests.
- Track request status.
- View notifications.

### Staff

Staff members must authenticate.

They can:

- View the staff work overview.
- View the request queue.
- View assigned requests.
- Update permitted request statuses.
- View notifications.
- Log out and sign in again.

### Administrator

Administrators must authenticate.

They can:

- View the management dashboard.
- Manage service requests.
- Change request status through the lifecycle service.
- Assign/reassign staff and departments where supported.
- View reports and operational activity.
- View notifications.
- Manage staff assignments.
- Log out and sign in again.

---

# 2. Technology Stack

| Technology | Purpose |
|---|---|
| C# | Programming language |
| .NET 8 | Application platform |
| ASP.NET Core Web API | REST API |
| Entity Framework Core 8 | ORM / data access |
| SQL Server | Relational database |
| Swagger / OpenAPI | API documentation |
| HTML / CSS / JavaScript | Frontend |
| xUnit | Automated testing |
| Docker / Docker Compose | Local staging environment |
| PowerShell | Release/staging scripts |
| GitHub Actions | CI automation |

---

# 3. Solution Structure

```text
CivicConnect/
│
├── Application/
│   ├── Abstractions/
│   ├── Notifications/
│   ├── ServiceRequests/
│   └── Locations/
│
├── Domain/
│
├── Infrastructure/
│   ├── Persistence/
│   ├── Notifications/
│   ├── ServiceRequests/
│   └── Location/
│
├── WebApi/
│   ├── Controllers/
│   ├── Contracts/
│   ├── Health/
│   ├── Observability/
│   └── wwwroot/
│       ├── index.html
│       ├── css/site.css
│       └── js/app.js
│
├── tests/
│   ├── CivicConnect.Api.Tests/
│   ├── CivicConnect.Application.Tests/
│   ├── CivicConnect.Domain.Tests/
│   └── CivicConnect.Infrastructure.Tests/
│
├── docs/
├── performance/
├── scripts/
├── docker-compose.staging.yml
├── .env.example
└── CivicConnect.sln
```

---

# 4. Architecture

CivicConnect uses a modular monolith with separation of concerns.

## Domain

Contains core business concepts and rules, including service-request lifecycle concepts and domain exceptions.

## Application

Contains application workflows and abstractions such as:

```text
IServiceRequestLifecycleService
IStatusTransitionPolicy
INotificationService
ILocationResolver
```

## Infrastructure

Contains persistence and infrastructure implementations, including:

```text
CivicConnectDbContext
NotificationService
ServiceRequestLifecycleService
ExternalLocationResolver
```

## WebApi

Provides HTTP endpoints, authentication, dependency injection, health checks, Swagger and the browser frontend.

---

# 5. Frontend

The frontend is served directly by ASP.NET Core from:

```text
CivicConnect/WebApi/wwwroot/
```

The main application is available at `/` when the Web API is running.

## Citizen portal

The citizen interface includes:

- Overview dashboard
- Request statistics
- Recent requests
- Submit request
- My requests
- Request details
- Request status information
- Notifications
- Search/filtering where supported

## Staff portal

The staff interface includes:

- Work overview
- Request queue
- My assigned requests
- Request details
- Status updates
- Notifications
- Profile/logout controls

## Administrator portal

The administrator interface includes:

- Management overview
- Service request management
- Request queue
- Reports
- Activity history
- Notifications
- Staff and assignment management
- Status management

The frontend is responsive and designed around a service-management dashboard layout with a persistent navigation area, KPI cards, tables, status badges, request timelines and operational panels.

---

# 6. Authentication and Access

Citizens do not require authentication.

Staff and administrators authenticate through the staff/admin login page. The login page uses one login form; users do not select a role manually. The backend determines the account role after authentication.

The password field includes a Show/Hide control.

Authenticated Staff/Admin users see logout instead of the Staff/Admin sign-in option. After logout, they can return to the login page and authenticate again.

The Web API uses ASP.NET Core cookie authentication for the administrator/staff console.

> Development/demo credentials must not be reused in production. Production credentials should be stored in an appropriate secret/identity-management system.

---

# 7. API Endpoints

Current application endpoints include:

## Departments

```http
GET /api/Departments
```

## Notifications

```http
GET /api/Notifications
```

## Service Requests

```http
GET   /api/ServiceRequests
POST  /api/ServiceRequests
GET   /api/ServiceRequests/{id}
PATCH /api/ServiceRequests/{id}/status
```

## Statuses

```http
GET /api/Statuses
```

## Health

```http
GET /health/live
GET /health/ready
```

The liveness endpoint verifies that the application process is running. The readiness endpoint includes database connectivity.

## Swagger

When running in Development or Staging, Swagger is available at:

```text
/swagger
```

---

# 8. Service Request Lifecycle

Status changes are controlled by the domain/application lifecycle rules rather than by the frontend directly changing database values.

```text
Frontend / API request
        ↓
ServiceRequestLifecycleService
        ↓
StatusTransitionPolicy
        ↓
CivicConnectDbContext
        ↓
SQL Server
```

Valid transitions are accepted according to the configured policy.

Invalid transitions are rejected. For example, the previously demonstrated transition:

```text
Assigned → Closed
```

is rejected when it is not permitted by the lifecycle policy and returns a `400 Bad Request` response.

---

# 9. Status History and Notifications

A successful status change can produce the following flow:

```text
Service Request Status Change
          ↓
ServiceRequestStatusChangedEvent
          ↓
ServiceRequestStatusChangedHandler
          ↓
NotificationService
          ↓
Notifications table
```

Status history records preserve the lifecycle activity associated with a Service Request.

This supports traceability of:

- Service Request
- Status
- User responsible for the change
- Change timestamp

---

# 10. Database

CivicConnect uses SQL Server through Entity Framework Core.

The persistence model includes areas for:

```text
Users
Roles
Departments
ServiceRequests
RequestStatuses
StatusHistories
Notifications
Assignments
```

The database connection is configured through:

```text
CivicConnect/WebApi/appsettings.json
```

Do not commit production passwords or secrets to source control.

---

# 11. Configuration

The application uses environment-specific configuration files:

```text
WebApi/appsettings.json
WebApi/appsettings.Development.json
WebApi/appsettings.Staging.json
```

Database lifecycle behaviour is controlled by configuration:

```json
"Database": {
  "ApplyMigrations": false,
  "SeedData": false
}
```

For development/staging, these values may be enabled as required by the setup instructions.

Production environments should not automatically mutate database schema or seed development data.

---

# 12. Running Locally

## Requirements

Install:

- Visual Studio 2022 or later
- .NET 8 SDK
- SQL Server / SQL Server LocalDB

Optional for staging:

- Docker Desktop
- PowerShell

## Open the solution

Open:

```text
CivicConnect/CivicConnect.sln
```

## Restore

```bash
dotnet restore
```

## Database

Configure the `CivicConnectDatabase` connection string in the appropriate appsettings/environment configuration.

If migrations are required and the project is configured for migration execution, use the project's EF Core migration process before running the API.

## Run

Set `WebApi` as the startup project and run with Visual Studio.

The exact HTTPS port is defined by the active launch profile. Use the URL shown by Visual Studio/ASP.NET Core.

The application root `/` opens the frontend.

Swagger is available at `/swagger` in Development/Staging.

---

# 13. Automated Testing

The solution contains:

```text
CivicConnect.Api.Tests
CivicConnect.Application.Tests
CivicConnect.Domain.Tests
CivicConnect.Infrastructure.Tests
```

Tests cover areas including:

- Status-transition rules
- Service-request lifecycle behaviour
- Notification behaviour
- Persistence
- Location-provider boundaries
- API/integration behaviour
- Authentication/authorisation checks
- Health and observability
- End-to-end lifecycle behaviour

Run all tests from Visual Studio:

```text
Test → Test Explorer → Run All Tests
```

or:

```bash
dotnet test
```

Do not claim a test result in project documentation until the test has actually been executed against the current release candidate.

---

# 14. End-to-End Testing

The critical Service Request journey is intended to verify the integrated path:

```text
Create Service Request
        ↓
Submitted
        ↓
Assigned
        ↓
In Progress
        ↓
Resolved
        ↓
Closed
```

The E2E verification checks the resulting Service Request state and associated StatusHistory/Notification behaviour.

A negative E2E scenario verifies that an invalid transition is rejected without incorrectly changing the request state.

---

# 15. Black-Box Testing

The M3 testing strategy uses appropriate black-box techniques for the selected behaviours, including:

- Equivalence Partitioning
- Boundary Value Analysis
- Decision Table Testing
- State-Transition Testing

State-transition testing is particularly relevant to the Service Request lifecycle because not every status can transition directly to every other status.

---

# 16. Performance Testing

A JMeter test plan is included at:

```text
performance/CivicConnect_Performance.jmx
```

The configured exercise uses a controlled read-only workload so that performance testing does not deliberately corrupt Service Request lifecycle state.

The current test configuration is intended as an initial performance baseline rather than a production capacity claim.

Results must be generated from an actual JMeter run and retained as evidence.

---

# 17. Production Readiness and Operations

Member 3 operational work includes:

## Health checks

```text
/health/live
/health/ready
```

## Observability

Request logging middleware records operational request information including:

- HTTP method
- Request path
- Response status
- Elapsed time
- Correlation ID

Sensitive request bodies and authorization headers are not intended to be logged.

## Release checks

```text
scripts/release-check.ps1
```

The release check performs restore, Release build validation and automated tests.

## Staging

```text
docker-compose.staging.yml
scripts/staging-start.ps1
scripts/staging-stop.ps1
scripts/staging-verify.ps1
```

## Recovery

```text
scripts/recovery-drill.ps1
docs/operations/recovery-runbook.md
```

## Security/quality checks

```text
scripts/check_secrets.py
scripts/check_dependencies.py
```

---

# 18. CI/CD

The GitHub Actions workflow is located at:

```text
.github/workflows/ci.yml
```

The workflow is intended to provide repeatable automated checks such as:

```text
Restore
  ↓
Build
  ↓
Tests
  ↓
Security / dependency checks
```

CI results should be treated as execution evidence only when the workflow has actually completed successfully for the release candidate.

---

# 19. Documentation and Governance

The repository contains supporting M3 documentation including:

```text
docs/governance/PED.md
docs/governance/rtm.md
docs/governance/risk-register.md
docs/governance/defect-register.md
docs/governance/technical-debt-register.md
docs/governance/ai-usage-register.md
```

Operational documentation includes:

```text
docs/operations/production-readiness.md
docs/operations/observability.md
docs/operations/recovery-runbook.md
docs/operations/performance-test.md
```

Evidence documentation includes:

```text
docs/evidence/member3-execution-checklist.md
docs/evidence/member3-final-evidence-index.md
docs/evidence/member3-status.md
docs/evidence/release-candidate.md
```

These documents support the final milestone evidence package.

---

# 20. Repository Workflow

Development follows a branch and pull-request workflow:

```text
Feature Branch
      ↓
Pull Request
      ↓
Review
      ↓
Integration
      ↓
Main
```

Pull requests should contain enough information to explain the change and the verification performed.

---

# 21. Security Notes

- Do not commit real passwords, API keys or production secrets.
- Use environment variables or an appropriate secret-management system for deployment credentials.
- Staff/admin operations must remain protected by authorisation.
- Status changes must continue to pass through lifecycle/business rules.
- Logging should not expose passwords, authentication headers or sensitive request bodies.
- Development/demo credentials are not production credentials.

---

# 22. Known Limitations / Future Enhancements

The following areas can be expanded beyond the current release candidate where required:

- Production-grade identity provider integration
- Advanced role/permission administration
- Live external location/geocoding provider
- External email/SMS/push notification providers
- Advanced analytics and reporting
- Production cloud deployment
- Production-scale performance and capacity testing

These are distinct from the current implemented release-candidate functionality and should not be presented as already completed unless corresponding implementation/evidence exists.

---

# 23. Milestone 3 Contribution Summary

The integrated M3 release candidate includes the following Member 3 contribution areas:

```text
Production Readiness
        ↓
Deployment / Staging
        ↓
Health Checks
        ↓
Observability
        ↓
Security / Quality Automation
        ↓
Release Validation
        ↓
Recovery Documentation / Drill
        ↓
Frontend Integration
        ↓
Staff / Admin Operational Console
```

The remaining submission evidence must be based on actual execution of the release candidate, including final test, staging, recovery and performance results.

---

# 24. Quick Start

For the normal Visual Studio workflow:

```text
1. Open CivicConnect.sln
2. Restore NuGet packages
3. Confirm SQL Server/database configuration
4. Set WebApi as startup project
5. Rebuild Solution
6. Run Test Explorer → Run All Tests
7. Run WebApi
8. Open /
9. Test Citizen workflow
10. Test Staff/Admin login and logout
11. Test request lifecycle
12. Verify database changes
13. Test /health/live and /health/ready
14. Run release/staging checks when ready
```

For staging and M3 operational verification, follow the instructions in:

```text
SETUP_M3.md
```

---

# CivicConnect

**SEN381 — Milestone 3 Release Candidate**

Integrated civic service-request platform with a Web API, SQL Server persistence, governed request lifecycle, automated verification, responsive frontend, staff/admin operations and production-readiness tooling.
