using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ServiceRequest
    {
        public Guid ServiceRequestId { get; set; }

        public Guid RequesterId { get; set; }

        public Guid StatusId { get; set; }

        public Guid? DepartmentId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public User Requester { get; set; } = null!;

        public RequestStatus Status { get; set; } = null!;

        public Department? Department { get; set; }

        public ICollection<StatusHistory> StatusHistory { get; set; }
            = new List<StatusHistory>();

        public ICollection<Assignment> Assignments { get; set; }
            = new List<Assignment>();

        public ICollection<Notification> Notifications { get; set; }
            = new List<Notification>();
    }
}
