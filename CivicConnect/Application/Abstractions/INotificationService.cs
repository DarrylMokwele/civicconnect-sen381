using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface INotificationService
    {
        Task CreateNotificationAsync(
            Guid userId,
            Guid serviceRequestId,
            string notificationType,
            string message,
            CancellationToken cancellationToken = default);
    }
}
