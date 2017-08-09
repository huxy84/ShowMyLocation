using CoreLocation;
using ShowMyLocation.iOS;
using ShowMyLocation.Services;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(LocationService))]
namespace ShowMyLocation.iOS
{
    public class LocationService : ILocationService
    {
        CLLocationManager locationManager;
        CLLocation newLocation;

        public event EventHandler<ILocationCoordinates> MyLocation;

        public double GetDistanceTravelled(double latitude, double longitude)
        {
            throw new NotImplementedException();
        }

        public void GetMyLocation()
        {
            locationManager = new CLLocationManager();

            // check location services enabled.
            if (!CLLocationManager.LocationServicesEnabled)
                return;

            // Set desired accurracy in meters.
            locationManager.DesiredAccuracy = 1;

            bool iOS8 = UIDevice.CurrentDevice.CheckSystemVersion(8, 0);
            bool iOS9 = UIDevice.CurrentDevice.CheckSystemVersion(9, 0);

            // iOS 8 has additional permission requirements.
            if(iOS8)
            {
                // Perform location changes within the background
                locationManager.RequestAlwaysAuthorization();
            }

            // iOS 9 comes with a new method allowing us to receive
            // location updates within the background, when the app's suspended.
            if (iOS9)
                try
                {
                    locationManager.AllowsBackgroundLocationUpdates = true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

            // CLLocationManagerDelegate Methods

            // Fired whenever there is a change in location.
            locationManager.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) =>
            {
                LocationUpdated(e);
            };
        }

        private void LocationUpdated(CLLocationsUpdatedEventArgs e)
        {
            var coords = new Coordinates();

            // Get the list of found locations.
            var locations = e.Locations;

            // Extract coordinates from locations array.
            int lastCoordIndex = locations.Length - 1;
            var lastLocation = locations[lastCoordIndex];
            coords.Latitude = lastLocation.Coordinate.Latitude;
            coords.Longitude = lastLocation.Coordinate.Longitude;

            // Convert coordinates to CLLocation object.
            newLocation = new CLLocation(coords.Latitude, coords.Longitude);
            MyLocation(this, coords);
        }

        private void DidAuthorisationChange(CLAuthorizationChangedEventArgs e)
        {
            switch (e.Status)
            {
                case CLAuthorizationStatus.AuthorizedAlways:
                    locationManager.RequestAlwaysAuthorization();
                    break;
                case CLAuthorizationStatus.AuthorizedWhenInUse:
                    locationManager.StartUpdatingLocation();
                    break;
                case CLAuthorizationStatus.Denied:
                    UIAlertView alertView = new UIAlertView()
                    {
                        Title = "Location Services Disabled",
                        Message = "Enable locations for this app via\nthe Settings app on your iPhone",
                        AlertViewStyle = UIAlertViewStyle.Default
                    };

                    alertView.AddButton("OK");
                    alertView.AddButton("Cancel");
                    alertView.Show();
                    break;
                default:
                    break;
            }
        }
    }

    public class Coordinates : EventArgs, ILocationCoordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
