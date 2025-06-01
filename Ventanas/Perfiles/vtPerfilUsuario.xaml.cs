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
        public vtPerfilUsuario()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void CargarDatos()
        {
            txb_Nombre.Text = Clases.SesionUsuario.Nombre;
            txb_Usuario.Text = Clases.SesionUsuario.NombreUsuario;

        }

        private void CargarListas()
        {

        }

        private void Click_VerEstadisticas(object sender, RoutedEventArgs e)
        {

        }

        private void Click_EditarPerfil(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Ventanas/Perfiles/vtEditarPerfil.xaml", UriKind.Relative));
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

        private void Click_Volver(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
