### File 3: Architecture Decision Record (`docs/adr/ADR-001-modular-monolith-architecture.md`)

```markdown
# ADR-001: Selection of Modular Monolith (Clean Architecture) Style
* **Status:** Approved / Baselined
* **Date:** 29 September 2026
* **Deciders:** Member 1 (Maqoba Mphelo), Member 2 (Jonathan Rossouw), Member 3 (Darryl Mokwele)
* **Traced Requirements & ASRs:** ASR-01, ASR-02, ASR-03, NFR-001, NFR-002, NFR-007, NFR-008

## Context and Problem Statement
CivicConnect requires a responsive (<3.0s latency), auditable, and secure service request management system. The software must be delivered by a three-person student engineering team working under fixed academic milestone deadlines and an absolute zero capital budget constraint. The architectural challenge is to enforce strict domain boundaries, lifecycle state-machine governance, and POPIA privacy controls without incurring unsustainable operational, networking, and deployment overhead.

## Considered Alternatives
1. Distributed Microservices Architecture
2. Traditional 3-Tier Layered Monolith
3. Modular Monolith Architecture (Clean / Hexagonal Style)

## Decision Outcome
Chosen Option: **Modular Monolith Architecture**.

### Justification against CivicConnect Drivers:
1. **Latency Guarantee (NFR-001):** Inter-module communication occurs in-memory via direct method calls instead of HTTP/JSON network calls, eliminating serialization latency and guaranteeing sub-3.0s response times.
2. **Domain Integrity & State Control (ASR-02, AC-010.2):** State transitions are encapsulated within the core domain layer, preventing UI controllers from executing unconstrained database status updates.
3. **Zero-Budget & Local Co-Development (ASR-03):** The application runs within a single process and packages into a single container, eliminating multi-service cloud hosting costs and networking configuration complexity.
4. **Automated Testability (NFR-007):** Application commands and domain logic depend on abstract interfaces, allowing 100% of business rules to be verified via rapid automated unit tests without requiring active database connections.

## Consequences and Trade-Offs
* **Positive:** Fast build times; straightforward single-container deployment; guaranteed transactional data consistency (ACID) across ticket updates; strong maintainability.
* **Negative / Trade-off:** Developers must exercise rigorous code discipline during pull request reviews to prevent direct cross-module table coupling. Direct database queries between modules are strictly forbidden; modules must communicate through public application interfaces.

## References
* Bass, L., Clements, P., & Kazman, R. (2021). *Software architecture in practice* (4th ed.). Addison-Wesley Professional.
* City of Boston. (2018). *BOS:311 Performance and platform compliance assessment report*. Department of Innovation and Technology.
* ISO/IEC/IEEE. (2018). *Systems and software engineering — Life cycle processes — Requirements engineering* (ISO/IEC/IEEE Standard No. 29148:2018). IEEE.
* King, M., & Brown, P. (2014). *FixMyStreet: Open data and civic engagement in municipal defect reporting*. Journal of Urban Technology, 21(2), 71-89.
* National Audit Office (NAO). (2020). *Challenges in implementing digital change*. Comptroller and Auditor General, HC 649.
* Nielsen, J. (1993). *Usability engineering*. Morgan Kaufmann.
* Republic of South Africa. (2013). *Protection of Personal Information Act 4 of 2013*. Government Gazette, 581(37067).
* Sommerville, I. (2016). *Software engineering* (10th ed.). Pearson.