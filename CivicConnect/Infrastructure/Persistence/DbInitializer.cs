using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public static class DbInitializer
{
    public static readonly Guid CitizenRoleId =
        Guid.Parse("11111111-1111-1111-1111-111111111111");

    public static readonly Guid AdminRoleId =
        Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

    public static readonly Guid StaffRoleId =
        Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

    public static readonly Guid TestUserId =
        Guid.Parse("22222222-2222-2222-2222-222222222222");

    public static readonly Guid AdminUserId =
        Guid.Parse("aaaaaaaa-1111-1111-1111-aaaaaaaaaaaa");

    public static readonly Guid StaffUserId =
        Guid.Parse("bbbbbbbb-2222-2222-2222-bbbbbbbbbbbb");

    public static readonly Guid SubmittedStatusId =
        Guid.Parse("33333333-3333-3333-3333-333333333333");

    public static readonly Guid AssignedStatusId =
        Guid.Parse("44444444-4444-4444-4444-444444444444");

    public static readonly Guid InProgressStatusId =
        Guid.Parse("55555555-5555-5555-5555-555555555555");

    public static readonly Guid ResolvedStatusId =
        Guid.Parse("66666666-6666-6666-6666-666666666666");

    public static readonly Guid ClosedStatusId =
        Guid.Parse("77777777-7777-7777-7777-777777777777");

    public static readonly Guid TestServiceRequestId =
        Guid.Parse("88888888-8888-8888-8888-888888888888");

    public static async Task SeedAsync(CivicConnectDbContext context)
    {
        if (!await context.Roles.AnyAsync(r => r.RoleId == CitizenRoleId))
        {
            context.Roles.Add(new Role
            {
                RoleId = CitizenRoleId,
                RoleName = "Citizen",
                Description = "Standard CivicConnect requester role."
            });
        }

        if (!await context.Roles.AnyAsync(r => r.RoleId == AdminRoleId))
        {
            context.Roles.Add(new Role
            {
                RoleId = AdminRoleId,
                RoleName = "Admin",
                Description = "CivicConnect service administrator."
            });
        }

        if (!await context.Users.AnyAsync(u => u.UserId == TestUserId))
        {
            context.Users.Add(new User
            {
                UserId = TestUserId,
                FirstName = "Test",
                LastName = "Citizen",
                Email = "test.citizen@civicconnect.local",
                RoleId = CitizenRoleId,
                CreatedAt = DateTime.UtcNow
            });
        }

        if (!await context.Roles.AnyAsync(r => r.RoleId == StaffRoleId))
        {
            context.Roles.Add(new Role
            {
                RoleId = StaffRoleId,
                RoleName = "Staff",
                Description = "CivicConnect service staff member."
            });
        }

        if (!await context.Users.AnyAsync(u => u.UserId == StaffUserId))
        {
            context.Users.Add(new User
            {
                UserId = StaffUserId,
                FirstName = "Service",
                LastName = "Staff",
                Email = "staff@civicconnect.local",
                RoleId = StaffRoleId,
                CreatedAt = DateTime.UtcNow
            });
        }

        if (!await context.Users.AnyAsync(u => u.UserId == AdminUserId))
        {
            context.Users.Add(new User
            {
                UserId = AdminUserId,
                FirstName = "CivicConnect",
                LastName = "Administrator",
                Email = "admin@civicconnect.local",
                RoleId = AdminRoleId,
                CreatedAt = DateTime.UtcNow
            });
        }

        await EnsureStatusAsync(context, SubmittedStatusId, "Submitted", "New request awaiting assignment.");
        await EnsureStatusAsync(context, AssignedStatusId, "Assigned", "Request assigned to a service team.");
        await EnsureStatusAsync(context, InProgressStatusId, "In Progress", "Service team is working on the request.");
        await EnsureStatusAsync(context, ResolvedStatusId, "Resolved", "Service team has resolved the request.");
        await EnsureStatusAsync(context, ClosedStatusId, "Closed", "Request has been completed and closed.");

        if (!await context.ServiceRequests.AnyAsync(r => r.ServiceRequestId == TestServiceRequestId))
        {
            context.ServiceRequests.Add(new ServiceRequest
            {
                ServiceRequestId = TestServiceRequestId,
                RequesterId = TestUserId,
                StatusId = SubmittedStatusId,
                Title = "Test Service Request",
                Description = "Development request used to test CivicConnect lifecycle governance.",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        await context.SaveChangesAsync();
    }

    private static async Task EnsureStatusAsync(
        CivicConnectDbContext context,
        Guid id,
        string name,
        string description)
    {
        if (!await context.RequestStatuses.AnyAsync(s => s.StatusId == id))
        {
            context.RequestStatuses.Add(new RequestStatus
            {
                StatusId = id,
                StatusName = name,
                Description = description,
                IsActive = true
            });
        }
    }
}
