using Application.Abstractions;
using Application.Notifications;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivicConnect.Application.Tests
{
    public class ServiceRequestStatusChangedHandlerTests
    {
        [Fact]
        public async Task HandleAsync_CreatesNotification()
        {
            // Arrange
            var fakeNotificationService =
                new FakeNotificationService();

            var handler =
                new ServiceRequestStatusChangedHandler(
                    fakeNotificationService);

            var serviceRequestId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var statusId = Guid.NewGuid();

            var domainEvent =
                new ServiceRequestStatusChangedEvent(
                    serviceRequestId,
                    userId,
                    statusId,
                    DateTime.UtcNow);

            // Act
            await handler.HandleAsync(domainEvent);

            // Assert
            Assert.True(
                fakeNotificationService.WasCalled);

            Assert.Equal(
                userId,
                fakeNotificationService.UserId);

            Assert.Equal(
                serviceRequestId,
                fakeNotificationService.ServiceRequestId);

            Assert.Equal(
                "StatusChanged",
                fakeNotificationService.NotificationType);
        }

        private class FakeNotificationService
            : INotificationService
        {
            public bool WasCalled { get; private set; }

            public Guid UserId { get; private set; }

            public Guid ServiceRequestId { get; private set; }

            public string NotificationType { get; private set; }
                = string.Empty;

            public Task CreateNotificationAsync(
                Guid userId,
                Guid serviceRequestId,
                string notificationType,
                string message,
                CancellationToken cancellationToken = default)
            {
                WasCalled = true;

                UserId = userId;

                ServiceRequestId = serviceRequestId;

                NotificationType = notificationType;

                return Task.CompletedTask;
            }
        }
    }
}
