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

namespace ClienteMusAPI.UserControls
{
    /// <summary>
    /// Lógica de interacción para ucNotificacion.xaml
    /// </summary>
    public partial class ucNotificacion : UserControl
    {
        private int idNotificacion;

        public ucNotificacion()
        {
            InitializeComponent();
        }

        public void SetNotificacion(NotificacionDTO dto)
        {
            idNotificacion = dto.idNotificacion;
            txb_Mensaje.Text = dto.mensaje;
            txb_Fecha.Text = dto.fechaEnvio;
        }

        private async void Click_MarcarLeida(object sender, System.Windows.RoutedEventArgs e)
        {
            var servicio = new NotificacionServicio();
            bool exito = await servicio.MarcarNotificacionComoLeidaAsync(idNotificacion);

            if (exito)
                this.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
