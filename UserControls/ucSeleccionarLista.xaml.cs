using ClienteMusAPI.Clases;
using ClienteMusAPI.DTOs;
using ClienteMusAPI.Servicios;
using System;
using System.Collections;
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
    /// Lógica de interacción para ucListasCreadas.xaml
    /// </summary>
    public partial class ucSeleccionarLista : UserControl
    {
        public ListaDeReproduccionDTO Lista { get; set; }
        public event EventHandler<ListaDeReproduccionDTO> ListaSeleccionada;
        public ucSeleccionarLista(ListaDeReproduccionDTO lista)
        {
            InitializeComponent();
            Lista = lista;
            txb_Nombre.Text = lista.Nombre;
            txb_Descripcion.Text = lista.Descripcion;
            CargarImagen(lista.UrlFoto);
        }

        private void SeleccionarLista(object sender, MouseButtonEventArgs e)
        {
            ListaSeleccionada?.Invoke(this, Lista);
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
    }
}
