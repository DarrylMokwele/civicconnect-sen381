using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Locations
{
    public interface ILocationResolver
    {
        Task<LocationResult?> ResolveAsync(
            string address,
            CancellationToken cancellationToken = default);
    }
}
