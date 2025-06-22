using ClienteMusAPI.DTOs;
using ClienteMusAPI.Servicios;
using System;
using System.Collections.Generic;
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

namespace ClienteMusAPI.Ventanas.Perfiles.Estadisticas
{
    /// <summary>
    /// Lógica de interacción para vtEstadisticasContenidoSubido.xaml
    /// </summary>
    public partial class vtEstadisticasContenidoSubido : Page
    {
        private int idArtista = -1;
        public vtEstadisticasContenidoSubido(int idArtista)
        {
            InitializeComponent();
            this.idArtista = idArtista;
            CargarDatos();
        }

        private async void CargarDatos()
        {
            EstadisticasServicio servicio = new EstadisticasServicio();
            String tipo = null;
            switch (cb_tipo.SelectedIndex)
            {
                case 0:
                    tipo = "Cancion";
                    break;
                case 1:
                    tipo = "Album";
                    break;
                default:
                    return;
            }
            EstadisticasContenidoSubidoDTO estadisticas = await servicio.obtenerEstadisticasContenidoSubido(idArtista, tipo);
            lbl_totalEscuchas.Content = estadisticas.numeroOyentes;
            lbl_totalGuardados.Content = estadisticas.numeroGuardados;
        }

        private void cb_tipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_tipo == null) return;
            Dispatcher.BeginInvoke(new Action(() =>
            {
                CargarDatos();
            }), System.Windows.Threading.DispatcherPriority.Input);
        }

        private void Click_Volver(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
