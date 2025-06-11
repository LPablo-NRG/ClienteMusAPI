using ClienteMusAPI.Clases;
using ClienteMusAPI.DTOs;
using ClienteMusAPI.Servicios;
using ClienteMusAPI.UserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
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

namespace ClienteMusAPI.Ventanas.Perfiles
{
    /// <summary>
    /// Lógica de interacción para vtPerfilArtista.xaml
    /// </summary>
    public partial class vtPerfilArtista : Page
    {
        private UsuarioServicio usuarioServicio = new UsuarioServicio();
        private BusquedaArtistaDTO perfilArtista = new BusquedaArtistaDTO();
        private int idUsuario = -1;
        public vtPerfilArtista()
        {
            InitializeComponent();
            sp_albumes.Children.Add(new ucContenido("Album"));
            sp_albumes.Children.Add(new ucContenido("Album"));
            sp_albumes.Children.Add(new ucContenido("Album"));


            sp_sencillos.Children.Add(new ucContenido("Cancion"));
            sp_sencillos.Children.Add(new ucContenido("Cancion"));
            sp_sencillos.Children.Add(new ucContenido("Cancion"));
            sp_sencillos.Children.Add(new ucContenido("Cancion"));
            sp_sencillos.Children.Add(new ucContenido("Cancion"));
            sp_sencillos.Children.Add(new ucContenido("Cancion"));
            sp_sencillos.Children.Add(new ucContenido("Cancion"));
            sp_sencillos.Children.Add(new ucContenido("Cancion"));
            sp_sencillos.Children.Add(new ucContenido("Cancion"));
            sp_sencillos.Children.Add(new ucContenido("Cancion"));
            sp_sencillos.Children.Add(new ucContenido("Cancion"));
            sp_sencillos.Children.Add(new ucContenido("Cancion"));
        }

        public vtPerfilArtista(int idUsuario)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            this.Loaded += Page_Loaded;
        }
        
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (idUsuario != -1)
                CargarDatos();
        }

        private async void CargarDatos()
        {
            perfilArtista = await usuarioServicio.ObtenerPerfilArtistaAsync(idUsuario);

            txb_Nombre.Text = perfilArtista.nombre;
            txb_Usuario.Text = "@" + perfilArtista.nombreUsuario;
            txb_Descripcion.Text = perfilArtista.descripcion;

            //cargar imagen
            if (!String.IsNullOrEmpty(perfilArtista.urlFoto))
            {
                var bytes = await ClienteAPI.HttpClient.GetByteArrayAsync(Constantes.URL_BASE + perfilArtista.urlFoto);
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


        private void Click_Volver(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Click_VerEstadisticas(object sender, RoutedEventArgs e)
        {

        }

        private void Click_EditarPerfil(object sender, RoutedEventArgs e)
        {

        }

        private void Click_SubirContenido(object sender, RoutedEventArgs e)
        {
            OverlayGrid.Children.Clear(); // Limpiar controles anteriores
            OverlayGrid.Visibility = Visibility.Visible;

            ucSubirContenido detalles = new ucSubirContenido(perfilArtista.idArtista);
            detalles.btn_Volver.Click += (sender2, e2) =>
            {
                OverlayGrid.Visibility = Visibility.Collapsed;
                OverlayGrid.Children.Remove(detalles);
            };

            OverlayGrid.Children.Add(detalles);
        }

        private void Click_SeguirArtista(object sender, RoutedEventArgs e)
        {

        }
        private void Click_EvaluarArtista(object sender, RoutedEventArgs e)
        {

        }
        private void Click_VerChat(object sender, RoutedEventArgs e)
        {

        }
    }
}
