# CivicConnect --- Project Engineering Document (PED) v2.0

## Milestone 2 --- Architecture, Technology & Initial Design Baseline

**Project:** CivicConnect\
**Module:** SEN381\
**Document Version:** 2.0\
**Status:** Integration Baseline

------------------------------------------------------------------------

# 1. Introduction

## 1.1 Purpose

This Project Engineering Document (PED) represents the Milestone 2
engineering baseline for CivicConnect.

PED v2.0 evolves the Milestone 1 requirements foundation by
incorporating architecture decisions, data and persistence decisions,
technology selection, design decisions, implementation evidence,
verification evidence and governance updates.

The document connects:

-   stakeholder needs;
-   requirements;
-   acceptance criteria;
-   architecture;
-   data design;
-   technology choices;
-   implementation;
-   testing;
-   project governance.

------------------------------------------------------------------------

# 2. Project Overview

CivicConnect is a civic service-request management platform designed to
connect citizens with municipal service departments.

The system enables citizens to submit service requests while allowing
operational staff to manage, update and track requests through
controlled lifecycle processes.

Milestone 2 establishes the initial engineering baseline through:

-   modular monolith architecture;
-   ASP.NET Core Web API;
-   Entity Framework Core persistence;
-   SQL Server database support;
-   service-request lifecycle governance;
-   notification architecture;
-   external integration boundaries;
-   automated verification.

------------------------------------------------------------------------

# 3. Architecture Baseline

## 3.1 Selected Architecture

CivicConnect uses a modular monolith architecture.

The system separates responsibilities into:

    Web API
       |
    Application
       |
    Domain
       |
    Infrastructure

## Domain Layer

Responsible for:

-   entities;
-   business rules;
-   domain events;
-   domain exceptions.

Examples:

-   ServiceRequest
-   RequestStatus
-   StatusHistory
-   Notification

## Application Layer

Responsible for:

-   application workflows;
-   abstractions;
-   use cases.

Examples:

-   IServiceRequestLifecycleService
-   INotificationService
-   ILocationResolver

## Infrastructure Layer

Responsible for:

-   persistence;
-   database access;
-   external integrations.

Examples:

-   CivicConnectDbContext
-   NotificationService
-   ExternalLocationResolver

------------------------------------------------------------------------

# 4. Data and Persistence Strategy

CivicConnect uses relational persistence through:

-   SQL Server;
-   Entity Framework Core;
-   explicit entity configurations.

Main entities include:

-   User
-   Role
-   ServiceRequest
-   RequestStatus
-   StatusHistory
-   Notification
-   Assignment
-   Department

Persistence responsibilities are isolated inside the Infrastructure
layer.

------------------------------------------------------------------------

# 5. Technology Stack

  Technology              Purpose
  ----------------------- ----------------------
  C#                      Programming language
  .NET                    Application platform
  ASP.NET Core Web API    REST API
  Entity Framework Core   ORM
  SQL Server              Database
  xUnit                   Automated testing
  Swagger/OpenAPI         API documentation
  GitHub                  Source control

------------------------------------------------------------------------

# 6. Milestone 2 Design Decisions

## 6.1 Notification Feedback Architecture

### Problem

Service-request lifecycle logic should not be tightly coupled to
notification creation.

### Selected Approach

Event-driven notification architecture.

Flow:

    ServiceRequest Status Change
            |
            v
    ServiceRequestStatusChangedEvent
            |
            v
    ServiceRequestStatusChangedHandler
            |
            v
    INotificationService
            |
            v
    NotificationService
            |
            v
    Notifications Table

Implementation evidence:

-   ServiceRequestStatusChangedEvent
-   ServiceRequestStatusChangedHandler
-   INotificationService
-   NotificationService

------------------------------------------------------------------------

## 6.2 External Location Provider Boundary

### Problem

CivicConnect may require location services without depending directly on
a specific external provider.

### Selected Approach

Adapter boundary using:

    ILocationResolver

Structure:

    Application

    ILocationResolver

            ^

    Infrastructure

    ExternalLocationResolver

A live third-party geocoding provider remains future work.

------------------------------------------------------------------------

# 7. Forward Engineering Implementation

## Service Request Lifecycle Governance

The lifecycle implementation prevents invalid status transitions.

Flow:

    PATCH /api/ServiceRequests/{id}/status

            |

    ServiceRequestLifecycleService

            |

    StatusTransitionPolicy

Example:

Valid:

    Submitted -> Assigned

Result:

    204 No Content

Invalid:

    Assigned -> Closed

Result:

    400 Bad Request

------------------------------------------------------------------------

# 8. Verification Evidence

Automated tests cover:

-   lifecycle transition rules;
-   invalid transition rejection;
-   persistence behaviour;
-   notification creation;
-   location provider boundary.

Projects:

    CivicConnect.Domain.Tests

    CivicConnect.Application.Tests

    CivicConnect.Infrastructure.Tests

Manual verification includes:

-   Swagger API testing;
-   database evidence;
-   status history records;
-   notification records.

------------------------------------------------------------------------

# 9. Traceability Summary

Example trace:

    FR-010
     |
    Lifecycle Governance Requirement
     |
    Architecture Decision
     |
    IStatusTransitionPolicy
     |
    ServiceRequestLifecycleService
     |
    PATCH API Endpoint
     |
    Automated Tests
     |
    Database Verification

------------------------------------------------------------------------

# 10. Known Limitations

The following remain future work:

-   authentication and authorisation;
-   complete citizen workflow;
-   staff assignment workflow;
-   reporting dashboards;
-   external notification providers;
-   live location provider integration;
-   production deployment.

------------------------------------------------------------------------

# 11. GitHub Governance

Development follows:

    Feature Branch
            |
            v
    Pull Request
            |
            v
    Review
            |
            v
    Integration Branch
            |
            v
    Main Branch

------------------------------------------------------------------------

# 12. Milestone 2 Baseline Status

Completed:

-   Architecture baseline
-   Technology selection
-   Persistence strategy
-   Design decisions
-   Initial implementation
-   API endpoint
-   Lifecycle governance
-   Notification architecture
-   Automated verification

CivicConnect will continue evolving in later milestones.
