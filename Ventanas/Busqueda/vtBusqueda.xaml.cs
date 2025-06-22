using ClienteMusAPI.Clases;
using ClienteMusAPI.DTOs;
using ClienteMusAPI.Modelo;
using ClienteMusAPI.Servicios;
using ClienteMusAPI.UserControls;
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

namespace ClienteMusAPI.Ventanas.Busqueda
{
    /// <summary>
    /// Lógica de interacción para vtBusqueda.xaml
    /// </summary>
    public partial class vtBusqueda : Page
    {
        bool mostrarBotonGuardar = true;
        bool mostrarBotonEliminar = false;
        public vtBusqueda()
        {
            InitializeComponent();

        }

        public vtBusqueda(string textoBusqueda, bool buscarUsuarios)
        {
            InitializeComponent();
            txb_Busqueda.Text = textoBusqueda;

            if (buscarUsuarios)
            {
                cb_tipo.Visibility = Visibility.Collapsed;
                btn_Buscar.Visibility = Visibility.Collapsed;

                BuscarUsuarios(textoBusqueda);
            }
        }


        public vtBusqueda(string busqueda)
        {
            InitializeComponent();
            txb_Busqueda.Text = busqueda;

            var sender = this; 
            var e = new RoutedEventArgs();

            Click_BuscarContenido(sender, e);

        }

        private async void BuscarUsuarios(string texto)
        {
            sp_Resultados.Children.Clear();
            mostrarBotonEliminar = true;
            mostrarBotonGuardar = false;

            UsuarioServicio usuarioServicio = new UsuarioServicio();
            List<BusquedaUsuarioDTO> usuarios = await usuarioServicio.BuscarUsuarioAsync(texto, SesionUsuario.IdUsuario);

            if (usuarios != null && usuarios.Count > 0)
            {
                foreach (var usuario in usuarios)
                {
                    ucContenido contenido = new ucContenido(usuario, mostrarBotonGuardar, mostrarBotonEliminar);
                    sp_Resultados.Children.Add(contenido);
                }
            }
            else
            {
                MessageBox.Show("No se encontraron usuarios llamados: " + texto);
            }
        }


        private void Click_Volver(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Click_VerPerfil(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Ventanas/Perfiles/vtPerfilArtista.xaml", UriKind.Relative));
        }

        private void Click_VerPerfilUsuario(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Ventanas/Perfiles/vtPerfilUsuario.xaml", UriKind.Relative));
        }

        private async void Click_BuscarContenido(object sender, RoutedEventArgs e)
        {
            sp_Resultados.Children.Clear();
            switch (cb_tipo.Text)
            {
                case "Artista":
                    UsuarioServicio usuarioServicio = new UsuarioServicio();
                    List<BusquedaArtistaDTO> artistas = await usuarioServicio.BuscarArtista(txb_Busqueda.Text);

                    if (artistas != null)
                    {
                        foreach (var artista in artistas)
                        {
                            ucContenido contenido = new ucContenido(artista, mostrarBotonGuardar, mostrarBotonEliminar);
                            sp_Resultados.Children.Add(contenido);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se encontraron resultados para la búsqueda de artistas llamados" + txb_Busqueda.Text);
                    }
                    break;
                case "Usuario":
                    //NavigationService.Navigate(new Uri("/Ventanas/Perfiles/vtPerfilUsuario.xaml", UriKind.Relative));
                    break;
                case "Canción":
                    CancionServicio cancionServicio = new CancionServicio();
                    List<BusquedaCancionDTO> canciones = await cancionServicio. BuscarCancion(txb_Busqueda.Text);

                    if (canciones != null)
                    {
                        foreach (var cancion in canciones)
                        {
                            ucContenido contenido = new ucContenido(new List<BusquedaCancionDTO> { cancion }, 0, mostrarBotonGuardar, mostrarBotonEliminar);
                            sp_Resultados.Children.Add(contenido);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se encontraron resultados para la búsqueda de canciones llamadas " + txb_Busqueda.Text);
                    }
                    break;
                case "Álbum":
                    AlbumServicio albumServicio = new AlbumServicio();
                    List<BusquedaAlbumDTO> albumes = await albumServicio.BuscarAlbum(txb_Busqueda.Text);
                    if (albumes != null)
                    {
                        foreach (var album in albumes)
                        {
                            ucContenido contenido = new ucContenido(album, mostrarBotonGuardar, mostrarBotonEliminar);
                            sp_Resultados.Children.Add(contenido);
                        }
                    }else
                    {
                        MessageBox.Show("No se encontraron resultados para la búsqueda de álbumes llamados "+txb_Busqueda.Text);
                    }
                    break;
            }
        }


        private void cb_tipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sp_Resultados == null) return; 
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Click_BuscarContenido(sender, new RoutedEventArgs());
            }), System.Windows.Threading.DispatcherPriority.Input);
        }
    }
}
