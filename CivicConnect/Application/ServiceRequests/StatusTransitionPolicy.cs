using Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceRequests
{
    public class StatusTransitionPolicy : IStatusTransitionPolicy
    {
        private readonly Dictionary<string, HashSet<string>>
            _allowedTransitions =
                new(StringComparer.OrdinalIgnoreCase)
                {
                    ["Submitted"] = new()
                    {
                    "Assigned"
                    },

                    ["Assigned"] = new()
                    {
                    "In Progress"
                    },

                    ["In Progress"] = new()
                    {
                    "Resolved"
                    },

                    ["Resolved"] = new()
                    {
                    "Closed"
                    }
                };

        public bool CanTransition(
            string currentStatus,
            string newStatus)
        {
            if (!_allowedTransitions.TryGetValue(
                    currentStatus,
                    out var allowedStatuses))
            {
                return false;
            }

            return allowedStatuses.Contains(newStatus);
        }
    }
}
