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

namespace ClienteMusAPI.Ventanas.Menu
{
    /// <summary>
    /// Lógica de interacción para vtReportes.xaml
    /// </summary>
    public partial class vtReportes : Page
    {
        public vtReportes()
        {
            InitializeComponent();

            CargarDatos();
        }

        private void CargarDatos()
        {
            switch (cb_tipo.SelectedIndex)
            {
                case 0:
                    grd_usuarios.Visibility = Visibility.Visible;
                    grd_top.Visibility = Visibility.Collapsed;
                    CargarUsuarios();
                    break;
                case 1:
                    lbl_tituloTop.Content = "Canciones más escuchadas";
                    grd_top.Visibility = Visibility.Visible;
                    grd_usuarios.Visibility = Visibility.Collapsed;
                    CargarCanciones();
                    break;
                case 2:
                    lbl_tituloTop.Content = "Artistas más escuchados";
                    grd_top.Visibility = Visibility.Visible;
                    grd_usuarios.Visibility = Visibility.Collapsed;
                    CargarArtistas();
                    break;
                default:
                    return;
            }
        }

        private async void CargarUsuarios()
        { 
            EstadisticasServicio servicio = new EstadisticasServicio();
            EstadisticasNumeroUsuariosDTO estadisticas = await servicio.obtenerEstadisticasUsuarios();

            if(estadisticas != null)
            {
                lbl_totalArtistas.Content = estadisticas.totalArtistas;
                lbl_totalUsuarios.Content = estadisticas.totalUsuarios + estadisticas.totalArtistas;
                lbl_totalOyentes.Content = estadisticas.totalUsuarios;
            }
            
        }

        private async void CargarArtistas()
        {
            if (!dp_FechaInicio.SelectedDate.HasValue || !dp_FechaFin.SelectedDate.HasValue)
            {
                tbc_top.Text = "No hay artistas escuchados en el periodo seleccionado.";
                return;
            }
            string fechaInicio = dp_FechaInicio.SelectedDate.Value.ToString("yyyy-MM-dd");
            string fechaFin = dp_FechaFin.SelectedDate.Value.ToString("yyyy-MM-dd");
            EstadisticasServicio servicio = new EstadisticasServicio();
            List<ArtistaMasEscuchadoDTO> estadisticas = await servicio.obtenerEstadisticasArtistas(fechaInicio, fechaFin);
            

            if (estadisticas.Count != 0 && estadisticas != null)
            {
                var top = new StringBuilder();
                int posicion = 1;
                
                foreach (var artista in estadisticas)
                {
                    long minutos = artista.segundosEscuchados / 60;
                    long segundosRestantes = artista.segundosEscuchados % 60;
                    top.AppendLine($"{posicion}. {artista.nombreArtista} - {minutos}m {segundosRestantes}s\n");
                    posicion++;
                }
                tbc_top.Text = top.ToString();

            } else
            {
                tbc_top.Text = "No hay artistas escuchados en el periodo seleccionado.";
            }
        }

        private async void CargarCanciones()
        {
            if (!dp_FechaInicio.SelectedDate.HasValue || !dp_FechaFin.SelectedDate.HasValue)
            {
                tbc_top.Text = "No hay canciones escuchadas en el periodo seleccionado.";
                return;
            }
            string fechaInicio = dp_FechaInicio.SelectedDate.Value.ToString("yyyy-MM-dd");
            string fechaFin = dp_FechaFin.SelectedDate.Value.ToString("yyyy-MM-dd");
            EstadisticasServicio servicio = new EstadisticasServicio();
            List<CancionMasEscuchadaDTO> estadisticas = await servicio.obtenerEstadisticasCanciones(fechaInicio, fechaFin);


            if (estadisticas.Count != 0 && estadisticas != null)
            {
                var top = new StringBuilder();
                int posicion = 1;
                foreach (var cancion in estadisticas)
                {
                    long minutos = cancion.segundosEscuchados / 60;
                    long segundosRestantes = cancion.segundosEscuchados % 60;
                    top.AppendLine($"{posicion}. {cancion.nombreCancion} - {minutos}m {segundosRestantes}s\n{cancion.nombreUsuarioArtista}\n");
                    posicion++;
                }
                tbc_top.Text = top.ToString();

            }
            else
            {
                tbc_top.Text = "No hay canciones escuchadas en el periodo seleccionado.";
            }
        }

        private void dp_FechaInicio_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            CargarDatos();
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
