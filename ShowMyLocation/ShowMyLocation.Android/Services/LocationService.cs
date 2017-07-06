using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Locations;
using ShowMyLocation.Services;
using Xamarin.Forms;
using ShowMyLocation.Droid.Services;
using ShowMyLocation.Data;
using System.Collections.ObjectModel;

[assembly: Dependency(typeof(LocationService))]
namespace ShowMyLocation.Droid.Services
{
    public class LocationService : Java.Lang.Object, ILocationService, ILocationListener
    {
        LocationManager locationManager;
        Location newLocation;

        public event EventHandler<ILocationCoordinates> MyLocation;

        public double GetDistanceTravelled(double latitude, double longitude)
        {
            Location lastKnownLocation = new Location("Last Known Location")
            {
                Latitude = latitude,
                Longitude = longitude
            };

            var dist = newLocation.DistanceTo(lastKnownLocation);
            return dist;// (dist / 1000);
        }

        public void GetMyLocation()
        {
            long minTime = 0;
            float minDistance = 2; // meters

            locationManager = (LocationManager)Forms.Context.GetSystemService(Context.LocationService);
            locationManager.RequestLocationUpdates(LocationManager.NetworkProvider, minTime, minDistance, this);
        }

        public void OnLocationChanged(Location location)
        {
            if (location == null)
                return;

            var coord = new Coordinate()
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude
            };

            // Store new location details.
            newLocation = new Location("Previous Point")
            {
                Latitude = coord.Latitude,
                Longitude = coord.Longitude
            };

            MyLocation(this, coord);
        }

        public void OnProviderDisabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            //throw new NotImplementedException();
        }

        // Stop the location update when the object is set to null
        ~LocationService()
        {
            locationManager.RemoveUpdates(this);
        }
    }
}