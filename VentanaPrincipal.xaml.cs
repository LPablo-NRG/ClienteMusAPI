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

namespace ClienteMusAPI
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class VentanaPrincipal : Window
    {
        private MediaPlayer player = new MediaPlayer();


        private void VentanaPrincipal_Loaded(object sender, RoutedEventArgs e)
        {
            player.Open(new Uri("pack://siteoforigin:,,,/Recursos/Sonidos/MusAPI.wav"));
            MarcoPrincipal.Navigate(new Uri("/Ventanas/Inicio/vtInicioSesion.xaml", UriKind.Relative));
            
            player.Play();
        }
        public VentanaPrincipal()
        {
            InitializeComponent();
            this.Loaded += VentanaPrincipal_Loaded;
        }

    }
}
