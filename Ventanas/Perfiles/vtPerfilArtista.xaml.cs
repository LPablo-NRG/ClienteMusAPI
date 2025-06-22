using ClienteMusAPI.Clases;
using ClienteMusAPI.DTOs;
using ClienteMusAPI.Servicios;
using ClienteMusAPI.UserControls;
using ClienteMusAPI.Ventanas.Perfiles.Chat;
using ClienteMusAPI.Ventanas.Perfiles.Estadisticas;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace ClienteMusAPI.Ventanas.Perfiles
{
    /// <summary>
    /// Lógica de interacción para vtPerfilArtista.xaml
    /// </summary>
    public partial class vtPerfilArtista : Page
    {
        private UsuarioServicio usuarioServicio = new UsuarioServicio();
        private BusquedaArtistaDTO perfilArtista;
        private int idUsuario = -1;
        bool mostrarBotonGuardar = true;
        bool mostrarBotonEliminar = false;
        public vtPerfilArtista()
        {
            InitializeComponent();
        }

        public vtPerfilArtista(int idUsuario)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            this.Loaded += Page_Loaded;
        }

        public vtPerfilArtista(BusquedaArtistaDTO artista)
        {
            InitializeComponent();
            this.perfilArtista = artista;
            this.Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CargarDatos();
        }

        private async void CargarDatos()
        {
            if (perfilArtista == null)
            {
                perfilArtista = await usuarioServicio.ObtenerPerfilArtistaAsync(idUsuario);
            }
            if (perfilArtista.nombreUsuario == SesionUsuario.NombreUsuario)
            {
                sp_MenuOyente.Visibility = Visibility.Collapsed;
                mostrarBotonEliminar = true;
            } 
            else
            {
                sp_MenuArtista.Visibility = Visibility.Collapsed;
                mostrarBotonEliminar = false;
            }

            txb_Nombre.Text = perfilArtista.nombre;
            txb_Usuario.Text = "@" + perfilArtista.nombreUsuario;
            txb_Descripcion.Text = perfilArtista.descripcion;

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

            CargarAlbumesAsync();
            CargarSencillosAsync();
        }

        private async void CargarAlbumesAsync()
        {
            sp_Albumes.Children.Clear();
            AlbumServicio albumServicio = new AlbumServicio();
            List<BusquedaAlbumDTO> albumesDelUsuario = new List<BusquedaAlbumDTO>();
            albumesDelUsuario = await albumServicio.ObtenerAlbumesPublicosAsync(perfilArtista.idArtista);
            if (albumesDelUsuario != null)
            {
                foreach (var album in albumesDelUsuario)
                {
                    ucContenido contenido = new ucContenido(album, mostrarBotonGuardar, mostrarBotonEliminar);
                    sp_Albumes.Children.Add(contenido);
                }
            }
            
        }
        private async void CargarSencillosAsync()
        {
            sp_Sencillos.Children.Clear();
            CancionServicio cancionServicio = new CancionServicio();
            List<BusquedaCancionDTO> sencillos = new List<BusquedaCancionDTO>();
            sencillos = await cancionServicio.ObtenerSencillosPorArtistaAsync(perfilArtista.idArtista);
            if (sencillos != null)
            {
                foreach (var sencillo in sencillos)
                {
                    ucContenido contenido = new ucContenido(new List<BusquedaCancionDTO> { sencillo }, 0, mostrarBotonGuardar, mostrarBotonEliminar);
                    sp_Sencillos.Children.Add(contenido);
                }
            }
            
        }

        private void Click_Volver(object sender, RoutedEventArgs e)
        {
            vtPerfilUsuario vtPerfilUsuario = new vtPerfilUsuario();
            NavigationService.GetNavigationService(this).Navigate(vtPerfilUsuario);
        }

        private void Click_VerEstadisticas(object sender, RoutedEventArgs e)
        {
            vtEstadisticasContenidoSubido estadisticas = new vtEstadisticasContenidoSubido(perfilArtista.idArtista);
            NavigationService.Navigate(estadisticas);
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
            DependencyObject parent = this;
            while (parent != null && !(parent is Window))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            if (parent is Window window)
            {
                var contenedor = (window as VentanaPrincipal)?.Contenido;
                if (contenedor != null)
                {
                    ucEvaluarArtista evaluacion = new ucEvaluarArtista(perfilArtista.idArtista);
                    evaluacion.btn_Cerrar.Click += (object sender2, RoutedEventArgs e2) =>
                    {
                        contenedor.Children.Remove(evaluacion);
                    };
                    contenedor.Children.Add(evaluacion);
                }
            }
        }
        private void Click_VerChat(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new vtChat(perfilArtista, SesionUsuario.NombreUsuario));
        }
    }
}
