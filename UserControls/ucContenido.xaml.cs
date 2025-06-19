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
        public List<BusquedaCancionDTO> listaCanciones { get; set; }
        public ListaDeReproduccionDTO lista { get; set; }
        public int indice { get; set; } = 0;
        public bool MostrarBotonGuardar { get; set; } = true;


        public ucContenido()
        {
            InitializeComponent();
            img_foto.Source = null;
        }

        public ucContenido(ListaDeReproduccionDTO lista, bool mostrarBotonGuardar)
        {
            InitializeComponent();
            this.tipo = "Lista";
            this.lista = lista;
            this.MostrarBotonGuardar = mostrarBotonGuardar;
            ConfigurarUserControl();
        }
        public ucContenido(List<BusquedaCancionDTO> canciones, int indice)
        {
            InitializeComponent();
            this.tipo = "Cancion";
            this.cancion = canciones[indice];
            this.listaCanciones = canciones;
            this.indice = indice;
            ConfigurarUserControl();
        }
        public ucContenido(BusquedaAlbumDTO album, bool mostrarBotonGuardar)
        {
            InitializeComponent();
            this.tipo = "Album";
            this.album = album;
            this.MostrarBotonGuardar = mostrarBotonGuardar;
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
        public ucContenido(BusquedaArtistaDTO artista, bool mostrarBotonGuardar)
        {
            InitializeComponent();
            this.tipo = "Artista";
            this.artista = artista;
            this.MostrarBotonGuardar = mostrarBotonGuardar;
            ConfigurarUserControl();
        }

        public ucContenido(String tipo, bool mostrarBotonGuardar)
        {
            InitializeComponent();
            this.tipo = tipo;
            this.MostrarBotonGuardar = mostrarBotonGuardar;
            ConfigurarUserControl();
        }

        public void ConfigurarUserControl()
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
                    btn_Reproducir.Visibility = Visibility.Collapsed;
                    CargarImagen(albumPendiente.urlFoto);
                    break;
                case "Cancion":
                    txb_Nombre.Text = cancion.nombre; 
                    txb_Autor.Text = cancion.nombreArtista;
                    CargarImagen(cancion.urlFoto);
                    break;
                case "Lista":
                    if (lista != null)
                    {
                        txb_Nombre.Text = lista.Nombre;
                        txb_Autor.Visibility = Visibility.Collapsed;
                        CargarImagen(lista.UrlFoto);
                    }
                    else
                    {
                        txb_Nombre.Text = "Lista de Reproducción";
                        txb_Autor.Visibility = Visibility.Collapsed;
                        img_foto.Source = new BitmapImage(new Uri("../Recursos/Iconos/iconoListaDeReproduccion.png", UriKind.Relative));
                    }

                    break;
                case "Artista":
                    txb_Nombre.Text = artista.nombre;
                    txb_Autor.Text = "@"+artista.nombreUsuario;
                    btn_Guardar.Content = "Seguir";
                    btn_Reproducir.Visibility = Visibility.Collapsed;
                    CargarImagen(artista.urlFoto);
                    break;

            }
            if (!MostrarBotonGuardar)
            {
                btn_Guardar.Visibility = Visibility.Collapsed;
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
                    NavigationService.GetNavigationService(this).Navigate(new vtListaDeReproduccion(lista));
                    break;
                case "Artista":
                    vtPerfilArtista vtPerfilArtista = new vtPerfilArtista(artista);
                    NavigationService.GetNavigationService(this).Navigate(vtPerfilArtista);
                    break;

            }
        }

        private async void Click_Guardar(object sender, RoutedEventArgs e)
        {
            var servicio = new ContenidoGuardadoServicio();

            var dto = new ContenidoGuardadoDTO
            {
                IdUsuario = SesionUsuario.IdUsuario,
                TipoDeContenido = tipo.ToUpper() // Asegúrate de enviar Album, Cancion, Artista, etc.
            };

            switch (tipo)
            {
                case "Album":
                    dto.IdContenidoGuardado = album.idAlbum;
                    break;
                case "Cancion":
                    dto.IdContenidoGuardado = cancion.idCancion;
                    break;
                case "Artista":
                    dto.IdContenidoGuardado = artista.idArtista;
                    break;
                case "Lista":
                    dto.IdContenidoGuardado = lista.IdListaDeReproduccion;
                    break;
                default:
                    MessageBox.Show("Tipo de contenido no reconocido.");
                    return;
            }

            var mensaje = await servicio.GuardarContenidoAsync(dto);

            if (!string.IsNullOrEmpty(mensaje))
            {
                MessageBox.Show(mensaje);

                if (mensaje == "Contenido guardado exitosamente")
                {
                    btn_Guardar.Visibility = Visibility.Collapsed;
                }
            }
        }

        private async void Click_Reproducir(object sender, RoutedEventArgs e)
        {
            switch (tipo)
            {
                case "Album":
                    await Reproductor.ReproducirCancionAsync(album.canciones, 0);
                    break;
                case "Lista":
                    //TODO
                    
                    break;
                case "Cancion":
                    await Reproductor.ReproducirCancionAsync(listaCanciones, indice);
                    break;
            }

        }
    }
}
