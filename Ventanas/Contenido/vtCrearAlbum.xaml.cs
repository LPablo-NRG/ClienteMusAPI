using ClienteMusAPI.Clases;
using ClienteMusAPI.DTOs;
using ClienteMusAPI.Servicios;
using ClienteMusAPI.Ventanas.Perfiles;
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
using AlbumDTOCliente = ClienteMusAPI.DTOs.AlbumDTO;


namespace ClienteMusAPI.Ventanas.Contenido
{
    /// <summary>
    /// Lógica de interacción para vtCrearAlbum.xaml
    /// </summary>
    public partial class vtCrearAlbum : Page
    {
        AlbumDTOCliente albumDTO = new AlbumDTOCliente();
        private AlbumServicio albumServicio = new AlbumServicio();
        private BusquedaAlbumDTO album = new BusquedaAlbumDTO();
        private bool esEdicion = false;
        private int idAlbumEditar = -1;


        public vtCrearAlbum()
        {
            InitializeComponent();
        }

        public vtCrearAlbum(BusquedaAlbumDTO album)
        {
            InitializeComponent();
            this.album = album;
            CargarDatosParaEditar(album);
        }

        public void CargarDatosParaEditar(BusquedaAlbumDTO album)
        {
            esEdicion = true;
            idAlbumEditar = album.idAlbum;

            lb_Titulo.Content = "Editar Álbum";
            txb_NombreAlbum.Text = album.nombreAlbum;
            CargarImagen(album.urlFoto);
            btn_Guardar.Content = "Editar";
        }

        private async void CargarImagen(string url)
        {
            if (!String.IsNullOrEmpty(url))
            {
                var bytes = await ClienteAPI.HttpClient.GetByteArrayAsync(Constantes.URL_BASE + url);
                using (var stream = new MemoryStream(bytes))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = stream;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();
                    img_PortadaAlbum.Source = image;
                }
            }
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
            
            albumDTO.Nombre = txb_NombreAlbum.Text;

            if (!esEdicion)
            {
                albumDTO.IdUsuario = SesionUsuario.IdUsuario;
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
            else
            {
                albumDTO.IdUsuario = album.idAlbum;
                bool exito = await albumServicio.EditarAlbumAsync(albumDTO);

                if (exito)
                {
                    MessageBox.Show("Álbum editado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    vtPerfilArtista vtPerfilArtista = new vtPerfilArtista(SesionUsuario.IdUsuario);
                    NavigationService.GetNavigationService(this).Navigate(vtPerfilArtista);
                }
                else
                {
                    MessageBox.Show("No se pudo editar el álbum.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
