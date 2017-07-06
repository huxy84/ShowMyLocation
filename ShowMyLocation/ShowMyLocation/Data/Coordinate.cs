using ShowMyLocation.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowMyLocation.Data
{
    public class Coordinate : EventArgs, ILocationCoordinates
    {
        public DateTime Timestamp { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
