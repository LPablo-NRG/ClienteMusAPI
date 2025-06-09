using ClienteMusAPI.Clases;
using ClienteMusAPI.DTOs;
using ClienteMusAPI.Servicios;
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
    /// Lógica de interacción para vtCrearLista.xaml
    /// </summary>
    public partial class vtCrearLista : Page
    {
        public vtCrearLista()
        {
            InitializeComponent();
        }

        private string rutaImagen = null;

        private void Click_SubirFoto(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Imágenes (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
            };

            if (dlg.ShowDialog() == true)
            {
                rutaImagen = dlg.FileName;
                img_PortadaLista.Source = new BitmapImage(new Uri(rutaImagen));
            }
        }

        private async void Click_Guardar(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txb_NombreLista.Text) || string.IsNullOrWhiteSpace(txb_DescripcionLista.Text))
            {
                MessageBox.Show("Por favor completa todos los campos.");
                return;
            }

            var dto = new ListaReproduccionDTO
            {
                Nombre = txb_NombreLista.Text,
                Descripcion = txb_DescripcionLista.Text,
                IdUsuario = SesionUsuario.IdUsuario,
                FotoPath = rutaImagen
            };

            var servicio = new ListaServicio();
            await servicio.CrearListaReproduccionAsync(dto);
        }

        private void Click_Volver(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Click_Cancelar(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

    }


}
