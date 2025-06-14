using ClienteMusAPI.Clases;
using ClienteMusAPI.DTOs;
using ClienteMusAPI.Servicios;
using ClienteMusAPI.Ventanas.Perfiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClienteMusAPI.UserControls
{
    /// <summary>
    /// Lógica de interacción para VentanaDetalles.xaml
    /// </summary>
    public partial class ucVentanaDetalles : UserControl
    {
        BusquedaCancionDTO busquedaCancionDTO;
        public ucVentanaDetalles(BusquedaCancionDTO busquedaCancionDTO)
        {
            InitializeComponent();
            this.busquedaCancionDTO = busquedaCancionDTO;
            CargarDatos();
        }

        private async void CargarDatos()
        {
            if (busquedaCancionDTO != null)
            {
                txb_Nombre.Text = busquedaCancionDTO.nombre;
                txb_Autor.Text = busquedaCancionDTO.nombreArtista;
                if (busquedaCancionDTO.nombreAlbum != null)
                {
                    txb_Album.Text = busquedaCancionDTO.nombreAlbum;
                }
                else
                {
                    sp_Album.Visibility = Visibility.Collapsed;
                }
                txb_Duracion.Text = busquedaCancionDTO.duracion;
                txb_Fecha.Text = busquedaCancionDTO.fechaPublicacion;

                //cargar imagen
                if (!String.IsNullOrEmpty(busquedaCancionDTO.urlFoto))
                {
                    var bytes = await ClienteAPI.HttpClient.GetByteArrayAsync(Constantes.URL_BASE + busquedaCancionDTO.urlFoto);
                    using (var stream = new MemoryStream(bytes))
                    {
                        var image = new BitmapImage();
                        image.BeginInit();
                        image.StreamSource = stream;
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.EndInit();
                        img_foto.Source = image;
                    }
                }
            }
        }

        private void Click_Cerrar(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
