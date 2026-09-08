# Scope Baseline

## 1. In-Scope (M1 Baseline Commitment)
* Citizen incident logging interface (category selection, description, geolocation capture).
* Image/photo attachment handling for civic issue verification.
* Municipal administrative issue dashboard (filtering, priority assignment, state transitions: Open -> In-Progress -> Resolved).
* Audit logging of ticket updates and status transitions.
* Core authentication and role-based access control (Citizen vs. Municipal Official).

## 2. Deferred Scope (Future Milestones)
* Automated department routing via machine learning categorization (deferred to M2/M3 architecture evaluation).
* Public community upvoting and commenting on nearby logged issues (deferred to post-M1 based on core tracking stability).
* Push notification/SMS alert service for ticket status changes (deferred pending external messaging API evaluation).

## 3. Deliberate Scope Exclusion & Engineering Defence
* **Excluded Feature:** Real-Time Payment / Fine Processing Gateway.
* **Engineering Defence:** CivicConnect is strictly an incident reporting and municipal resolution tracking system. Introducing financial transactions introduces PCI-DSS compliance requirements, financial audit liabilities, and banking API dependencies that offer zero functional value to service delivery resolution while consuming critical schedule and development capacity.