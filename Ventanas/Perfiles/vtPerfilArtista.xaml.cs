using ClienteMusAPI.UserControls;
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
    /// Lógica de interacción para vtPerfilArtista.xaml
    /// </summary>
    public partial class vtPerfilArtista : Page
    {
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
