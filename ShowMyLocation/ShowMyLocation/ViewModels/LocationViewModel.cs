using ShowMyLocation.Data;
using ShowMyLocation.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ShowMyLocation.ViewModels
{
    public class LocationViewModel : INotifyPropertyChanged
    {
        ILocationService locationService;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;

            if (handler == null)
                return;

            handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private Command getLocationCommand;
        public Command GetLocationCommand
        {
            get
            {
                return getLocationCommand ?? (getLocationCommand = new Command(GetLocation));
            }
        }

        private void GetLocation()
        {
            locationService.GetMyLocation();
        }

        private double distanceTravelled;
        public double DistanceTravelled
        {
            get { return distanceTravelled; }

            set
            {
                distanceTravelled = value;

                OnPropertyChanged();
            }
        }

        private double latitude;
        public double Latitude
        {
            get { return latitude; }
            set
            {
                latitude = value;

                OnPropertyChanged();
            }
        }

        private double longitude;
        public double Longitude
        {
            get { return longitude; }
            set
            {
                longitude = value;

                OnPropertyChanged();
            }
        }

        private DateTime timestamp;
        public DateTime Timestamp
        {
            get { return timestamp; }
            set
            {
                timestamp = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Coordinate> coordinates;
        public ObservableCollection<Coordinate> Coordinates
        {
            get { return coordinates; }
            set
            {
                coordinates = value;

                OnPropertyChanged();
            }
        }

        public LocationViewModel()
        {
            if (Coordinates == null)
                Coordinates = new ObservableCollection<Coordinate>();

            //Coordinates.Add(new Coordinate { Latitude = 0, Longitude = 0 });

            locationService = DependencyService.Get<ILocationService>();

            if (locationService == null)
                return;

            locationService.MyLocation += LocationService_MyLocation;

            locationService.GetMyLocation();
        }

        private void LocationService_MyLocation(object sender, ILocationCoordinates e)
        {
            Latitude = e.Latitude;
            Longitude = e.Longitude;
            Timestamp = DateTime.Now;
            //Coordinates = locationService.GetAllCoordinates();
            Coordinates.Add(new Coordinate { Timestamp = Timestamp, Latitude = Latitude, Longitude = Longitude });
            //Coordinates.Add(new Coordinate { Latitude = e.Latitude, Longitude = e.Longitude });
            DistanceTravelled = locationService.GetDistanceTravelled(e.Latitude, e.Longitude);
        }
    }
}