using Plugin.Geolocator;
using Plugin.Media;
using Plugin.Media.Abstractions;
using PM2E10022.clases;
using PM2E10022.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PM2E10022.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Actualizar : ContentPage
    {
        List<Modelo> service;
      
        double latitud, longitud;
        byte[] image;
        public Actualizar()
        {
            InitializeComponent();
        }

       
        private async void Actualizar_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Descripcion.Text))
            {
                await DisplayAlert("Alerta", "Debe describir la ubicacion", "ok");
                return;
            }
            if (imagefile.Source == null)
            {
                await DisplayAlert("Alerta", "Seleccione Imagen", "ok");
                return;
            }


            else
            {
                OperacionesDB crud = new OperacionesDB();
                cnx conn = new cnx();
                var ubicacion = new Modelo()
                {
                    id = Convert.ToInt32(Id.Text),
                    latitud = Convert.ToDouble(Latitud.Text),
                    longitud = Convert.ToDouble(Longitud.Text),
                    descripcion = Descripcion.Text,
                    fotografia = image

                };

                conn.Conn().Update(ubicacion);
                await DisplayAlert("Success", "Ubicacion actualizada", "Ok");



            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var ubic = CrossGeolocator.Current;
            ubic.DesiredAccuracy = 50;

            if (!ubic.IsGeolocationEnabled || !ubic.IsGeolocationAvailable)
            {
                await DisplayAlert("Warning", " Debe activar el GPS", "Salir");
            }
            else
            {
                if (!ubic.IsListening)
                {
                    await ubic.StartListeningAsync(TimeSpan.FromSeconds(10), 1);
                }
                ubic.PositionChanged += (posicion, args) =>
                {
                    var ubicacion = args.Position;
                    Latitud.Text = ubicacion.Latitude.ToString();
                    latitud = Convert.ToDouble(Latitud.Text);
                    Longitud.Text = ubicacion.Longitude.ToString();
                    longitud = Convert.ToDouble(Longitud.Text);
                };

            }
        }
      
     
        private async void Foto_Clicked(object sender, EventArgs e)
        {
            var img = new StoreCameraMediaOptions();
            img.PhotoSize = PhotoSize.Full;
            img.Name = "img";
            img.Directory = "PM2E10022";
            var foto = await CrossMedia.Current.TakePhotoAsync(img);
            if (foto != null)
            {
                imagefile.Source = ImageSource.FromStream(() => {
                    return foto.GetStream();
                });
                imagefile.IsVisible = true;
                using (MemoryStream memory = new MemoryStream())
                {
                    Stream stream = foto.GetStream();
                    stream.CopyTo(memory);
                    image = memory.ToArray();
                }
            }
        }
    }
}