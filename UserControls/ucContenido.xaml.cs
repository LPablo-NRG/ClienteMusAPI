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
    /// Lógica de interacción para Contenido.xaml
    /// </summary>
    public partial class ucContenido : UserControl
    {
        String tipo;
        public ucContenido()
        {
            InitializeComponent();
            img_foto.Source = null;
        }
        public ucContenido(String tipo)
        {
            InitializeComponent();
            this.tipo = tipo;
            switch (tipo)
            {
                case "Album":
                    txb_Nombre.Text = "Album";
                    break;
                case "Cancion":
                    txb_Nombre.Text = "Cancion";
                    break;
                case "Lista":
                    txb_Nombre.Text = "Lista de Reproducción";
                    txb_Autor.Visibility = Visibility.Collapsed;
                    btn_Guardar.Visibility = Visibility.Collapsed;
                    img_foto.Source = new BitmapImage(new Uri("../Recursos/Iconos/iconoListaDeReproduccion.png", UriKind.Relative));
                    break;
                case "Artista":
                    txb_Nombre.Text = "Artista";
                    txb_Autor.Visibility = Visibility.Collapsed;
                    btn_Guardar.Visibility = Visibility.Collapsed;
                    btn_Reproducir.Visibility = Visibility.Collapsed;
                    img_foto.Source = new BitmapImage(new Uri("../Recursos/Iconos/iconoPerfil.png", UriKind.Relative));
                    break;

            }
        }

        private void Click_VerDetalles(object sender, RoutedEventArgs e)
        {
            switch (tipo)
            {
                case "Album":
                    NavigationService.GetNavigationService(this).Navigate(new Uri("/Ventanas/Contenido/vtAlbum.xaml", UriKind.Relative));
                    break;
                case "Cancion":
                    // Buscar el contenedor llamado "ContenedorGeneral"
                    DependencyObject parent = this;
                    while (parent != null && !(parent is Window))
                    {
                        parent = VisualTreeHelper.GetParent(parent);
                    }

                    if (parent is Window window)
                    {
                        var contenedor = (window as VentanaPrincipal)?.Contenido;
                        if (contenedor != null)
                        {
                            ucVentanaDetalles detalles = new ucVentanaDetalles();
                            detalles.btn_Cerrar.Click += (object sender2, RoutedEventArgs e2) =>
                            {
                                contenedor.Children.Remove(detalles);
                            };
                            contenedor.Children.Add(new ucVentanaDetalles());
                        }
                    }
                    break;

                case "Lista":
                    NavigationService.GetNavigationService(this).Navigate(new Uri("/Ventanas/Contenido/vtListaDeReproduccion.xaml", UriKind.Relative));
                    break;
                case "Artista":
                    NavigationService.GetNavigationService(this).Navigate(new Uri("/Ventanas/Perfiles/vtPerfilArtista.xaml", UriKind.Relative));    
                    break;

            }
        }

        private void Click_Guardar(object sender, RoutedEventArgs e)
        {

        }
        private void Click_Reproducir(object sender, RoutedEventArgs e)
        {

        }
    }
}
