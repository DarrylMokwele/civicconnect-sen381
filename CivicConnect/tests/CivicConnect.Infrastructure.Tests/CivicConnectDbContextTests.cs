using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivicConnect.Infrastructure.Tests
{
    public class CivicConnectDbContextTests
    {
        [Fact]
        public async Task CanSaveAndRetrieveServiceRequest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CivicConnectDbContext>()
                .UseInMemoryDatabase(
                    databaseName: Guid.NewGuid().ToString())
                .Options;

            await using var context =
                new CivicConnectDbContext(options);

            var role = new Role
            {
                RoleId = Guid.NewGuid(),
                RoleName = "Test Role",
                Description = "Role used for persistence testing"
            };

            var user = new User
            {
                UserId = Guid.NewGuid(),
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                RoleId = role.RoleId,
                Role = role,
                CreatedAt = DateTime.UtcNow
            };

            var status = new RequestStatus
            {
                StatusId = Guid.NewGuid(),
                StatusName = "Test Status",
                Description = "Status used for persistence testing",
                IsActive = true
            };

            var serviceRequest = new ServiceRequest
            {
                ServiceRequestId = Guid.NewGuid(),
                RequesterId = user.UserId,
                Requester = user,
                StatusId = status.StatusId,
                Status = status,
                Title = "Test Service Request",
                Description = "Persistence test",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Act
            context.ServiceRequests.Add(serviceRequest);
            await context.SaveChangesAsync();

            var savedRequest =
                await context.ServiceRequests
                    .Include(r => r.Requester)
                    .Include(r => r.Status)
                    .SingleAsync(
                        r => r.ServiceRequestId ==
                             serviceRequest.ServiceRequestId);

            // Assert
            Assert.NotNull(savedRequest);
            Assert.Equal(
                "Test Service Request",
                savedRequest.Title);

            Assert.Equal(
                user.UserId,
                savedRequest.RequesterId);

            Assert.Equal(
                status.StatusId,
                savedRequest.StatusId);
        }
    }
}
