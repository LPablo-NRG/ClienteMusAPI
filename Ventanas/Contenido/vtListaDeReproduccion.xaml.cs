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
    /// Lógica de interacción para vtListaDeReproduccion.xaml
    /// </summary>
    public partial class vtListaDeReproduccion : Page
    {
        private ListaDeReproduccionDTO lista;
        bool mostrarBotonGuardar = true;
        bool mostrarBotonEliminar = false;

        public vtListaDeReproduccion(ListaDeReproduccionDTO lista)
        {
            InitializeComponent();
            this.lista = lista;
            this.Loaded += vtListaDeReproduccion_Loaded;
        }

        private void vtListaDeReproduccion_Loaded(object sender, RoutedEventArgs e)
        {
            CargarDatos();
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

        private void CargarDatos()
        {
            txb_Nombre.Text = lista.Nombre;
            txb_Descripcion.Text = "Descripción: " + lista.Descripcion;
            txb_Autor.Text = "Autor: "+SesionUsuario.Nombre;
            CargarImagen(lista.UrlFoto);

            if (lista.Canciones != null)
            {
                int indice = 0;
                TimeSpan duracionTotal = TimeSpan.Zero;
                sp_Canciones.Children.Clear();
                foreach (var cancion in lista.Canciones)
                {
                    sp_Canciones.Children.Add(new ucContenido(lista.Canciones, indice, mostrarBotonGuardar, mostrarBotonEliminar));
                    //sp_Canciones.Children.Add(new ucContenido(lista.Canciones, indice));
                    indice++;
                    TimeSpan duracionRecuperada = TimeSpan.ParseExact(cancion.duracion, @"mm\:ss", null);
                    duracionTotal += duracionRecuperada;
                }
                txb_Duracion.Text = duracionTotal.ToString(@"hh\:mm\:ss");
            }
        }

        private void Click_Volver(object sender, RoutedEventArgs e)
        {
            vtPerfilUsuario vtPerfilUsuario = new vtPerfilUsuario();
            NavigationService.GetNavigationService(this).Navigate(vtPerfilUsuario);
        }

        private async void Click_GuardarListaDeReproduccion(object sender, RoutedEventArgs e)
        {
            var servicio = new ContenidoGuardadoServicio();
            var dto = new ContenidoGuardadoDTO
            {
                IdUsuario = SesionUsuario.IdUsuario,
                TipoDeContenido = "LISTA",
                IdContenidoGuardado = lista.IdListaDeReproduccion
            };

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

        private void Click_VerEstadisticas(object sender, RoutedEventArgs e)
        {
            // O dejar en blanco si no aplica aún
        }

        private void Click_EditarListaDeReproduccion(object sender, RoutedEventArgs e)
        {
            vtCrearLista vtCrearLista = new vtCrearLista(lista);
            NavigationService.GetNavigationService(this).Navigate(vtCrearLista);
        }
    }

}
