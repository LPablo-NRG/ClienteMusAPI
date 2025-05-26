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
            ucContenido trench = new ucContenido("Album");
            trench.txb_Nombre.Text = "Trench";
            ucContenido ssi = new ucContenido("Album");
            ssi.txb_Nombre.Text = "Sadness Sorrow Imathgination Experiment";
            ucContenido blurry = new ucContenido("Album");
            blurry.txb_Nombre.Text = "Blurryface";
            ucContenido htbahb = new ucContenido("Album");
            htbahb.txb_Nombre.Text = "How to be a human being";
            ucContenido thenewabnormal = new ucContenido("Album");
            thenewabnormal.txb_Nombre.Text = "The New Abnormal";

            sp_albumes.Children.Add(trench);
            sp_albumes.Children.Add(ssi);
            sp_albumes.Children.Add(blurry);
            sp_albumes.Children.Add(htbahb);
            sp_albumes.Children.Add(thenewabnormal);
            sp_albumes.Children.Add(new ucContenido("Album"));
            sp_albumes.Children.Add(new ucContenido("Album"));
            sp_albumes.Children.Add(new ucContenido("Album"));


            sp_listas.Children.Add(new ucContenido("Lista"));
            sp_listas.Children.Add(new ucContenido("Lista"));
            sp_listas.Children.Add(new ucContenido("Lista"));
            sp_listas.Children.Add(new ucContenido("Lista"));
            sp_listas.Children.Add(new ucContenido("Lista"));
            sp_listas.Children.Add(new ucContenido("Lista"));

            ucContenido twenty = new ucContenido("Artista");
            twenty.txb_Nombre.Text = "Twenty One Pilots";
            ucContenido muse = new ucContenido("Artista");
            muse.txb_Nombre.Text = "Muse";
            ucContenido idkh = new ucContenido("Artista");
            idkh.txb_Nombre.Text = "I Don't Know How But They Found Me";

            sp_Artistas.Children.Add(twenty);
            sp_Artistas.Children.Add(muse);
            sp_Artistas.Children.Add(idkh);
            sp_Artistas.Children.Add(new ucContenido("Artista"));
            sp_Artistas.Children.Add(new ucContenido("Artista"));
            sp_Artistas.Children.Add(new ucContenido("Artista"));
            sp_Artistas.Children.Add(new ucContenido("Artista"));
            sp_Artistas.Children.Add(new ucContenido("Artista"));
            sp_Artistas.Children.Add(new ucContenido("Artista"));
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

        private void Click_CrearListaDeReproduccion(object sender, RoutedEventArgs e)
        {
            
        }

        private void Click_Salir(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
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

    }
}
