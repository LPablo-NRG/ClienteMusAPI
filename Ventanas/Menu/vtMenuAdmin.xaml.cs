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
    }
}
