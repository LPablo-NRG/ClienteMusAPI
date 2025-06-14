using ClienteMusAPI.Clases;
using ClienteMusAPI.DTOs;
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
        public vtBusqueda()
        {
            InitializeComponent();
            ucContenido twenty = new ucContenido("Artista");
            twenty.txb_Nombre.Text = "Twenty One Pilots";
            ucContenido muse = new ucContenido("Artista");
            muse.txb_Nombre.Text = "Muse";
            ucContenido idkh = new ucContenido("Artista");
            idkh.txb_Nombre.Text = "I Don't Know How But They Found Me";
            idkh.txb_Nombre.MaxWidth = 800;
            ucContenido ejemplolargo = new ucContenido("Artista");
            ejemplolargo.txb_Nombre.Text = "Este es un ejemplo de un nombre de artista que es muy largo y debería ajustarse correctamente en la interfaz de usuario para no desbordar el diseño.";
            //ejemplolargo.MaxWidth = sp_Resultados.MaxWidth;
            ejemplolargo.sp_Texto.MaxWidth = 500;

            ucContenido cancionlarga = new ucContenido("Cancion");
            cancionlarga.txb_Nombre.Text = "Esta es una canción con un nombre muy largo que debería ajustarse correctamente en la interfaz de usuario para no desbordar el diseño.";
            cancionlarga.txb_Autor.Text = "Artista muy exageradamente largo pero que no creo que baya a ocurrir en la base de datos real porque tiene un limite de caractereshacerato escribir baya muy mal";
            cancionlarga.sp_Texto.MaxWidth = 500;

            sp_Resultados.Children.Add(cancionlarga);
            sp_Resultados.Children.Add(twenty);
            sp_Resultados.Children.Add(muse);
            sp_Resultados.Children.Add(idkh);
            sp_Resultados.Children.Add(ejemplolargo);
            sp_Resultados.Children.Add(new ucContenido());
            sp_Resultados.Children.Add(new ucContenido("Cancion"));
            sp_Resultados.Children.Add(new ucContenido("Lista"));
            sp_Resultados.Children.Add(new ucContenido("Album"));
            sp_Resultados.Children.Add(new ucContenido("Artista"));

        }

        public vtBusqueda(string busqueda)
        {
            InitializeComponent();
            txb_Busqueda.Text = busqueda;

            var sender = this; 
            var e = new RoutedEventArgs();

            Click_BuscarContenido(sender, e);

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
            //TODO: Implementar la lógica de búsqueda
            switch (cb_tipo.Text)
            {
                case "Artista":
                    UsuarioServicio usuarioServicio = new UsuarioServicio();
                    List<BusquedaArtistaDTO> artistas = await usuarioServicio.BuscarArtista(txb_Busqueda.Text);

                    if (artistas != null)
                    {
                        foreach (var artista in artistas)
                        {
                            ucContenido contenido = new ucContenido(artista);
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
                            ucContenido contenido = new ucContenido(cancion);
                            sp_Resultados.Children.Add(contenido);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se encontraron resultados para la búsqueda de canciones llamadas " + txb_Busqueda.Text);
                    }
                    break;
                case "Album":
                    AlbumServicio albumServicio = new AlbumServicio();
                    List<BusquedaAlbumDTO> albumes = await albumServicio.BuscarAlbum(txb_Busqueda.Text);
                    if (albumes != null)
                    {
                        foreach (var album in albumes)
                        {
                            ucContenido contenido = new ucContenido(album);
                            sp_Resultados.Children.Add(contenido);
                        }
                    }else
                    {
                        MessageBox.Show("No se encontraron resultados para la búsqueda de álbumes llamados "+txb_Busqueda.Text);
                    }
                    break;
            }
        }
    }
}
