using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Locations
{
    public class LocationResult
    {
        public double Latitude { get; init; }

        public double Longitude { get; init; }

        public string? FormattedAddress { get; init; }
    }
}
