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

namespace ClienteMusAPI.Ventanas.Menu
{
    /// <summary>
    /// Lógica de interacción para MenuPrincipal.xaml
    /// </summary>
    public partial class vtMenuPrincipal : Page
    {
        public vtMenuPrincipal()
        {
            InitializeComponent();

            sp_albumes.Children.Add(new Contenido("Lista"));
            sp_albumes.Children.Add(new Contenido("Album"));
            sp_albumes.Children.Add(new Contenido("Artista"));
            sp_albumes.Children.Add(new Contenido());
            sp_albumes.Children.Add(new Contenido("Cancion"));

            sp_listas.Children.Add(new Contenido());
            sp_listas.Children.Add(new Contenido("Cancion"));
            sp_listas.Children.Add(new Contenido("Lista"));
            sp_listas.Children.Add(new Contenido("Album"));
            sp_listas.Children.Add(new Contenido("Artista"));
        }

        private void Click_MenuAdmin(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Ventanas/Menu/vtMenuAdmin.xaml", UriKind.Relative));
        }

        private void Click_BuscarContenido(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Ventanas/Busqueda/vtBusqueda.xaml", UriKind.Relative));
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
            NavigationService.GoBack();
        }

        private void Click_Salir(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

    }
}
