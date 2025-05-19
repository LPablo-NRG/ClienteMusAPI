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
    /// Lógica de interacción para vtRegistrarCuenta.xaml
    /// </summary>
    public partial class vtRegistroCuenta : Page
    {
        private readonly UsuarioServicio usuarioServicio;
        public vtRegistroCuenta()
        {
            InitializeComponent();
            usuarioServicio = new UsuarioServicio();
        }

        private void Click_Cancelar(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private async void Registrar_Click(object sender, RoutedEventArgs e)
        {
            var usuario = new Usuario
            {
                nombre = "Axel de Jesus Luna Hernandez",
                correo = "axel.lu04@gmail.com",
                nombreUsuario = "AxelLuna",
                pais = "Mexico",
                esAdmin = true,
                esArtista = false
            };

            bool exito = await usuarioServicio.RegistrarUsuarioAsync(usuario);

            if (exito)
            {
                MessageBox.Show("Usuario registrado exitosamente.");
            }
            else
            {
                MessageBox.Show("Error al registrar usuario.");
            }
        }
    }
}
