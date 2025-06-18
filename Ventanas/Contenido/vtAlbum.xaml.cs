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
        public vtAlbum()
        {
            InitializeComponent();

            ucContenido jumpsuit = new ucContenido("Cancion");
            jumpsuit.txb_Nombre.Text = "Jumpsuit";
            ucContenido levitate = new ucContenido("Cancion");
            levitate.txb_Nombre.Text = "Levitate";
            ucContenido morph = new ucContenido("Cancion");
            morph.txb_Nombre.Text = "Morph";
            ucContenido myblood = new ucContenido("Cancion");
            myblood.txb_Nombre.Text = "My Blood";
            ucContenido chlorine = new ucContenido("Cancion");
            chlorine.txb_Nombre.Text = "Chlorine";
            ucContenido smithereens = new ucContenido("Cancion");
            smithereens.txb_Nombre.Text = "Smithereens";
            ucContenido ng = new ucContenido("Cancion");
            ng.txb_Nombre.Text = "Neon Gravestones";
            ucContenido th = new ucContenido("Cancion");
            th.txb_Nombre.Text = "The Hype";
            ucContenido natn = new ucContenido("Cancion");
            natn.txb_Nombre.Text = "Nico and the Niners";
            ucContenido cml = new ucContenido("Cancion");
            cml.txb_Nombre.Text = "Cut My Lip";
            ucContenido bandito = new ucContenido("Cancion");
            bandito.txb_Nombre.Text = "Bandito";
            ucContenido pet = new ucContenido("Cancion");
            pet.txb_Nombre.Text = "Pet Cheetah";
            ucContenido legend = new ucContenido("Cancion");
            legend.txb_Nombre.Text = "Legend";
            ucContenido ltc = new ucContenido("Cancion");
            ltc.txb_Nombre.Text = "Leave the City";

            
            sp_Canciones.Children.Add(jumpsuit);
            sp_Canciones.Children.Add(levitate);
            sp_Canciones.Children.Add(morph);
            sp_Canciones.Children.Add(myblood);
            sp_Canciones.Children.Add(chlorine);
            sp_Canciones.Children.Add(smithereens);
            sp_Canciones.Children.Add(ng);
            sp_Canciones.Children.Add(th);
            sp_Canciones.Children.Add(natn);
            sp_Canciones.Children.Add(cml);
            sp_Canciones.Children.Add(bandito);
            sp_Canciones.Children.Add(pet);
            sp_Canciones.Children.Add(legend);
            sp_Canciones.Children.Add(ltc);


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
            //txb_Duracion.Text = album.
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

            //txb_Duracion.Text = album.
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
                    ucContenido contenido = new ucContenido(canciones, indice);
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
            NavigationService.GoBack();
        }

        private void Click_VerEstadisticas(object sender, RoutedEventArgs e)
        {
            //TODO
        }

        private void Click_EditarAlbum(object sender, RoutedEventArgs e)
        {
            //TODO
        }

        private void Click_GuardarAlbum(object sender, RoutedEventArgs e)
        {
            //TODO
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
    }
}
