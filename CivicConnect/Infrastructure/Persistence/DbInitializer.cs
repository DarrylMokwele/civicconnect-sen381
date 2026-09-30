using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public static class DbInitializer
    {
        public static readonly Guid CitizenRoleId =
            Guid.Parse("11111111-1111-1111-1111-111111111111");

        public static readonly Guid TestUserId =
            Guid.Parse("22222222-2222-2222-2222-222222222222");

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

        public static async Task SeedAsync(
            CivicConnectDbContext context)
        {
            if (!await context.Roles.AnyAsync())
            {
                context.Roles.Add(new Role
                {
                    RoleId = CitizenRoleId,
                    RoleName = "Citizen"
                });
            }

            if (!await context.Users.AnyAsync())
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

            if (!await context.RequestStatuses.AnyAsync())
            {
                context.RequestStatuses.AddRange(
                    new RequestStatus
                    {
                        StatusId = SubmittedStatusId,
                        StatusName = "Submitted",
                        IsActive = true
                    },
                    new RequestStatus
                    {
                        StatusId = AssignedStatusId,
                        StatusName = "Assigned",
                        IsActive = true
                    },
                    new RequestStatus
                    {
                        StatusId = InProgressStatusId,
                        StatusName = "In Progress",
                        IsActive = true
                    },
                    new RequestStatus
                    {
                        StatusId = ResolvedStatusId,
                        StatusName = "Resolved",
                        IsActive = true
                    },
                    new RequestStatus
                    {
                        StatusId = ClosedStatusId,
                        StatusName = "Closed",
                        IsActive = true
                    });
            }

            if (!await context.ServiceRequests.AnyAsync())
            {
                context.ServiceRequests.Add(
                    new ServiceRequest
                    {
                        ServiceRequestId = TestServiceRequestId,
                        RequesterId = TestUserId,
                        StatusId = SubmittedStatusId,

                        Title = "Test Service Request",

                        Description =
                            "Development request used to test " +
                            "CivicConnect lifecycle governance.",

                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
            }

            await context.SaveChangesAsync();
        }
    }
}
