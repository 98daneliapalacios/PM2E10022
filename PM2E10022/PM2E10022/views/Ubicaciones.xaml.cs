using PM2E10022.clases;
using PM2E10022.model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PM2E10022.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Ubicaciones : ContentPage
    {
        List<Modelo> service;
        OperacionesDB crud = new OperacionesDB();
        byte[] image;
        public Ubicaciones()
        {
            InitializeComponent();
            OnAppearing();

        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                var lista_Ubicaciones = await crud.getReadUbicacion();
                if (lista_Ubicaciones != null)
                {
                    listaUbicaciones.ItemsSource = lista_Ubicaciones;
                }
            }
            catch (SQLiteException e)
            {
                await DisplayAlert("Mensaje", "No hay ubicaciones guardadas", "Cerrar");

            }
        }

        private async void ShowMapa_Clicked(object sender, EventArgs e)
        {
            if ((string.IsNullOrEmpty(id.Text)))
            {
                await DisplayAlert("Mensaje", "Seleccione una ubicacion", "Cerrar");
                return;
            }
            bool mosrar = await DisplayAlert("Mensaje", "Desea ver el mapa", "Si", "No");
            if (mosrar)
            {
                await Navigation.PushAsync(new Mapa(Convert.ToDouble(latitud.Text), Convert.ToDouble(longitud.Text), descripcion.Text));
            }
        }

        private async void lista_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var ubic = new Modelo();
            var obj = (Modelo)e.SelectedItem;
            if (!string.IsNullOrEmpty(obj.id.ToString()))
            {
                var lst = await crud.getUbicacionId(obj.id);
                if (lst != null)
                {
                    id.Text = (lst.id).ToString();
                    descripcion.Text = lst.descripcion;
                    latitud.Text = (lst.latitud).ToString();
                    longitud.Text = lst.longitud.ToString();
                    image = lst.fotografia;
                }
            }
        }

        private async void Eliminar_Clicked(object sender, EventArgs e)
        {
            var ubic = await crud.getUbicacionId(Convert.ToInt32(id.Text));
            if ((string.IsNullOrEmpty(id.Text)))
            {
                await DisplayAlert("Mensaje", "Seleccione el elemento a borrar", "Cerrar");
                return;
            }
            bool mostrar = await DisplayAlert("Mensaje", "Desea eliminar este valor", "Si", "No");
            if (mostrar)
            {

                if (ubic != null)
                {
                    await crud.Delete(ubic);
                    await DisplayAlert("Mensaje", "Ubicacion eliminada", "Cerrar");
                    OnAppearing();
                }
                else
                {
                    await DisplayAlert("Mensaje", "Seleccione un elemento para borrar", "Cerrar");
                }
                OnAppearing();

            }

        }

        private async void actualizar_Clicked(object sender, EventArgs e)
        {
            if ((string.IsNullOrEmpty(id.Text)))
            {
                await DisplayAlert("Mensaje", "Seleccione el elemento a actualizar", "Cerrar");
                return;
            }
            bool mostrar = await DisplayAlert("Mensaje", "Desea actualizar este valor", "Si", "No");
            if (mostrar)
            {
                var lst = new Lista
                {
                    idlista = Convert.ToInt32(id.Text),
                    latitudLista = Convert.ToDouble(latitud.Text),
                    longitudLista = Convert.ToDouble(longitud.Text),
                    descripcionLista = descripcion.Text,
                    imagenlista = image,
                 
                };
                var upd = new Actualizar();
                upd.BindingContext = lst;
                await Navigation.PushAsync(upd);
            }


        }
    }
}