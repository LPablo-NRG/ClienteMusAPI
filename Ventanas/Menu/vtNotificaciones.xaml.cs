using ClienteMusAPI.Clases;
using ClienteMusAPI.Servicios;
using ClienteMusAPI.UserControls;
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
    /// Lógica de interacción para vtNotificaciones.xaml
    /// </summary>
    public partial class vtNotificaciones : Page
    {
        public vtNotificaciones()
        {
            InitializeComponent();
            CargarNotificaciones();
        }

        private async void CargarNotificaciones()
        {
            var servicio = new NotificacionServicio();
            var notificaciones = await servicio.ObtenerNotificacionesPendientesAsync(SesionUsuario.IdUsuario);

            sp_Notificaciones.Children.Clear();

            if (notificaciones == null)
            {
                TextBlock sinNotif = new TextBlock
                {
                    Text = "No se pudieron obtener tus notificaciones.",
                    FontSize = 18,
                    Margin = new Thickness(10),
                    Foreground = Brushes.Gray
                };
                sp_Notificaciones.Children.Add(sinNotif);
                return;
            }

            if (notificaciones.Count == 0)
            {
                TextBlock sinNotif = new TextBlock
                {
                    Text = "No tienes notificaciones pendientes.",
                    FontSize = 18,
                    Margin = new Thickness(10),
                    Foreground = Brushes.Gray
                };
                sp_Notificaciones.Children.Add(sinNotif);
                return;
            }

            foreach (var noti in notificaciones)
            {
                var uc = new ucNotificacion();
                uc.SetNotificacion(noti);
                sp_Notificaciones.Children.Add(uc);
            }
        }

        private void Click_Volver(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Click_VerPerfilUsuario(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Ventanas/Perfiles/vtPerfilUsuario.xaml", UriKind.Relative));
        }

        private async void Click_VaciarNotificaciones(object sender, RoutedEventArgs e)
        {
            var servicio = new NotificacionServicio();
            bool exito = await servicio.MarcarTodasNotificacionesComoLeidasAsync(SesionUsuario.IdUsuario);
            if (exito) 
                sp_Notificaciones.Children.Clear();
        }
    }
}
