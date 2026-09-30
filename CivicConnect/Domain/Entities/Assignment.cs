using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Assignment
    {
        public Guid AssignmentId { get; set; }

        public Guid ServiceRequestId { get; set; }

        public Guid DepartmentId { get; set; }

        public Guid? AssignedUserId { get; set; }

        public DateTime AssignedAt { get; set; }

        public DateTime? UnassignedAt { get; set; }

        public ServiceRequest ServiceRequest { get; set; } = null!;

        public Department Department { get; set; } = null!;

        public User? AssignedUser { get; set; }
    }
}
