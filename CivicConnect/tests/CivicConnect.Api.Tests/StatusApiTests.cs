using System.Net;
using System.Net.Http.Json;
using System.Text;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CivicConnect.Api.Tests;

// Each case owns a real relational SQLite database and real application services.
// SQLite verifies persistence/FKs, but does not establish SQL Server equivalence.
public sealed class ApiHost : WebApplicationFactory<global::Program>
{
    private readonly SqliteConnection connection = new("Data Source=:memory:");
    public Guid RequestId { get; } = Guid.NewGuid();
    public Guid ActorId { get; } = Guid.NewGuid();
    public Guid SubmittedId { get; } = Guid.NewGuid();
    public Guid AssignedId { get; } = Guid.NewGuid();
    public Guid ClosedId { get; } = Guid.NewGuid();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        connection.Open();
        builder.UseEnvironment("Testing"); // Avoid development SQL Server seeding.
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<CivicConnectDbContext>();
            services.RemoveAll<DbContextOptions<CivicConnectDbContext>>();
            // Remove SQL Server options configuration before adding SQLite.
            foreach (var item in services.Where(d => d.ServiceType.IsGenericType &&
                d.ServiceType.GetGenericTypeDefinition().FullName ==
                "Microsoft.EntityFrameworkCore.Infrastructure.IDbContextOptionsConfiguration`1" &&
                d.ServiceType.GenericTypeArguments[0] == typeof(CivicConnectDbContext)).ToList())
                services.Remove(item);
            services.AddDbContext<CivicConnectDbContext>(o => o.UseSqlite(connection));
        });
    }

    public async Task<HttpClient> SeedAsync()
    {
        var client = CreateClient(new WebApplicationFactoryClientOptions { BaseAddress = new Uri("https://localhost") });
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CivicConnectDbContext>();
        await db.Database.EnsureCreatedAsync();
        var role = new Role { RoleId = Guid.NewGuid(), RoleName = "Staff" };
        var user = new User { UserId = ActorId, FirstName = "API", LastName = "Test", Email = "api@example.test", Role = role, RoleId = role.RoleId, CreatedAt = DateTime.UtcNow };
        var submitted = new RequestStatus { StatusId = SubmittedId, StatusName = "Submitted", IsActive = true };
        db.RequestStatuses.AddRange(submitted,
            new RequestStatus { StatusId = AssignedId, StatusName = "Assigned", IsActive = true },
            new RequestStatus { StatusId = ClosedId, StatusName = "Closed", IsActive = true });
        db.ServiceRequests.Add(new ServiceRequest { ServiceRequestId = RequestId, Requester = user, RequesterId = ActorId,
            Status = submitted, StatusId = SubmittedId, Title = "Broken light", Description = "Controlled integration fixture",
            CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
        await db.SaveChangesAsync();
        return client;
    }

    public async Task AssertStateAsync(bool changed)
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CivicConnectDbContext>();
        var request = await db.ServiceRequests.SingleAsync();
        Assert.Equal(changed ? AssignedId : SubmittedId, request.StatusId);
        Assert.Equal(changed ? 1 : 0, await db.StatusHistories.CountAsync());
        Assert.Equal(changed ? 1 : 0, await db.Notifications.CountAsync());
        if (changed)
        {
            var history = await db.StatusHistories.SingleAsync();
            Assert.Equal(ActorId, history.ChangedByUserId);
            Assert.Equal(RequestId, history.ServiceRequestId);
            Assert.Equal(AssignedId, history.StatusId);
            Assert.True(history.ChangedAt > DateTime.UtcNow.AddMinutes(-2));
            var notification = await db.Notifications.SingleAsync();
            Assert.Equal(ActorId, notification.UserId);
            Assert.Equal(RequestId, notification.ServiceRequestId);
            Assert.Equal("StatusChanged", notification.NotificationType);
        }
    }
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing) connection.Dispose();
    }
}

public class StatusApiTests
{
    [Fact] // API-01: FR-010 AC-010.3, NFR-005, partial FR-005.
    public async Task ValidTransition_PersistsStatusHistoryAndNotification()
    {
        using var host = new ApiHost(); using var client = await host.SeedAsync();
        using var response = await client.PatchAsJsonAsync($"/api/ServiceRequests/{host.RequestId}/status", new { newStatusId = host.AssignedId, changedByUserId = host.ActorId });
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Equal("", await response.Content.ReadAsStringAsync());
        await host.AssertStateAsync(true);
    }
    [Fact] // API-02: FR-010 AC-010.2, state-transition technique.
    public async Task InvalidTransition_Returns400WithoutSideEffects()
    {
        using var host = new ApiHost(); using var client = await host.SeedAsync();
        using var response = await client.PatchAsJsonAsync($"/api/ServiceRequests/{host.RequestId}/status", new { newStatusId = host.ClosedId, changedByUserId = host.ActorId });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("error", await response.Content.ReadAsStringAsync());
        await host.AssertStateAsync(false);
    }
    [Fact] // API-03: API contract, unknown resource partition.
    public async Task MissingRequest_Returns404WithoutSideEffects()
    {
        using var host = new ApiHost(); using var client = await host.SeedAsync();
        using var response = await client.PatchAsJsonAsync($"/api/ServiceRequests/{Guid.NewGuid()}/status", new { newStatusId = host.AssignedId, changedByUserId = host.ActorId });
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        await host.AssertStateAsync(false);
    }
    [Fact] // API-04: unknown target status partition.
    public async Task MissingStatus_Returns404WithoutSideEffects()
    {
        using var host = new ApiHost(); using var client = await host.SeedAsync();
        using var response = await client.PatchAsJsonAsync($"/api/ServiceRequests/{host.RequestId}/status", new { newStatusId = Guid.NewGuid(), changedByUserId = host.ActorId });
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        await host.AssertStateAsync(false);
    }
    [Theory] // SEC-01/02: missing/default IDs, invalid input partitions.
    [InlineData("{}")]
    [InlineData("{\"newStatusId\":\"00000000-0000-0000-0000-000000000000\",\"changedByUserId\":\"00000000-0000-0000-0000-000000000000\"}")]
    [InlineData("{\"newStatusId\":\"not-a-guid\"}")]
    [InlineData("{")]
    public async Task InvalidBody_Returns400WithoutSideEffects(string body)
    {
        using var host = new ApiHost(); using var client = await host.SeedAsync();
        using var response = await client.PatchAsync($"/api/ServiceRequests/{host.RequestId}/status", new StringContent(body, Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        await host.AssertStateAsync(false);
    }
    [Fact] // SEC-03: characterisation of OPEN DEFECT M2-SEC-01, not a security acceptance pass.
    public async Task AnonymousRequest_CurrentlySucceeds_DocumentsAccessControlGap()
    {
        using var host = new ApiHost(); using var client = await host.SeedAsync();
        Assert.Null(client.DefaultRequestHeaders.Authorization);
        using var response = await client.PatchAsJsonAsync($"/api/ServiceRequests/{host.RequestId}/status", new { newStatusId = host.AssignedId, changedByUserId = host.ActorId });
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        await host.AssertStateAsync(true);
    }
}
