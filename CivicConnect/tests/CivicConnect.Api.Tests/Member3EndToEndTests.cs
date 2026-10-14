using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CivicConnect.Api.Tests;

/// <summary>
/// Member 3 release-candidate E2E coverage. Unlike the earlier simulation test,
/// these tests exercise the real HTTP pipeline, application services, EF Core,
/// relational SQLite persistence, status-history creation and notification creation.
/// </summary>
public sealed class Member3EndToEndTests
{
    [Fact]
    public async Task CitizenSubmission_AndFullLifecycle_PersistsExpectedStateAndNotifications()
    {
        using var host = new ApiHost();
        using var client = await host.SeedAsync();

        var create = await client.PostAsJsonAsync("/api/ServiceRequests", new
        {
            requesterId = host.ActorId,
            title = "M3 E2E release test",
            description = "End-to-end release candidate verification"
        });

        Assert.Equal(HttpStatusCode.Created, create.StatusCode);
        using var createJson = JsonDocument.Parse(await create.Content.ReadAsStringAsync());
        var requestId = createJson.RootElement.GetProperty("serviceRequestId").GetGuid();

        await ChangeAsync(client, requestId, host.AssignedId, host.ActorId);
        await ChangeAsync(client, requestId, host.InProgressId, host.ActorId);
        await ChangeAsync(client, requestId, host.ResolvedId, host.ActorId);
        await ChangeAsync(client, requestId, host.ClosedId, host.ActorId);

        using var scope = host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CivicConnectDbContext>();
        var request = await db.ServiceRequests.SingleAsync(r => r.ServiceRequestId == requestId);

        Assert.Equal(host.ClosedId, request.StatusId);
        Assert.Equal(4, await db.StatusHistories.CountAsync(h => h.ServiceRequestId == requestId));
        Assert.Equal(4, await db.Notifications.CountAsync(n => n.ServiceRequestId == requestId));

        var history = await db.StatusHistories
            .Where(h => h.ServiceRequestId == requestId)
            .OrderBy(h => h.ChangedAt)
            .ToListAsync();

        Assert.Equal(
            new[] { host.AssignedId, host.InProgressId, host.ResolvedId, host.ClosedId },
            history.Select(h => h.StatusId).ToArray());
        Assert.All(history, h => Assert.Equal(host.ActorId, h.ChangedByUserId));
    }

    [Fact]
    public async Task InvalidLifecycleTransition_ReturnsBadRequest_AndDoesNotCreateAuditOrNotification()
    {
        using var host = new ApiHost();
        using var client = await host.SeedAsync();

        var response = await client.PatchAsJsonAsync(
            $"/api/ServiceRequests/{host.RequestId}/status",
            new { newStatusId = host.ClosedId, changedByUserId = host.ActorId });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        using var scope = host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CivicConnectDbContext>();
        var request = await db.ServiceRequests.SingleAsync(r => r.ServiceRequestId == host.RequestId);

        Assert.Equal(host.SubmittedId, request.StatusId);
        Assert.Equal(0, await db.StatusHistories.CountAsync(h => h.ServiceRequestId == host.RequestId));
        Assert.Equal(0, await db.Notifications.CountAsync(n => n.ServiceRequestId == host.RequestId));
    }

    private static async Task ChangeAsync(HttpClient client, Guid requestId, Guid statusId, Guid actorId)
    {
        using var response = await client.PatchAsJsonAsync(
            $"/api/ServiceRequests/{requestId}/status",
            new { newStatusId = statusId, changedByUserId = actorId });

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}
