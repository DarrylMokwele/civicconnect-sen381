using System.Net;
using System.Net.Http.Headers;

namespace CivicConnect.Api.Tests;

public sealed class HealthAndObservabilityTests
{
    [Fact]
    public async Task LiveHealth_ReturnsOkWithoutDatabaseDependency()
    {
        using var host = new ApiHost();
        using var client = host.CreateClient();

        using var response = await client.GetAsync("/health/live");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("Healthy", await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task ReadyHealth_ReturnsOkWhenDatabaseIsAvailable()
    {
        using var host = new ApiHost();
        using var client = await host.SeedAsync();

        using var response = await client.GetAsync("/health/ready");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("Healthy", await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task RequestLogging_ReturnsCorrelationId()
    {
        using var host = new ApiHost();
        using var client = host.CreateClient();

        using var response = await client.GetAsync("/health/live");

        Assert.True(response.Headers.TryGetValues("X-Correlation-ID", out var values));
        var correlationId = values.Single();
        Assert.True(Guid.TryParse(correlationId, out _));
    }

    [Fact]
    public async Task RequestLogging_PreservesValidCorrelationId()
    {
        using var host = new ApiHost();
        using var client = host.CreateClient();
        var correlationId = Guid.NewGuid().ToString();
        client.DefaultRequestHeaders.Add("X-Correlation-ID", correlationId);

        using var response = await client.GetAsync("/health/live");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(correlationId, response.Headers.GetValues("X-Correlation-ID").Single());
    }
}
