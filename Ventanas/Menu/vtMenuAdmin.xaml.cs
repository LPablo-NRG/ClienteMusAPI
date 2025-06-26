using ClienteMusAPI.Ventanas.Busqueda;
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

namespace ClienteMusAPI.Ventanas.Menu
{
    /// <summary>
    /// Lógica de interacción para vtMenuAdmin.xaml
    /// </summary>
    public partial class vtMenuAdmin : Page
    {
        public vtMenuAdmin()
        {
            InitializeComponent();
            txb_Busqueda.GotFocus += txb_Busqueda_GotFocus;
            txb_Busqueda.LostFocus += txb_Busqueda_LostFocus;
        }

        private void txb_Busqueda_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void txb_Busqueda_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txb_Busqueda.Text))
            {
                txtPlaceholder.Visibility = Visibility.Visible;
            }
        }
        private void Click_Volver(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Click_CategoriasMusicales(object sender, RoutedEventArgs e)
        {
            vtCategoriasMusicales vtCategoriasMusicales = new vtCategoriasMusicales();
            NavigationService.Navigate(vtCategoriasMusicales);
        }

        private void Click_BuscarUsuarios(object sender, RoutedEventArgs e)
        {
            vtBusqueda busquedaUsuarios = new vtBusqueda(txb_Busqueda.Text, buscarUsuarios: true);
            NavigationService.Navigate(busquedaUsuarios);
        }

        private void Click_Reportes(object sender, RoutedEventArgs e)
        {
            vtReportes reportes = new vtReportes();
            NavigationService.Navigate(reportes);
        }

        private void Click_AdministrarUsuarios(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            vtReportes vtReportes = new vtReportes();
            NavigationService.Navigate(vtReportes);
        }
    }
}
