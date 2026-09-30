using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class StatusHistory
    {
        public Guid StatusHistoryId { get; set; }

        public Guid ServiceRequestId { get; set; }

        public Guid StatusId { get; set; }

        public Guid ChangedByUserId { get; set; }

        public DateTime ChangedAt { get; set; }

        public ServiceRequest ServiceRequest { get; set; } = null!;

        public RequestStatus Status { get; set; } = null!;

        public User ChangedByUser { get; set; } = null!;
    }
}
