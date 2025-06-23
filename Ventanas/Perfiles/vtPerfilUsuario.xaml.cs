using ClienteMusAPI.Servicios;
using ClienteMusAPI.UserControls;
using ClienteMusAPI.Ventanas.Menu;
using ClienteMusAPI.Ventanas.Perfiles.Estadisticas;
using System;
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

namespace ClienteMusAPI.Ventanas.Perfiles
{
    /// <summary>
    /// Lógica de interacción para vtPerfilUsuario.xaml
    /// </summary>
    public partial class vtPerfilUsuario : Page
    {
        bool mostrarBotonGuardar = true;
        bool mostrarBotonEliminar = false;
        public vtPerfilUsuario()
        {
            InitializeComponent();
            this.Loaded += Page_Loaded;
        }

        //al cargar la página, se cargan los datos del usuario
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CargarDatos();
            CargarListas();
        }

        private void CargarDatos()
        {
            btn_CrearPerfilArtista.Visibility = Visibility.Collapsed;
            btn_VerPerfilArtista.Visibility = Visibility.Collapsed;

            if (Clases.SesionUsuario.EsArtista == true)
            {
                btn_VerPerfilArtista.Visibility = Visibility.Visible;

            }
            else 
            {
                btn_CrearPerfilArtista.Visibility = Visibility.Visible;
            }
            txb_Nombre.Text = Clases.SesionUsuario.Nombre;
            txb_Usuario.Text = "@"+Clases.SesionUsuario.NombreUsuario;

        }

        private async void CargarListas()
        {
            var servicio = new ListaServicio();
            int idUsuario = Clases.SesionUsuario.IdUsuario;

            var listas = await servicio.ObtenerListasPorUsuarioAsync(idUsuario);

            sp_Listas.Children.Clear();

            if (listas != null)
            {
                foreach (var lista in listas)
                {
                    var uc = new ucContenido(lista, mostrarBotonGuardar, mostrarBotonEliminar);
                    sp_Listas.Children.Add(uc);
                }
            }

        }

        private void Click_VerEstadisticas(object sender, RoutedEventArgs e)
        {
            vtEstadisticasConsumoPersonal vtEstadisticasConsumoPersonal = new vtEstadisticasConsumoPersonal(Clases.SesionUsuario.IdUsuario);
            NavigationService.Navigate(vtEstadisticasConsumoPersonal);
        }

        private void Click_EditarPerfil(object sender, RoutedEventArgs e)
        {
            vtEditarPerfil vtEditarPerfil = new vtEditarPerfil();
            NavigationService.Navigate(vtEditarPerfil);
        }

        private void Click_CrearPerfilArtista(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Funcionalidad no implementada aún.", "Información", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (MessageBoxResult.Yes == MessageBox.Show("¿Deseas crear un perfil de artista?\nEsto asociará el perfil a tu cuenta de usuario.", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question))
            {
                // Aquí se podría navegar a una página para crear el perfil de artista
                NavigationService.Navigate(new Uri("/Ventanas/Perfiles/vtCrearPerfilArtista.xaml", UriKind.Relative));
            }
        }

        private void Click_VerPerfilArtista(object sender, RoutedEventArgs e)
        {
            vtPerfilArtista vtPerfilArtista = new vtPerfilArtista(Clases.SesionUsuario.IdUsuario);
            NavigationService.Navigate(vtPerfilArtista);
        }

        private void Click_Volver(object sender, RoutedEventArgs e)
        {
            var vtMenuPrincipal = new vtMenuPrincipal();
            NavigationService.GetNavigationService(this).Navigate(vtMenuPrincipal);
        }
    }
}
