using ClienteMusAPI.DTOs;
using ClienteMusAPI.Modelo;
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

namespace ClienteMusAPI.Ventanas.Inicio
{
    /// <summary>
    /// Lógica de interacción para InicioSesion.xaml
    /// </summary>
    public partial class vtInicioSesion : Page
    {
        private SolidColorBrush colorBordeCorrecto = (SolidColorBrush)(new BrushConverter().ConvertFrom("#4F959D"));
        private readonly UsuarioServicio usuarioServicio;

        public vtInicioSesion()
        {
            InitializeComponent();
            usuarioServicio = new UsuarioServicio();
            this.Loaded += vtInicioSesion_Loaded;
        } 

        private void vtInicioSesion_Loaded(object sender, RoutedEventArgs e)
        {
            VentanaPrincipal vt = (VentanaPrincipal)Application.Current.MainWindow;
            vt.Reproductor.Visibility = Visibility.Collapsed;
        }

        private void Click_RegistrarCuenta(object sender, RoutedEventArgs e)
        {

            NavigationService.Navigate(new Uri("/Ventanas/Inicio/vtRegistroCuenta.xaml", UriKind.Relative));
        }

        private async void Click_IniciarSesion(object sender, RoutedEventArgs e)
        {
            bool correoValido = CorreoEsValido();
            bool contraseniaValida = ContraseniaEsValida();
            if (correoValido && contraseniaValida)
            {
                bool exito = await IniciarSesion();
                if (exito)
                {
                    VentanaPrincipal vt = (VentanaPrincipal)Application.Current.MainWindow;
                    vt.Reproductor.Visibility = Visibility.Visible;

                    NavigationService.Navigate(new Uri("/Ventanas/Menu/vtMenuPrincipal.xaml", UriKind.Relative));
                }
            }
        }

        private async Task<bool> IniciarSesion()
        {
            var login = new LoginRequest
            {
                correo = txb_Correo.Text,
                contrasenia = pwb_Contrasenia.Password
            };
            bool exito = await usuarioServicio.IniciarSesionAsync(login);
            return exito;
        }

        public bool CorreoEsValido()
        {
            string correo = txb_Correo.Text.Trim();
            try
            {
                var addr = new System.Net.Mail.MailAddress(correo);
                if (addr.Address == correo)
                {
                    txb_Correo.BorderBrush = colorBordeCorrecto;
                    return true;
                }
                else
                {
                    txb_Correo.BorderBrush = Brushes.Crimson;
                    return false;
                }
            }
            catch
            {
                txb_Correo.BorderBrush = Brushes.Crimson;
                return false;
            }
        }

        public bool ContraseniaEsValida()
        {
            string contrasenia = pwb_Contrasenia.Password;
            if (!string.IsNullOrWhiteSpace(contrasenia))
            {
                pwb_Contrasenia.BorderBrush = colorBordeCorrecto;
                return true;
            }
            else
            {
                pwb_Contrasenia.BorderBrush = Brushes.Crimson;
                return false;
            }
        }

        private void Click_Salir(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}
