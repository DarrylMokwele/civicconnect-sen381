using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceRequests
{
    public class ChangeServiceRequestStatusRequest
    {
        public Guid NewStatusId { get; set; }

        public Guid ChangedByUserId { get; set; }
    }
}
