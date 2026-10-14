# CivicConnect Observability Baseline

## Signals

### Logs

The API records request method, route, HTTP status, duration and correlation ID. This supports tracing an individual request without logging request bodies or authentication headers.

### Metrics

The M3 baseline uses ASP.NET Core/.NET runtime diagnostics plus request duration in application logs. The team should use these signals to observe latency, errors and resource pressure during the performance exercise.

### Health

- Liveness: `/health/live`
- Readiness: `/health/ready`

## Actionable interpretation

Telemetry is useful only when it changes an operational decision. The following initial interpretation model is recommended for the release candidate:

| Signal | Warning interpretation | Action |
|---|---|---|
| Readiness unhealthy | Database dependency unavailable | Stop/hold traffic and inspect database connectivity |
| HTTP 5xx increases | Application or dependency failure | Inspect correlation IDs and recent deployment changes |
| Request latency approaches NFR target | Capacity/dependency degradation | Run performance checks and inspect database/API bottlenecks |
| Repeated startup failures | Configuration/migration problem | Compare environment configuration and migration state |
| High alert volume with few incidents | Noisy threshold | Tune the alert condition rather than escalating every event |

These are operational starting points, not claims that production thresholds have already been validated.
