using ClienteMusAPI.Clases;
using ClienteMusAPI.DTOs;
using ClienteMusAPI.Servicios;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Lógica de interacción para vtCrearAlbum.xaml
    /// </summary>
    public partial class vtCrearAlbum : Page
    {
        private AlbumDTO albumDTO = new AlbumDTO();
        private AlbumServicio albumServicio = new AlbumServicio();

        public vtCrearAlbum()
        {
            InitializeComponent();
        }

        private void Click_Volver(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Click_Cancelar(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private async void Click_Guardar(object sender, RoutedEventArgs e)
        {
            albumDTO.IdUsuario = SesionUsuario.IdUsuario;
            albumDTO.Nombre = txb_NombreAlbum.Text;

            bool exito = await albumServicio.CrearAlbumAsync(albumDTO);

            if (exito)
            {
                MessageBox.Show("Álbum creado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("No se pudo crear el álbum.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Click_SubirFoto(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Imágenes (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                FileInfo informacionFoto = new FileInfo(openFileDialog.FileName);
                const long tamanioMaximo = 10 * 1024 * 1024;

                if (informacionFoto.Length > tamanioMaximo)
                {
                    MessageBox.Show("La imagen supera el tamaño máximo permitido.");
                    return;
                }

                BitmapImage bitmap = new BitmapImage(new Uri(openFileDialog.FileName));
                img_PortadaAlbum.Source = bitmap;
                albumDTO.FotoPath = openFileDialog.FileName;
            }
        }
    }
}
