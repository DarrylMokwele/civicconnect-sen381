using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events
{
    public class ServiceRequestStatusChangedEvent
    {
        public Guid ServiceRequestId { get; }

        public Guid UserId { get; }

        public Guid NewStatusId { get; }

        public DateTime ChangedAt { get; }

        public ServiceRequestStatusChangedEvent(
            Guid serviceRequestId,
            Guid userId,
            Guid newStatusId,
            DateTime changedAt)
        {
            ServiceRequestId = serviceRequestId;
            UserId = userId;
            NewStatusId = newStatusId;
            ChangedAt = changedAt;
        }
    }
}
