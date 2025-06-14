using ClienteMusAPI.Clases;
using ClienteMusAPI.DTOs;
using ClienteMusAPI.Servicios;
using ClienteMusAPI.Ventanas.Contenido;
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

namespace ClienteMusAPI.UserControls
{
    /// <summary>
    /// Lógica de interacción para Contenido.xaml
    /// </summary>
    public partial class ucContenido : UserControl
    {
        public String tipo { get; set; }
        public int idArtista { get; set; }
        public InfoAlbumDTO albumPendiente { get; set; }
        public BusquedaAlbumDTO album { get; set; }
        public BusquedaCancionDTO cancion { get; set; }
        public BusquedaArtistaDTO artista { get; set; }

        public ucContenido()
        {
            InitializeComponent();
            img_foto.Source = null;
        }
        public ucContenido(BusquedaCancionDTO cancion)
        {
            InitializeComponent();
            this.tipo = "Cancion";
            this.cancion = cancion;
            ConfigurarUserControl();
        }
        public ucContenido(BusquedaAlbumDTO album)
        {
            InitializeComponent();
            this.tipo = "Album";
            this.album = album;
            ConfigurarUserControl();
        }
        public ucContenido(InfoAlbumDTO albumPendiente, int idArtista)
        {
            InitializeComponent();
            this.tipo = "Album Pendiente";
            this.albumPendiente = albumPendiente;
            this.idArtista = idArtista;
            ConfigurarUserControl();
        }
        public ucContenido(int lista) //gcahvbdskjlhvbjhaskdfhbsndajskkbhjkdsaljbnadskjdsbn TODO:
        {
            InitializeComponent();
            this.tipo = "Lista";
            ConfigurarUserControl();
        }
        public ucContenido(BusquedaArtistaDTO artista)
        {
            InitializeComponent();
            this.tipo = "Artista";
            this.artista = artista;
            ConfigurarUserControl();
        }

        public ucContenido(String tipo)
        {
            InitializeComponent();
            this.tipo = tipo;
            //ConfigurarUserControl();
        }

        private void ConfigurarUserControl()
        {
            switch (tipo)
            {
                case "Album":
                    txb_Nombre.Text = album.nombreAlbum;
                    txb_Autor.Text = album.nombreArtista;
                    CargarImagen(album.urlFoto);
                    break;
                case "Album Pendiente":
                    txb_Nombre.Text = albumPendiente.nombre;
                    txb_Autor.Visibility = Visibility.Collapsed;
                    btn_Guardar.Visibility = Visibility.Collapsed;
                    CargarImagen(albumPendiente.urlFoto);
                    break;
                case "Cancion":
                    txb_Nombre.Text = cancion.nombre; 
                    txb_Autor.Text = cancion.nombreArtista;
                    CargarImagen(cancion.urlFoto);
                    break;
                case "Lista":
                    txb_Nombre.Text = "Lista de Reproducción";
                    txb_Autor.Visibility = Visibility.Collapsed;
                    btn_Guardar.Visibility = Visibility.Collapsed;
                    img_foto.Source = new BitmapImage(new Uri("../Recursos/Iconos/iconoListaDeReproduccion.png", UriKind.Relative));
                    break;
                case "Artista":
                    txb_Nombre.Text = artista.nombre;
                    txb_Autor.Text = "@"+artista.nombreUsuario;
                    btn_Guardar.Visibility = Visibility.Collapsed;
                    btn_Reproducir.Visibility = Visibility.Collapsed;
                    CargarImagen(artista.urlFoto);
                    break;

            }
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
                    img_foto.Source = image;
                }
            }
        }

        private void Click_VerDetalles(object sender, RoutedEventArgs e)
        {
            switch (tipo)
            {
                case "Album":
                    vtAlbum vtAlbum = new vtAlbum(album);
                    NavigationService.GetNavigationService(this).Navigate(vtAlbum);
                    break;
                case "Album Pendiente":
                    vtAlbum = new vtAlbum(albumPendiente, idArtista);
                    NavigationService.GetNavigationService(this).Navigate(vtAlbum);
                    break;
                case "Cancion":
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
                            ucVentanaDetalles detalles = new ucVentanaDetalles(cancion);
                            detalles.btn_Cerrar.Click += (object sender2, RoutedEventArgs e2) =>
                            {
                                contenedor.Children.Remove(detalles);
                            };
                            contenedor.Children.Add(new ucVentanaDetalles(cancion));
                        }
                    }
                    break;

                case "Lista":
                    NavigationService.GetNavigationService(this).Navigate(new Uri("/Ventanas/Contenido/vtListaDeReproduccion.xaml", UriKind.Relative));
                    break;
                case "Artista":
                    vtPerfilArtista vtPerfilArtista = new vtPerfilArtista(artista);
                    NavigationService.GetNavigationService(this).Navigate(vtPerfilArtista);
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
