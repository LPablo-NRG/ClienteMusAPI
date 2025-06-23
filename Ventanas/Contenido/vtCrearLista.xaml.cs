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

namespace ClienteMusAPI.Ventanas.Contenido
{
    /// <summary>
    /// Lógica de interacción para vtCrearLista.xaml
    /// </summary>
    public partial class vtCrearLista : Page
    {
        private bool esEdicion = false;
        private int idListaEditar = -1;
        private ListaDeReproduccionDTO lista = new ListaDeReproduccionDTO();
        public vtCrearLista()
        {
            InitializeComponent();
        }

        public vtCrearLista(ListaDeReproduccionDTO lista)
        {
            InitializeComponent();
            this.lista = lista;
            CargarDatosParaEditar(lista);
        }

        public void CargarDatosParaEditar(ListaDeReproduccionDTO lista)
        {
            esEdicion = true;
            idListaEditar = lista.IdListaDeReproduccion;

            lb_Titulo.Content = "Editar Lista";
            txb_NombreLista.Text = lista.Nombre;
            txb_DescripcionLista.Text = lista.Descripcion;
            CargarImagen(lista.UrlFoto);
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
                    img_PortadaLista.Source = image;
                }
            }
        }

        private string rutaImagen = null;

        private void Click_SubirFoto(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Imágenes (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
            };

            if (dlg.ShowDialog() == true)
            {
                try
                {
                    rutaImagen = dlg.FileName;

                    var imagen = new BitmapImage();
                    imagen.BeginInit();
                    imagen.UriSource = new Uri(rutaImagen);
                    imagen.CacheOption = BitmapCacheOption.OnLoad;
                    imagen.EndInit();

                    img_PortadaLista.Source = imagen;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrió un error al cargar la imagen.");
                }
            }

        }

        private async void Click_Guardar(object sender, RoutedEventArgs e)
        {
            var dto = new ListaReproduccionDTO
            {
                Nombre = txb_NombreLista.Text,
                Descripcion = txb_DescripcionLista.Text,
                FotoPath = rutaImagen
            };
            if (!esEdicion)
            {
                dto.IdUsuario = SesionUsuario.IdUsuario;
                var servicio = new ListaServicio();
                bool exito = await servicio.CrearListaReproduccionAsync(dto);

                if (exito)
                {
                    MessageBox.Show("Lista creada exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.GoBack();
                }
                else
                {
                    MessageBox.Show("No se pudo crear la lista.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                dto.IdUsuario = idListaEditar;
                var servicio = new ListaServicio();
                bool exito = await servicio.EditarListaAsync(dto);
                if (exito)
                {
                    MessageBox.Show("Lista editada exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    vtPerfilUsuario vtPerfilUsuario = new vtPerfilUsuario();
                    NavigationService.GetNavigationService(this).Navigate(vtPerfilUsuario);
                }
                else
                {
                    MessageBox.Show("No se pudo editar la lista.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

    }


}
