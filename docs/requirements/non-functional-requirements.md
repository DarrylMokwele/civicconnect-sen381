# CivicConnect Non-Functional Requirements

The project brief requires measurable non-functional requirements. The targets below must be reviewed and agreed on by the team before the requirements baseline is approved.

| ID | Category | Requirement | Source | Priority |
|---|---|---|---|---|
| NFR-001 | Performance | At least 95% of normal user actions shall complete within 3 seconds under the workload agreed by the team. | All stakeholders | Must |
| NFR-002 | Security | Users shall only access functions and request information allowed for their role. | All stakeholders | Must |
| NFR-003 | Reliability | The system shall store a service request before confirming that the submission was successful. | STK-001 | Must |
| NFR-004 | Usability | At least 4 out of 5 representative test users shall submit a valid standard request without assistance. | STK-001 | Should |
| NFR-005 | Auditability | For each tested status change, the system shall record the user, new status and time of the change. | STK-002 / STK-003 | Must |
| NFR-006 | Availability | The system should achieve at least 99% availability during the operational hours agreed by the team, excluding planned maintenance. | All stakeholders | Should |
