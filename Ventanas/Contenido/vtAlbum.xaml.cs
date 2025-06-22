using ClienteMusAPI.Clases;
using ClienteMusAPI.DTOs;
using ClienteMusAPI.Servicios;
using ClienteMusAPI.UserControls;
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

namespace ClienteMusAPI.Ventanas.Contenido
{
    /// <summary>
    /// Lógica de interacción para vtAlbum.xaml
    /// </summary>
    public partial class vtAlbum : Page
    {
        private InfoAlbumDTO albumPendiente;
        private int idArtista;
        private BusquedaAlbumDTO albumPublico;
        bool mostrarBotonGuardar = true;
        bool mostrarBotonoEliminar = false;
        public vtAlbum()
        {
            InitializeComponent();
        }

        public vtAlbum(BusquedaAlbumDTO album)
        {
            InitializeComponent();
            btn_SubirCancion.Visibility = Visibility.Collapsed;
            btn_PublicarAlbum.Visibility = Visibility.Collapsed;
            this.albumPublico = album;
            this.Loaded += Page_Loaded;
        }

        public vtAlbum(InfoAlbumDTO album, int idArtista)
        {
            InitializeComponent();
            this.albumPendiente = album;
            this.idArtista = idArtista;
            this.Loaded += Page_Loaded;

        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            sp_Canciones.Children.Clear();
            if (albumPublico != null)
            {
                await CargarInformacionAlbumPublicoAsync();
            }
            else if (albumPendiente != null)
            {
                await CargarInformacionAlbumPendienteAsync();
            }
        }

        private async Task CargarInformacionAlbumPublicoAsync()
        {
            txb_Artista.Text = albumPublico.nombreArtista;
            txb_Nombre.Text = albumPublico.nombreAlbum;
            img_foto.Source = new BitmapImage(new Uri(albumPublico.urlFoto, UriKind.RelativeOrAbsolute));
            txb_Fecha.Text = albumPublico.fechaPublicacion;
            
            //cargar imagen
            if (!String.IsNullOrEmpty(albumPublico.urlFoto))
            {
                var bytes = await ClienteAPI.HttpClient.GetByteArrayAsync(Constantes.URL_BASE + albumPublico.urlFoto);
                using (var stream = new MemoryStream(bytes))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = stream;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();
                    img_foto.Source = image;
                }
            }
            await CargarCanciones(albumPublico.canciones);
        }

        private async Task CargarInformacionAlbumPendienteAsync()
        {
            txb_Artista.Text = albumPendiente.nombreArtista;
            txb_Nombre.Text = albumPendiente.nombre;
            img_foto.Source = new BitmapImage(new Uri(albumPendiente.urlFoto, UriKind.RelativeOrAbsolute));
            
            //cargar imagen
            if (!String.IsNullOrEmpty(albumPendiente.urlFoto))
            {
                var bytes = await ClienteAPI.HttpClient.GetByteArrayAsync(Constantes.URL_BASE + albumPendiente.urlFoto);
                using (var stream = new MemoryStream(bytes))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = stream;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();
                    img_foto.Source = image;
                }
            }
            await CargarCanciones(null);

        }

        private async Task CargarCanciones(List<BusquedaCancionDTO> canciones)
        {
            if (canciones == null)
            {
                CancionServicio cancionServicio = new CancionServicio();
                canciones = await cancionServicio.ObtenerCancionesPorAlbumAsync(albumPendiente.idAlbum);
            }
            
            if (canciones != null)
            {
                int indice = 0;
                TimeSpan duracionTotal = TimeSpan.Zero;
                foreach (var cancion in canciones)
                {
                    ucContenido contenido = new ucContenido(canciones, indice, mostrarBotonGuardar, mostrarBotonoEliminar);
                    sp_Canciones.Children.Add(contenido);
                    indice++;
                    TimeSpan duracionRecuperada = TimeSpan.ParseExact(cancion.duracion, @"mm\:ss", null);
                    duracionTotal += duracionRecuperada;
                }
                txb_Duracion.Text = duracionTotal.ToString(@"hh\:mm\:ss");
            }
            else
                Console.WriteLine("Error");
        }

        private void Click_Volver(object sender, RoutedEventArgs e)
        {
            vtPerfilUsuario vtPerfilUsuario = new vtPerfilUsuario();
            NavigationService.GetNavigationService(this).Navigate(vtPerfilUsuario);
        }

        private void Click_VerEstadisticas(object sender, RoutedEventArgs e)
        {
            //TODO
        }

        private void Click_EditarAlbum(object sender, RoutedEventArgs e)
        {
            if (albumPublico != null)
            {
                vtCrearAlbum vtCrearAlbum = new vtCrearAlbum(albumPublico);
                NavigationService.GetNavigationService(this).Navigate(vtCrearAlbum);
            }
            else if (albumPendiente != null)
            {
                var album = new BusquedaAlbumDTO();
                album.idAlbum = albumPendiente.idAlbum;
                album.nombreAlbum = albumPendiente.nombre;
                album.urlFoto = albumPendiente.urlFoto;
                vtCrearAlbum vtCrearAlbum = new vtCrearAlbum(album);
                NavigationService.GetNavigationService(this).Navigate(vtCrearAlbum);
            }

        }

        private async void Click_GuardarAlbum(object sender, RoutedEventArgs e)
        {
            var servicio = new ContenidoGuardadoServicio();
            var dto = new ContenidoGuardadoDTO
            {
                IdUsuario = SesionUsuario.IdUsuario,
                TipoDeContenido = "ALBUM"
            };
            if(albumPublico != null)
                dto.IdContenidoGuardado = albumPublico.idAlbum;
            else if (albumPendiente != null)
                dto.IdContenidoGuardado = albumPendiente.idAlbum;

            var mensaje = await servicio.GuardarContenidoAsync(dto);
            if (!string.IsNullOrEmpty(mensaje))
            {
                MessageBox.Show(mensaje);
                if (mensaje == "Contenido guardado exitosamente")
                {
                    btn_Seguir
                        .Visibility = Visibility.Collapsed;
                }
            }
        }

        private void Click_SubirCancion(object sender, RoutedEventArgs e)
        {
            vtSubirCancion vtSubirCancion = new vtSubirCancion(idArtista, albumPendiente.idAlbum);
            NavigationService.GetNavigationService(this).Navigate(vtSubirCancion);
        }

        private async void Click_PublicarAlbum(object sender, RoutedEventArgs e)
        {
            if (sp_Canciones.Children.Count < 2)
            {
                MessageBox.Show("Un álbum debe tener por lo menos 2 canciones para poder ser publicado.");
                return;
            }
            if (MessageBoxResult.OK == MessageBox.Show("¿Desea publicar el album?", "Publicar Album", MessageBoxButton.OKCancel))
            {
                AlbumServicio albumServicio = new AlbumServicio();
                bool exito = await albumServicio.PublicarAlbum(albumPendiente.idAlbum);
                if (exito)
                {
                    MessageBox.Show("El album se ha publicado correctamente.");
                    NavigationService.GoBack();
                }
                else
                {
                    MessageBox.Show("Error al publicar el album.");
                }
            }
        }

        private async void Click_EliminarAlbum(object sender, RoutedEventArgs e)
        {
            if(albumPublico != null)
            {
                Console.WriteLine("Eliminando album publico: IdAlbum: "+albumPublico.idAlbum);
                var resultadoAlbum = MessageBox.Show("¿Estás seguro de que deseas eliminar este album?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (resultadoAlbum == MessageBoxResult.Yes)
                {
                    AlbumServicio servicio = new AlbumServicio();
                    bool exito = await servicio.EliminarAlbumAsync(albumPublico.idAlbum);
                    if (exito)
                    {
                        MessageBox.Show("Album eliminado correctamente.");
                        this.Visibility = Visibility.Collapsed;
                    }
                }
                return;
            }

            if(albumPendiente != null)
            {
                Console.WriteLine("Eliminando album pendiente: IdAlbum: " + albumPendiente.idAlbum);
                var resultadoAlbum = MessageBox.Show("¿Estás seguro de que deseas eliminar este album?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (resultadoAlbum == MessageBoxResult.Yes)
                {
                    AlbumServicio servicio = new AlbumServicio();
                    bool exito = await servicio.EliminarAlbumAsync(albumPendiente.idAlbum);
                    if (exito)
                    {
                        MessageBox.Show("Album eliminado correctamente.");
                        this.Visibility = Visibility.Collapsed;
                    }
                }
                return;
            }

        }

            
    }
}
