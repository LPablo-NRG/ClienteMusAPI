using ClienteMusAPI.Clases;
using ClienteMusAPI.DTOs;
using ClienteMusAPI.Modelo;
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
        public bool MostrarBotonGuardar { get; set; } = false;
        public bool MostrarBotonEliminar { get; set; } = false;
        public BusquedaUsuarioDTO usuario { get; set; }


        public ucContenido()
        {
            InitializeComponent();
            img_foto.Source = null;
        }

        public ucContenido(BusquedaUsuarioDTO usuario, bool mostrarBotonGuardar, bool mostrarBotonEliminar)
        {
            InitializeComponent();
            this.tipo = "Usuario";
            this.usuario = usuario;
            this.MostrarBotonGuardar = mostrarBotonGuardar;
            this.MostrarBotonEliminar = mostrarBotonEliminar;
            ConfigurarUserControl();

            txb_Nombre.Text = usuario.nombre;
            txb_Autor.Text = usuario.nombreUsuario + " • " + usuario.pais;
            img_foto.Source = new BitmapImage(new Uri("/Recursos/Iconos/iconoPerfil.png", UriKind.Relative));
        }

        public void CargarUsuario(BusquedaUsuarioDTO usuario, bool mostrarBotonGuardar, bool mostrarBotonEliminar)
        {
            InitializeComponent();
            this.tipo = "Usuario";
            this.usuario = usuario;
            this.MostrarBotonGuardar = mostrarBotonGuardar;
            this.MostrarBotonEliminar = mostrarBotonEliminar;
            ConfigurarUserControl();

            txb_Nombre.Text = usuario.nombre;
            txb_Autor.Text = usuario.nombreUsuario + " • " + usuario.pais;
            img_foto.Source = new BitmapImage(new Uri("/Recursos/Iconos/iconoPerfil.png", UriKind.Relative));
        }

        public ucContenido(ListaDeReproduccionDTO lista, bool mostrarBotonGuardar, bool mostrarBotonEliminar)
        {
            InitializeComponent();
            this.tipo = "Lista";
            this.lista = lista;
            this.MostrarBotonGuardar = mostrarBotonGuardar;
            this.MostrarBotonEliminar = mostrarBotonEliminar;
            ConfigurarUserControl();
        }
        public ucContenido(List<BusquedaCancionDTO> canciones, int indice, bool mostrarBotonGuardar, bool mostrarBotonEliminar)
        {
            InitializeComponent();
            this.tipo = "Cancion";
            this.cancion = canciones[indice];
            this.listaCanciones = canciones;
            this.indice = indice;
            this.MostrarBotonGuardar = mostrarBotonGuardar;
            this.MostrarBotonEliminar = mostrarBotonEliminar;
            ConfigurarUserControl();
        }
        public ucContenido(BusquedaAlbumDTO album, bool mostrarBotonGuardar, bool MostrarBotonEliminar)
        {
            InitializeComponent();
            this.tipo = "Album";
            this.album = album;
            this.MostrarBotonGuardar = mostrarBotonGuardar;
            this.MostrarBotonEliminar = MostrarBotonEliminar;
            ConfigurarUserControl();
        }
        public ucContenido(InfoAlbumDTO albumPendiente, int idArtista, bool mostrarBotonGuardar, bool mostrarBotonEliminar)
        {
            InitializeComponent();
            this.tipo = "Album Pendiente";
            this.albumPendiente = albumPendiente;
            this.idArtista = idArtista;
            this.MostrarBotonGuardar = mostrarBotonGuardar;
            this.MostrarBotonEliminar = mostrarBotonEliminar;
            ConfigurarUserControl();
        }
        public ucContenido(int lista) //gcahvbdskjlhvbjhaskdfhbsndajskkbhjkdsaljbnadskjdsbn TODO:
        {
            InitializeComponent();
            this.tipo = "Lista";
            ConfigurarUserControl();
        }
        public ucContenido(BusquedaArtistaDTO artista, bool mostrarBotonGuardar, bool mostrarBotonEliminar)
        {
            InitializeComponent();
            this.tipo = "Artista";
            this.artista = artista;
            this.MostrarBotonGuardar = mostrarBotonGuardar;
            this.MostrarBotonEliminar = mostrarBotonEliminar;
            ConfigurarUserControl();
        }

        public ucContenido(String tipo, bool mostrarBotonGuardar, bool mostrarBotonEliminar)
        {
            InitializeComponent();
            this.tipo = tipo;
            this.MostrarBotonGuardar = mostrarBotonGuardar;
            this.MostrarBotonEliminar = mostrarBotonEliminar;
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
                    btn_Eliminar.Visibility = Visibility.Collapsed;
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

            if (!MostrarBotonEliminar)
            {
                btn_Eliminar.Visibility = Visibility.Collapsed;
            }
        }

        private async void CargarImagen(string url)
        {

            if (!String.IsNullOrEmpty(url))
            {
                if (tipo == "Cancion")
                {
                    Console.WriteLine("cancion con foto en: " + url);
                }
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
            else
            {
                if (tipo == "Cancion")
                {
                    Console.WriteLine("cancion con foto nula");
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
                case "Usuario":
                    DependencyObject parentUsuario = this;
                    while (parentUsuario != null && !(parentUsuario is Window))
                    {
                        parentUsuario = VisualTreeHelper.GetParent(parentUsuario);
                    }

                    if (parentUsuario is Window windowUsuario)
                    {
                        var contenedor = (windowUsuario as VentanaPrincipal)?.Contenido;
                        if (contenedor != null)
                        {
                            ucVentanaDetalles detallesUsuario = new ucVentanaDetalles(usuario);
                            detallesUsuario.btn_Cerrar.Click += (object sender2, RoutedEventArgs e2) =>
                            {
                                contenedor.Children.Remove(detallesUsuario);
                            };
                            contenedor.Children.Add(detallesUsuario);
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
            if (tipo == "Cancion")
            {
                var listaServicio = new ListaServicio();
                var listas = await listaServicio.ObtenerListasPorUsuarioAsync(SesionUsuario.IdUsuario);

                if (listas == null || listas.Count == 0)
                {
                    MessageBox.Show("No tienes listas de reproducción. Crea una para agregar canciones.");
                    vtCrearLista vtCrearLista = new vtCrearLista();
                    NavigationService.GetNavigationService(this).Navigate(vtCrearLista);
                    return;
                }

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
                        var popup = new ucSeleccionarListaPopup(listas);

                        popup.ListaSeleccionada += async (s, listaSeleccionada) =>
                        {
                            Console.WriteLine($"IdCancion: {cancion.idCancion}");
                            Console.WriteLine($"IdListaDeReproduccion: {listaSeleccionada.IdListaDeReproduccion}");
                            Console.WriteLine($"IdUsuario: {SesionUsuario.IdUsuario}");
                            var exito = await listaServicio.AgregarCancionAListaAsync(new ListaDeReproduccion_CancionDTO
                            {
                                IdCancion = cancion.idCancion,
                                IdListaDeReproduccion = listaSeleccionada.IdListaDeReproduccion,
                                IdUsuario = SesionUsuario.IdUsuario
                            });

                            MessageBox.Show(exito ? "Canción agregada a la lista." : "Error al agregar la canción");
                            contenedor.Children.Remove(popup);
                        };

                        if (!contenedor.Children.Contains(popup))
                        {
                            contenedor.Children.Add(popup);
                        }

                    }
                }

            }
            else
            {
                var servicio = new ContenidoGuardadoServicio();
                var dto = new ContenidoGuardadoDTO
                {
                    IdUsuario = SesionUsuario.IdUsuario,
                    TipoDeContenido = tipo.ToUpper()
                };

                switch (tipo)
                {
                    case "Album": dto.IdContenidoGuardado = album.idAlbum; break;
                    case "Artista": dto.IdContenidoGuardado = artista.idArtista; break;
                    case "Lista": dto.IdContenidoGuardado = lista.IdListaDeReproduccion; break;
                    default: MessageBox.Show("Tipo de contenido no reconocido."); return;
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
            
        }

        private async void Click_Reproducir(object sender, RoutedEventArgs e)
        {
            switch (tipo)
            {
                case "Album":
                    await Reproductor.ReproducirCancionAsync(album.canciones, 0);
                    break;
                case "Lista":
                    await Reproductor.ReproducirCancionAsync(lista.Canciones, 0);
                    break;

                    break;
                case "Cancion":
                    await Reproductor.ReproducirCancionAsync(listaCanciones, indice);
                    break;
            }

        }

        private async void Click_Eliminar(object sender, RoutedEventArgs e)
        {
            switch (tipo) 
            {
                case "Album":
                    var resultadoAlbum = MessageBox.Show("¿Estás seguro de que deseas eliminar este album?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (resultadoAlbum == MessageBoxResult.Yes)
                    {
                        AlbumServicio servicio = new AlbumServicio();
                        bool exito = await servicio.EliminarAlbumAsync(album.idAlbum);
                        if (exito)
                        {
                            MessageBox.Show("Album eliminado correctamente.");
                            this.Visibility = Visibility.Collapsed;
                        }
                    }
                    break;
                case "Album Pendiente":
                    break;
                case "Cancion":
                    var resultadoCancion = MessageBox.Show("¿Estás seguro de que deseas eliminar esta canción?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (resultadoCancion == MessageBoxResult.Yes)
                    {
                        CancionServicio servicio = new CancionServicio();
                        bool exito = await servicio.EliminarCancionAsync(cancion.idCancion);
                        if (exito)
                        {
                            MessageBox.Show("Canción eliminada correctamente.");
                            this.Visibility = Visibility.Collapsed;
                        }
                    }
                    break;
                case "Lista":
                    break;
                case "Usuario":
                    vtEliminarUsuario vtEliminarUsuario = new vtEliminarUsuario(usuario);
                    NavigationService.GetNavigationService(this).Navigate(vtEliminarUsuario);
                    break;
                default:
                    MessageBox.Show("Tipo de contenido no reconocido para eliminar.");
                    break;
            }

        }
    }
}
