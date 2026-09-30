using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Department
    {
        public Guid DepartmentId { get; set; }

        public string DepartmentName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public ICollection<ServiceRequest> ServiceRequests { get; set; }
            = new List<ServiceRequest>();

        public ICollection<Assignment> Assignments { get; set; }
            = new List<Assignment>();
    }
}
