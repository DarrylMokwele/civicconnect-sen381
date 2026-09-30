using Domain.Entities;
using Infrastructure.Notifications;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivicConnect.Infrastructure.Tests
{
    public class NotificationServiceTests
    {
        [Fact]
        public async Task CreateNotificationAsync_SavesNotification()
        {
            // Arrange
            var options =
                new DbContextOptionsBuilder<CivicConnectDbContext>()
                    .UseInMemoryDatabase(
                        Guid.NewGuid().ToString())
                    .Options;

            await using var context =
                new CivicConnectDbContext(options);

            var role = new Role
            {
                RoleId = Guid.NewGuid(),
                RoleName = "Test Role"
            };

            var user = new User
            {
                UserId = Guid.NewGuid(),
                FirstName = "Test",
                LastName = "User",
                Email = $"{Guid.NewGuid()}@test.com",
                RoleId = role.RoleId,
                Role = role,
                CreatedAt = DateTime.UtcNow
            };

            var status = new RequestStatus
            {
                StatusId = Guid.NewGuid(),
                StatusName = "Test Status",
                IsActive = true
            };

            var request = new ServiceRequest
            {
                ServiceRequestId = Guid.NewGuid(),
                RequesterId = user.UserId,
                Requester = user,
                StatusId = status.StatusId,
                Status = status,
                Title = "Test Request",
                Description = "Notification test",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.ServiceRequests.Add(request);

            await context.SaveChangesAsync();

            var service =
                new NotificationService(context);

            // Act
            await service.CreateNotificationAsync(
                user.UserId,
                request.ServiceRequestId,
                "StatusChanged",
                "Your request status changed.");

            // Assert
            var notification =
                await context.Notifications.SingleAsync();

            Assert.Equal(
                user.UserId,
                notification.UserId);

            Assert.Equal(
                request.ServiceRequestId,
                notification.ServiceRequestId);

            Assert.Equal(
                "StatusChanged",
                notification.NotificationType);

            Assert.Equal(
                "Pending",
                notification.Status);

            Assert.Null(
                notification.SentAt);
        }
    }
}
