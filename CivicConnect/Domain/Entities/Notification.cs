using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Notification
    {
        public Guid NotificationId { get; set; }

        public Guid UserId { get; set; }

        public Guid ServiceRequestId { get; set; }

        public string NotificationType { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime? SentAt { get; set; }

        public string Status { get; set; } = string.Empty;

        public User User { get; set; } = null!;

        public ServiceRequest ServiceRequest { get; set; } = null!;
    }
}
