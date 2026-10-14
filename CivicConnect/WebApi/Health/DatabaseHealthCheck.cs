using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebApi.Health;

/// <summary>
/// Verifies that the API can reach its configured relational database.
/// This is intentionally separate from the liveness check: an application
/// process can be alive while its database dependency is unavailable.
/// </summary>
public sealed class DatabaseHealthCheck : IHealthCheck
{
    private readonly CivicConnectDbContext _dbContext;

    public DatabaseHealthCheck(CivicConnectDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var canConnect = await _dbContext.Database
                .CanConnectAsync(cancellationToken);

            return canConnect
                ? HealthCheckResult.Healthy("Database connection is available.")
                : HealthCheckResult.Unhealthy("Database connection is unavailable.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(
                "Database health check failed.",
                ex);
        }
    }
}
