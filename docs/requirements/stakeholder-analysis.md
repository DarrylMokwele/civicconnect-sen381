# CivicConnect Stakeholder Analysis

## ID Key

| ID | Meaning |
|---|---|
| STK | Stakeholder |
| FR | Functional Requirement |
| NFR | Non-Functional Requirement |
| AC | Acceptance Criterion |

## Stakeholders

| ID | Stakeholder | Main Needs | Interest | Influence |
|---|---|---|---|---|
| STK-001 | Requesters | Submit and categorise requests, view request status and history, and receive feedback. | High | Medium |
| STK-002 | Operational Staff | View relevant requests, search and filter requests, take responsibility, update status, record actions, and resolve or close requests. | High | High |
| STK-003 | Management / Oversight | Monitor service activity, identify open, overdue, resolved and closed requests, and view useful service information. | High | High |

## Stakeholder Conflicts

### Quick Submission vs Enough Information

Requesters want a simple form that is quick to complete. Staff still need enough information to understand and handle the request.

**Requirement effect:** Only ask for information needed to process the request.

### Request Visibility vs Access Control

Requesters need to see their own requests, but they should not see information they are not allowed to access.

**Requirement effect:** Access to requests and system functions must depend on the user's permissions.

### Management Information vs Staff Workload

Management needs records to monitor requests and accountability. Staff should not have to capture unnecessary information.

**Requirement effect:** Record important request actions without adding unnecessary admin work.
