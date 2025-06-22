using ClienteMusAPI.DTOs;
using ClienteMusAPI.Servicios;
using ClienteMusAPI.UserControls;
using ClienteMusAPI.Ventanas.Menu;
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

namespace ClienteMusAPI.Ventanas.Contenido
{
    /// <summary>
    /// Lógica de interacción para vtEliminarUsuario.xaml
    /// </summary>
    public partial class vtEliminarUsuario : Page
    {
        private BusquedaUsuarioDTO usuario;

        public vtEliminarUsuario(BusquedaUsuarioDTO usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
            ucUsuario.CargarUsuario(usuario, false, false);
            UpdateCharacterCounter();
        }

        private void txbMotivo_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateCharacterCounter();
        }

        private void UpdateCharacterCounter()
        {
            if (txbMotivo != null && txtContador != null)
            {
                int count = txbMotivo.Text.Length;
                txtContador.Text = $"{count}/100";

                if (count > 90)
                    txtContador.Foreground = new SolidColorBrush(System.Windows.Media.Colors.OrangeRed);
                else
                    txtContador.Foreground = (SolidColorBrush)FindResource("ColorLetrasHEX");
            }
        }

        private void Click_Cancelar(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

        private async void Click_Eliminar(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txbMotivo.Text))
            {
                MessageBox.Show("Por favor ingrese un motivo para la eliminación.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var confirmacion = MessageBox.Show($"¿Está seguro que desea eliminar la cuenta de {usuario.nombre}? Esta acción no se puede deshacer.",
                                             "Confirmar eliminación",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Warning);

            if (confirmacion == MessageBoxResult.Yes)
            {
                // Lógica para eliminar el usuario
                try
                {
                    var servicio = new UsuarioServicio();
                    bool resultado = await servicio.EliminarUsuarioAsync(usuario.idUsuario, txbMotivo.Text);

                    if (resultado)
                    {
                        MessageBox.Show("Usuario eliminado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        NavigationService?.Navigate(new vtMenuAdmin());
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el usuario. Por favor intente nuevamente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar usuario: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Click_Volver(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }
}
