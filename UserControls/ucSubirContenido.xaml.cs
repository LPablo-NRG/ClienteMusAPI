using ClienteMusAPI.Ventanas.Contenido;
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

namespace ClienteMusAPI.UserControls
{
    /// <summary>
    /// Lógica de interacción para ucSubirContenido.xaml
    /// </summary>
    public partial class ucSubirContenido : UserControl
    {
        private int idPerfilArtista;
        public ucSubirContenido(int idPerfilArtista)
        {
            InitializeComponent();
            this.idPerfilArtista = idPerfilArtista;
        }


        private void Click_CrearAlbum(object sender, RoutedEventArgs e)
        {
            NavigationService.GetNavigationService(this).Navigate(new Uri("/Ventanas/Contenido/vtCrearAlbum.xaml", UriKind.Relative));
        }

        private void Click_SubirCancion(object sender, RoutedEventArgs e)
        {
            vtSubirCancion vtSubirCancion = new vtSubirCancion(idPerfilArtista);
            NavigationService.GetNavigationService(this).Navigate(vtSubirCancion);
        }

        private void Click_Volver(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
