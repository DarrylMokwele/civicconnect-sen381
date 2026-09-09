# CivicConnect Acceptance Criteria

## Requester Requirements

| Requirement | Acceptance Criteria |
|---|---|
| FR-001 | AC-001.1 A valid request is saved. AC-001.2 The request receives a unique ID. AC-001.3 The requester receives confirmation. AC-001.4 Missing required information prevents submission and shows what must be corrected. |
| FR-002 | AC-002.1 The requester can select a category from the available list. AC-002.2 The selected category is saved with the request. |
| FR-003 | AC-003.1 The requester can open one of their requests and see its current status. AC-003.2 The requester cannot view another requester's restricted request information. |
| FR-004 | AC-004.1 The requester can view a list of their previous requests. AC-004.2 Each listed request shows enough information to identify it and see its current status. |
| FR-005 | AC-005.1 The requester receives clear feedback when a request is submitted, rejected, updated or completed. |

## Operational Staff Requirements

| Requirement | Acceptance Criteria |
|---|---|
| FR-006 | AC-006.1 Authorised staff can view requests relevant to their work. AC-006.2 Users without permission cannot use restricted staff functions. |
| FR-007 | AC-007.1 Staff can search requests. AC-007.2 Staff can filter requests by status and category. AC-007.3 Staff can sort requests using supported fields. |
| FR-008 | AC-008.1 An authorised staff member can open a request and view its available details. |
| FR-009 | AC-009.1 Authorised staff can take or be assigned responsibility for a request. AC-009.2 The responsible staff member is recorded. |
| FR-010 | AC-010.1 Only authorised staff can change a request's status. AC-010.2 Invalid status changes are rejected. AC-010.3 A valid status change is saved and displayed. |
| FR-011 | AC-011.1 Authorised staff can add an action, comment or resolution detail. AC-011.2 The system records who added it and when. |
| FR-012 | AC-012.1 Authorised staff can resolve or close an eligible request. AC-012.2 The new status is saved. |

## Management Requirements

| Requirement | Acceptance Criteria |
|---|---|
| FR-013 | AC-013.1 An authorised management user can view service request activity. |
| FR-014 | AC-014.1 Management can identify open requests. AC-014.2 Management can identify overdue requests. AC-014.3 Management can identify resolved and closed requests. |
| FR-015 | AC-015.1 Management can view or filter request information by category. AC-015.2 Management can view or filter request information by status. |
| FR-016 | AC-016.1 Management can view information used to check service performance and accountability. AC-016.2 The information comes from stored CivicConnect request records. |

## Non-Functional Requirements

| Requirement | Acceptance Criteria |
|---|---|
| NFR-001 | A performance test shows that at least 95% of normal user actions complete within 3 seconds under the workload agreed by the team. |
| NFR-002 | Role tests show that users cannot access functions or request information outside their allowed access. |
| NFR-003 | The system does not show a successful submission unless the request has been stored. |
| NFR-004 | At least 4 out of 5 representative users submit a valid standard request without assistance. |
| NFR-005 | Each tested status change contains the user, new status and time of the change. |
| NFR-006 | Monitoring for the agreed test period shows at least 99% availability during agreed operational hours, excluding planned maintenance. |
