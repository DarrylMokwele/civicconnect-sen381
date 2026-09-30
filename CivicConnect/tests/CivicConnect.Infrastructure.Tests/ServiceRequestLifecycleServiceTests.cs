using Application.Notifications;
using Application.ServiceRequests;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.Notifications;
using Infrastructure.Persistence;
using Infrastructure.ServiceRequests;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivicConnect.Infrastructure.Tests
{
    public class ServiceRequestLifecycleServiceTests
    {
        [Fact]
        public async Task ChangeStatusAsync_SubmittedToClosed_ThrowsException()
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

            var submitted = new RequestStatus
            {
                StatusId = Guid.NewGuid(),
                StatusName = "Submitted",
                IsActive = true
            };

            var closed = new RequestStatus
            {
                StatusId = Guid.NewGuid(),
                StatusName = "Closed",
                IsActive = true
            };

            var request = new ServiceRequest
            {
                ServiceRequestId = Guid.NewGuid(),
                RequesterId = user.UserId,
                Requester = user,

                StatusId = submitted.StatusId,
                Status = submitted,

                Title = "Test Request",
                Description = "Lifecycle test",

                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.ServiceRequests.Add(request);
            context.RequestStatuses.Add(closed);

            await context.SaveChangesAsync();

            var notificationService =
                new NotificationService(context);

            var notificationHandler =
                new ServiceRequestStatusChangedHandler(
                    notificationService);

            var transitionPolicy =
                new StatusTransitionPolicy();

            var lifecycleService =
                new ServiceRequestLifecycleService(
                    context,
                    transitionPolicy,
                    notificationHandler);

            // Act + Assert
            await Assert.ThrowsAsync<
                InvalidStatusTransitionException>(
                    () =>
                        lifecycleService.ChangeStatusAsync(
                            request.ServiceRequestId,
                            closed.StatusId,
                            user.UserId));
        }

        [Fact]
        public async Task ChangeStatusAsync_SubmittedToAssigned_UpdatesRequest()
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

            var submitted = new RequestStatus
            {
                StatusId = Guid.NewGuid(),
                StatusName = "Submitted",
                IsActive = true
            };

            var assigned = new RequestStatus
            {
                StatusId = Guid.NewGuid(),
                StatusName = "Assigned",
                IsActive = true
            };

            var request = new ServiceRequest
            {
                ServiceRequestId = Guid.NewGuid(),

                RequesterId = user.UserId,
                Requester = user,

                StatusId = submitted.StatusId,
                Status = submitted,

                Title = "Test Request",
                Description = "Valid lifecycle test",

                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.ServiceRequests.Add(request);
            context.RequestStatuses.Add(assigned);

            await context.SaveChangesAsync();

            var notificationService =
                new NotificationService(context);

            var notificationHandler =
                new ServiceRequestStatusChangedHandler(
                    notificationService);

            var transitionPolicy =
                new StatusTransitionPolicy();

            var lifecycleService =
                new ServiceRequestLifecycleService(
                    context,
                    transitionPolicy,
                    notificationHandler);

            // Act
            await lifecycleService.ChangeStatusAsync(
                request.ServiceRequestId,
                assigned.StatusId,
                user.UserId);

            // Assert
            var updatedRequest =
                await context.ServiceRequests
                    .SingleAsync(
                        r => r.ServiceRequestId ==
                             request.ServiceRequestId);

            Assert.Equal(
                assigned.StatusId,
                updatedRequest.StatusId);

            var history =
                await context.StatusHistories.SingleAsync();

            Assert.Equal(
                assigned.StatusId,
                history.StatusId);

            Assert.Equal(
                user.UserId,
                history.ChangedByUserId);

            var notification =
                await context.Notifications.SingleAsync();

            Assert.Equal(
                request.ServiceRequestId,
                notification.ServiceRequestId);

            Assert.Equal(
                "StatusChanged",
                notification.NotificationType);
        }
    }
}
