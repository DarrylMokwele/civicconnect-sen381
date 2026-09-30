using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IServiceRequestLifecycleService
    {
        Task ChangeStatusAsync(
            Guid serviceRequestId,
            Guid newStatusId,
            Guid changedByUserId,
            CancellationToken cancellationToken = default);
    }
}
