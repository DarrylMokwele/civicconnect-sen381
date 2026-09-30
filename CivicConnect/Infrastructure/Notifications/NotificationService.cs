using Application.Abstractions;
using Domain.Entities;
using Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly CivicConnectDbContext _dbContext;

        public NotificationService(
            CivicConnectDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateNotificationAsync(
            Guid userId,
            Guid serviceRequestId,
            string notificationType,
            string message,
            CancellationToken cancellationToken = default)
        {
            var notification = new Notification
            {
                NotificationId = Guid.NewGuid(),

                UserId = userId,

                ServiceRequestId = serviceRequestId,

                NotificationType = notificationType,

                Message = message,

                CreatedAt = DateTime.UtcNow,

                SentAt = null,

                Status = "Pending"
            };

            _dbContext.Notifications.Add(notification);

            await _dbContext.SaveChangesAsync(
                cancellationToken);
        }
    }
}
