using ShowMyLocation.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowMyLocation.Services
{
    public interface ILocationService
    {
        void GetMyLocation();

        double GetDistanceTravelled(double latitude, double longitude);

        event EventHandler<ILocationCoordinates> MyLocation;
    }

    public interface ILocationCoordinates
    {
        double Latitude { get; set; }
        double Longitude { get; set; }
    }
}