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

namespace ClienteMusAPI.Ventanas.Inicio
{
    /// <summary>
    /// Lógica de interacción para InicioSesion.xaml
    /// </summary>
    public partial class vtInicioSesion : Page
    {
        //VentanaPrincipal ventanaPrincipal = new VentanaPrincipal();
        public vtInicioSesion()
        {
            InitializeComponent();
            this.Loaded += vtInicioSesion_Loaded;
        } 

        private void vtInicioSesion_Loaded(object sender, RoutedEventArgs e)
        {
            VentanaPrincipal vt = (VentanaPrincipal)Application.Current.MainWindow;
            vt.Reproductor.Visibility = Visibility.Collapsed;
        }

        private void Click_RegistrarCuenta(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Ventanas/Inicio/vtRegistroCuenta.xaml", UriKind.Relative));
        }

        private void Click_IniciarSesion(object sender, RoutedEventArgs e)
        {
            VentanaPrincipal vt = (VentanaPrincipal)Application.Current.MainWindow;
            vt.Reproductor.Visibility = Visibility.Visible;

            NavigationService.Navigate(new Uri("/Ventanas/Menu/vtMenuPrincipal.xaml", UriKind.Relative));
        }

        private void Click_Salir(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}
