using System;
using ClienteMusAPI.UserControls;
using System.Collections.Generic;
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
using ClienteMusAPI.Ventanas.Busqueda;
using ClienteMusAPI.Ventanas.Inicio;
using ClienteMusAPI.Clases;
using ClienteMusAPI.Servicios;
using ClienteMusAPI.Ventanas.Contenido;

namespace ClienteMusAPI.Ventanas.Menu
{
    /// <summary>
    /// Lógica de interacción para MenuPrincipal.xaml
    /// </summary>
    public partial class vtMenuPrincipal : Page
    {

        bool mostrarBotonGuardar = false;
        bool mostrarBotonEliminar = false;
        public vtMenuPrincipal()
        {
            InitializeComponent();
            CargarContenidoGuardado();
        }

        private void Click_MenuAdmin(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Ventanas/Menu/vtMenuAdmin.xaml", UriKind.Relative));
        }

        private void Click_BuscarContenido(object sender, RoutedEventArgs e)
        {
            vtBusqueda busqueda = new vtBusqueda(txb_Busqueda.Text);
            NavigationService.Navigate(busqueda);
        }

        private void Click_VerPerfilArtista(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Ventanas/Perfiles/vtPerfilArtista.xaml", UriKind.Relative));
        }

        private void Click_VerPerfilUsuario(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Ventanas/Perfiles/vtPerfilUsuario.xaml", UriKind.Relative));
        }

        private void Click_CerrarSesion(object sender, RoutedEventArgs e)
        {
            ;
            var ventana = Window.GetWindow(this) as VentanaPrincipal;
            if (ventana != null)
            {
                ventana.LimpiarInterfazReproductor();

                vtInicioSesion inicioSesion = new vtInicioSesion();
                NavigationService.Navigate(inicioSesion);
            }
            Reproductor.Detener();
            Reproductor.indiceActual = 0;
            Reproductor.listaCanciones = null;
        }

        private void Click_CrearListaDeReproduccion(object sender, RoutedEventArgs e)
        {
            vtCrearLista crearLista = new vtCrearLista();
            NavigationService.Navigate(crearLista);

        } 

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollViewer = sender as ScrollViewer;
            if (scrollViewer != null)
            {
                // Desplazar horizontalmente según el delta del mouse
                double offset = scrollViewer.HorizontalOffset - e.Delta;
                scrollViewer.ScrollToHorizontalOffset(offset);

                // Marcar el evento como manejado para evitar desplazamiento vertical predeterminado
                e.Handled = true;
            }
        }

        private async void CargarContenidoGuardado()
        {
            var servicio = new ContenidoGuardadoServicio();
            int idUsuario = SesionUsuario.IdUsuario;

            var albumes = await servicio.ObtenerAlbumesGuardadosAsync(idUsuario);
            foreach (var album in albumes)
            {
                ucContenido uc = new ucContenido(album, mostrarBotonGuardar, mostrarBotonEliminar);
                uc.MostrarBotonGuardar = false;
                sp_albumes.Children.Add(uc);
            }

            var listas = await servicio.ObtenerListasGuardadasAsync(idUsuario);
            foreach (var lista in listas)
            {
                ucContenido uc = new ucContenido(lista, mostrarBotonGuardar, mostrarBotonEliminar);
                uc.MostrarBotonGuardar = false;
                sp_listas.Children.Add(uc);
            }

            var artistas = await servicio.ObtenerArtistasGuardadosAsync(idUsuario);
            foreach (var artista in artistas)
            {
                ucContenido uc = new ucContenido(artista, mostrarBotonGuardar, mostrarBotonEliminar);
                uc.MostrarBotonGuardar = false;
                sp_Artistas.Children.Add(uc);
            }
        }

    }
}
