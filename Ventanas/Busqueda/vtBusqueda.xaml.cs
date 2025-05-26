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

namespace ClienteMusAPI.Ventanas.Busqueda
{
    /// <summary>
    /// Lógica de interacción para vtBusqueda.xaml
    /// </summary>
    public partial class vtBusqueda : Page
    {
        public vtBusqueda()
        {
            InitializeComponent();
            ucContenido twenty = new ucContenido("Artista");
            twenty.txb_Nombre.Text = "Twenty One Pilots";
            ucContenido muse = new ucContenido("Artista");
            muse.txb_Nombre.Text = "Muse";
            ucContenido idkh = new ucContenido("Artista");
            idkh.txb_Nombre.Text = "I Don't Know How But They Found Me";

            sp_Resultados.Children.Add(twenty);
            sp_Resultados.Children.Add(muse);
            sp_Resultados.Children.Add(idkh);
            sp_Resultados.Children.Add(new ucContenido());
            sp_Resultados.Children.Add(new ucContenido("Cancion"));
            sp_Resultados.Children.Add(new ucContenido("Lista"));
            sp_Resultados.Children.Add(new ucContenido("Album"));
            sp_Resultados.Children.Add(new ucContenido("Artista"));

        }

        private void Click_Volver(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Click_VerPerfil(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Ventanas/Perfiles/vtPerfilArtista.xaml", UriKind.Relative));
        }

        private void Click_VerPerfilUsuario(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Ventanas/Perfiles/vtPerfilUsuario.xaml", UriKind.Relative));
        }

        private void Click_BuscarContenido(object sender, RoutedEventArgs e)
        {
            //TODO: Implementar la lógica de búsqueda
        }
    }
}
