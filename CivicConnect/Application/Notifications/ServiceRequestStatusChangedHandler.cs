using Application.Abstractions;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Notifications
{
    public class ServiceRequestStatusChangedHandler
    {
        private readonly INotificationService _notificationService;

        public ServiceRequestStatusChangedHandler(
            INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task HandleAsync(
            ServiceRequestStatusChangedEvent domainEvent,
            CancellationToken cancellationToken = default)
        {
            var message =
                $"The status of service request " +
                $"{domainEvent.ServiceRequestId} has changed.";

            await _notificationService.CreateNotificationAsync(
                domainEvent.UserId,
                domainEvent.ServiceRequestId,
                "StatusChanged",
                message,
                cancellationToken);
        }
    }
}
