# CivicConnect — Architecture Baseline & System Design (Milestone 2)

## 1. Evaluation of Architectural Alternatives

| Architectural Style | Description | Strengths for CivicConnect | Weaknesses & Fatal Flaws | Verdict |
|---|---|---|---|---|
| **Style 1: Distributed Microservices** | Independent deployable services (Auth, Ticket, Reporting, Audit) communicating across HTTP/gRPC network boundaries. | Independent scaling; isolated service failures. | Severe network latency and serialization overhead jeopardizes the 3.0s latency SLA (NFR-001). Distributed transactions require two-phase commits or Saga patterns, which are completely disproportionate for a 3-person student team with zero budget. | **REJECTED** |
| **Style 2: Traditional Layered Monolith (3-Tier)** | Presentation, Business Logic, and Data Access Layers sharing a common runtime and database without enforced internal boundaries. | Simple initial setup; familiar paradigm. | Controllers routinely bypass domain logic to call database contexts directly, causing architectural erosion. Makes enforcing strict state-machine rules brittle and resists isolated automated unit testing (NFR-007). | **REJECTED** |
| **Style 3: Modular Monolith (Clean / Hexagonal)** | A single deployable process structured internally into distinct, decoupled bounded modules interacting via explicit in-memory interface contracts. | Eliminates network latency (in-memory execution ensures NFR-001 compliance); zero infrastructure cost; strictly isolates domain rules and POPIA data; enables automated unit testing without database dependencies (NFR-007). | Requires strict code review discipline to prevent developers from making direct cross-module database calls. | **SELECTED BASELINE** |

---

## 2. Logical Module Decomposition & Layer Boundaries

* **Presentation Layer (`CivicConnect.WebUI / API`):** Handles HTTP user interactions for Citizen Portal, Staff Console, and Management Dashboards. Translates user inputs into structured application commands without containing business rules.
* **Application Layer (`CivicConnect.Application`):** Coordinates application use cases via Command Handlers (write operations: submit, assign, update) and Query Handlers (read operations: search, filter, reporting).
* **Domain Layer (`CivicConnect.Domain`):** Fully isolated core domain model containing the Ticket Aggregate, business validation rules, and the Finite State Machine engine enforcing valid lifecycle transitions. Zero dependencies on databases or UI frameworks.
* **Infrastructure Layer (`CivicConnect.Infrastructure`):** Implements persistence repositories, database mappings, role-based access providers (NFR-002), and the immutable audit logger (NFR-005).

---

## 3. Architecture Diagrams

### Logical Architecture Diagram
```mermaid
graph TD
    subgraph Presentation_Layer [Presentation Layer / UI]
        CitizenUI[Citizen Web Portal - FR-001 to FR-005]
        StaffUI[Staff Operations Portal - FR-006 to FR-012]
        MgmtUI[Management Dashboard - FR-013 to FR-016]
    end

    subgraph Application_Layer [Application & Orchestration Layer]
        Commands[Command Handlers: Submit, Assign, Update Status]
        Queries[Query Handlers: Search, Filter, Management Analytics]
    end

    subgraph Domain_Layer [Core Domain Model - Fully Isolated]
        TicketAgg[Ticket Aggregate Entity]
        StateEngine[FSM State Transition Engine - AC-010.2]
        DomainEvents[Domain Events: StatusChangedEvent]
    end

    subgraph Infrastructure_Layer [Infrastructure & Persistence]
        DBContext[Database Context & Repositories]
        AuditLog[Immutable Audit Logger - NFR-005]
        RBACService[Role Authorization Provider - NFR-002]
    end

    CitizenUI -->|Submit / View| Commands
    StaffUI -->|Update / Assign| Commands
    MgmtUI -->|Read Aggregate Data| Queries

    Commands --> Domain_Layer
    Queries --> DBContext

    Domain_Layer -->|Triggers| DomainEvents
    DomainEvents -->|Captures State Change| AuditLog

    Infrastructure_Layer -.->|Implements Interfaces| Application_Layer
    Domain_Layer -->|Persisted By| DBContext
    Presentation_Layer -->|Enforces Permissions| RBACService
```

### Sequence Trace Diagram
```mermaid
sequenceDiagram
    autonumber
    actor Staff as Operational Staff
    participant UI as Presentation Layer
    participant App as Application Layer
    participant FSM as Domain State Engine
    participant Repo as Infrastructure Repository
    participant Audit as Immutable Audit Logger

    Staff->>UI: Request Status Change (e.g., In-Progress -> Resolved)
    UI->>App: Send TransitionStatusCommand(TicketId, NewState, StaffId)
    App->>Repo: Fetch Ticket Aggregate by TicketId
    Repo-->>App: Return Ticket Aggregate
    App->>FSM: Validate Transition(CurrentState, NewState)
    alt Invalid Transition
        FSM-->>App: Throw InvalidStateTransitionException
        App-->>UI: Return HTTP 400 Bad Request (AC-010.2)
        UI-->>Staff: Display Error: "Invalid status workflow change"
    else Valid Transition
        FSM-->>App: Transition Approved
        App->>Repo: Save Updated Ticket Aggregate
        App->>Audit: Append Audit Record(TicketId, OldState, NewState, StaffId, Timestamp)
        Audit-->>App: Acknowledge Persistence
        App-->>UI: Return HTTP 200 OK (AC-010.3)
        UI-->>Staff: Display Updated Status
    end
```