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

namespace ClienteMusAPI.Ventanas.Contenido
{
    /// <summary>
    /// Lógica de interacción para vtListaDeReproduccion.xaml
    /// </summary>
    public partial class vtListaDeReproduccion : Page
    {
        public vtListaDeReproduccion()
        {
            InitializeComponent();

            ucContenido tostados = new ucContenido("Cancion");
            tostados.txb_Nombre.Text = "tostados";
            ucContenido hurt = new ucContenido("Cancion");
            hurt.txb_Nombre.Text = "hurt";
            ucContenido rosies = new ucContenido("Cancion");
            rosies.txb_Nombre.Text = "rosies";
            ucContenido theOutside = new ucContenido("Cancion");
            theOutside.txb_Nombre.Text = "The Outside";

            sp_Canciones.Children.Add(tostados);
            sp_Canciones.Children.Add(hurt);
            sp_Canciones.Children.Add(rosies);
            sp_Canciones.Children.Add(theOutside);
        }
        private void Click_Volver(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Click_VerEstadisticas(object sender, RoutedEventArgs e)
        {

        }

        private void Click_EditarListaDeReproduccion(object sender, RoutedEventArgs e)
        {

        }

        private void Click_GuardarListaDeReproduccion(object sender, RoutedEventArgs e)
        {

        }
    }
}
