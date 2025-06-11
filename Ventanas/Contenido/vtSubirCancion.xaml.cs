using ClienteMusAPI.Clases;
using ClienteMusAPI.DTOs;
using ClienteMusAPI.Servicios;
using ClienteMusAPI.Ventanas.Perfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
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
    /// Lógica de interacción para vtSubirCancion.xaml
    /// </summary>
    public partial class vtSubirCancion : Page
    {
        private int idPerfilArtista;
        public vtSubirCancion(int idPerfilArtista)
        {
            InitializeComponent();
            this.idPerfilArtista = idPerfilArtista;
            CargarAlbumesAsync();
        }

        private async void CargarAlbumesAsync()
        {
            AlbumServicio albumServicio = new AlbumServicio();
            List<InfoAlbumDTO> albumesDelUsuario = new List<InfoAlbumDTO>();
            albumesDelUsuario = await albumServicio.ObtenerAlbumesPendientesAsync(idPerfilArtista);

            var albumes = new List<InfoAlbumDTO>
                {
                    new InfoAlbumDTO { idAlbum = -1, nombre = "N/A (Sencillo)" }
                };
            if (albumesDelUsuario != null && albumesDelUsuario.Count > 0)
            {
                albumes.AddRange(albumesDelUsuario);
            }
            
            cb_Album.ItemsSource = albumes;
            cb_Album.SelectedIndex = 0;
        }

        private void Click_Volver(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Click_Confirmar(object sender, RoutedEventArgs e)
        {

        }
    }
}
