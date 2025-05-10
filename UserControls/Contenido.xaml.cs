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
    public partial class Contenido : UserControl
    {
        public Contenido()
        {
            InitializeComponent();
            img_foto.Source = null;
        }
        public Contenido(String tipo)
        {
            InitializeComponent();
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
                    break;
                case "Artista":
                    txb_Nombre.Text = "Artista";
                    txb_Autor.Visibility = Visibility.Collapsed;
                    btn_Guardar.Visibility = Visibility.Collapsed;
                    btn_Reproducir.Visibility = Visibility.Collapsed;
                    break;

            }
        }

        private void Click_VerDetalles(object sender, RoutedEventArgs e)
        {

        }

        private void Click_Guardar(object sender, RoutedEventArgs e)
        {

        }
        private void Click_Reproducir(object sender, RoutedEventArgs e)
        {

        }
    }
}
