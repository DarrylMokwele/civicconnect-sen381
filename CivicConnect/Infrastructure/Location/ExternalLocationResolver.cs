using Application.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Location
{
    public class ExternalLocationResolver : ILocationResolver
    {
        public Task<LocationResult?> ResolveAsync(
            string address,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                return Task.FromResult<LocationResult?>(null);
            }

            // M2 architecture baseline:
            // External geocoding provider integration will be
            // implemented behind this adapter in a later milestone.
            //
            // No provider-specific API is called here yet.

            return Task.FromResult<LocationResult?>(null);
        }
    }
}
