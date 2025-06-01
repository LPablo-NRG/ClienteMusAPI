using ClienteMusAPI.DTOs;
using ClienteMusAPI.Servicios;
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

namespace ClienteMusAPI.Ventanas.Perfiles
{
    /// <summary>
    /// Lógica de interacción para vtCrearPerfilArtista.xaml
    /// </summary>
    public partial class vtCrearPerfilArtista : Page
    {
        private PerfilArtistaDTO perfilArtistaDTO = new PerfilArtistaDTO();
        private UsuarioServicio usuarioServicio = new UsuarioServicio();
        public vtCrearPerfilArtista()
        {
            InitializeComponent();
        }

        private void CrearPerfilArtista()
        {
            // Aquí se implementaría la lógica para crear el perfil de artista
            // Por ejemplo, enviar los datos al servidor y manejar la respuesta
            MessageBox.Show("Perfil de artista creado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Click_Confirmar(object sender, RoutedEventArgs e)
        {
            
            CrearPerfil();
        }

        private async void CrearPerfil()
        {
            perfilArtistaDTO.descripcion = txb_Descripcion.Text;
            bool exito = await usuarioServicio.CrearPerfilArtistaAsync(perfilArtistaDTO);

            if (exito)
            {
                MessageBox.Show("Usuario registrado correctamente.");
            }
            else
            {
                MessageBox.Show("Error al registrar usuario.");
            }
        }

        private void Click_SubirFoto(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.png) | *.jpg; *.png";
            if (openFileDialog.ShowDialog() == true)
            {
                FileInfo informacionFoto = new FileInfo(openFileDialog.FileName);
                const long tamanioMaximo = 10 * 1024 * 1024;

                if (informacionFoto.Length > tamanioMaximo)
                {
                    MessageBox.Show("La imagen supera el tamaño máximo.");
                    return;
                }

                BitmapImage bitmap = new BitmapImage(new Uri(openFileDialog.FileName));
                img_foto.Source = bitmap;
                perfilArtistaDTO.foto = informacionFoto.FullName;
                Console.WriteLine("Foto seleccionada: " + perfilArtistaDTO.foto);
            }
        }


        private void Click_Volver(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
