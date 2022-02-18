using Plugin.Geolocator;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace PM2E10022.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Mapa : ContentPage
    {
        double latitud, longitud;
        string label;
        public Mapa(double lat, double longi, string lab)
        {
            InitializeComponent();

            latitud = lat;
            longitud = longi;
            label = lab;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var location = CrossGeolocator.Current;
            location.DesiredAccuracy = 50;

            if (!location.IsGeolocationEnabled || !location.IsGeolocationAvailable)
            {

                await DisplayAlert("Warning", "Debe Activar el GPS", "Cerrar");
                return;
            }
            if (!location.IsListening)
            {
                await location.StartListeningAsync(TimeSpan.FromSeconds(10), 1);

                return;
            }

            Pin pin = new Pin
            {
                Label = label,
                Address = "ubicacion",
                Type = PinType.Place,
                Position = new Position(latitud, longitud)
            };
            m.Pins.Add(pin);
            m.MoveToRegion(mapSpan: MapSpan.FromCenterAndRadius(new Position(latitud, longitud), Distance.FromKilometers(1)));

        }
    }
}