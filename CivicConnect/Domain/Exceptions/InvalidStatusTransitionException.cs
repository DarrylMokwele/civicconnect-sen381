using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class InvalidStatusTransitionException : Exception
    {
        public InvalidStatusTransitionException(
            string currentStatus,
            string requestedStatus)
            : base(
                $"Transition from '{currentStatus}' " +
                $"to '{requestedStatus}' is not allowed.")
        {
        }
    }
}
