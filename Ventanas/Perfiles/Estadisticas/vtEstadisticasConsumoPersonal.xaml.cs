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
    /// Lógica de interacción para vtEstadisticasConsumoPersonal.xaml
    /// </summary>
    public partial class vtEstadisticasConsumoPersonal : Page
    {
        private int idUsuario = -1;
        public vtEstadisticasConsumoPersonal(int idUsuario)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            CargarDatos();
        }

        private async void CargarDatos()
        {
            if (!dp_FechaInicio.SelectedDate.HasValue || !dp_FechaFin.SelectedDate.HasValue)
            {
                return;
            }
            string fechaInicio = dp_FechaInicio.SelectedDate.Value.ToString("yyyy-MM-dd");
            string fechaFin = dp_FechaFin.SelectedDate.Value.ToString("yyyy-MM-dd");
            EstadisticasServicio servicio = new EstadisticasServicio();
            EstadisticasPersonalesDTO estadisticas = await servicio.obtenerEstadisticasPersonales(idUsuario, fechaInicio, fechaFin);
            if (estadisticas != null)
            {
                if (estadisticas.topArtistas.Count != 0)
                {
                    var textoArtistas = new StringBuilder();
                    int posicionArtistas = 1;
                    foreach (var artista in estadisticas.topArtistas)
                    {
                        textoArtistas.AppendLine($"{posicionArtistas}. {artista}");
                        posicionArtistas++;
                    }
                    tbc_Artistas.Text = textoArtistas.ToString();
                }
                if (estadisticas.topCanciones.Count != 0)
                {
                    var textoCanciones = new StringBuilder();
                    int posicionCanciones = 1;
                    foreach (var cancion in estadisticas.topCanciones)
                    {
                        textoCanciones.AppendLine($"{posicionCanciones}. {cancion}");
                        posicionCanciones++;
                    }
                    tbc_Canciones.Text = textoCanciones.ToString();

                }
            }
            
            long minutos = estadisticas.segundosEscuchados / 60;

            string tiempoEnMinutos = $"{minutos} minutos";

            long segundosRestantes = estadisticas.segundosEscuchados % 60;
            string tiempoConSegundos = $"{minutos} minutos y {segundosRestantes} segundos";
            lbl_tiempo.Content = tiempoConSegundos;
        }

        private void Click_Volver(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void dp_FechaInicio_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            CargarDatos();
        }
    }
}
