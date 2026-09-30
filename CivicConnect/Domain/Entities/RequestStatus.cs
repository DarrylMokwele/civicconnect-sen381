using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class RequestStatus
    {
        public Guid StatusId { get; set; }

        public string StatusName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public ICollection<ServiceRequest> ServiceRequests { get; set; }
            = new List<ServiceRequest>();

        public ICollection<StatusHistory> StatusHistories { get; set; }
            = new List<StatusHistory>();
    }
}
