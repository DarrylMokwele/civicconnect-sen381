using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public Guid UserId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public Guid RoleId { get; set; }

        public DateTime CreatedAt { get; set; }

        public Role Role { get; set; } = null!;

        public ICollection<ServiceRequest> ServiceRequests { get; set; }
            = new List<ServiceRequest>();

        public ICollection<StatusHistory> StatusChanges { get; set; }
            = new List<StatusHistory>();

        public ICollection<Assignment> Assignments { get; set; }
            = new List<Assignment>();

        public ICollection<Notification> Notifications { get; set; }
            = new List<Notification>();
    }
}
