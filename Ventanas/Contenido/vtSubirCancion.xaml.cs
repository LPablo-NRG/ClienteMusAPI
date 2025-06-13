using ClienteMusAPI.Clases;
using ClienteMusAPI.DTOs;
using ClienteMusAPI.Servicios;
using ClienteMusAPI.Ventanas.Perfiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
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
    /// Lógica de interacción para vtSubirCancion.xaml
    /// </summary>
    public partial class vtSubirCancion : Page
    {
        private int idPerfilArtista;
        private int idAlbum = 0; // 0 indica que es un sencillo, no un álbum
        private CancionDTO cancionDTO = new CancionDTO();
        public vtSubirCancion(int idPerfilArtista)
        {
            InitializeComponent();
            this.idPerfilArtista = idPerfilArtista;
            CargarCategorias();
        }

        public vtSubirCancion(int idPerfilArtista, int idAlbum)
        {
            InitializeComponent();
            this.idPerfilArtista = idPerfilArtista;
            this.idAlbum = idAlbum;
            CargarCategorias();
        }

        private async void CargarCategorias()
        {
            CategoriaMusicalServicio categoriaMusicalServicio = new CategoriaMusicalServicio();
            List<CategoriaMusicalDTO> categorias = new List<CategoriaMusicalDTO>();
            categorias = await categoriaMusicalServicio.ObtenerCategoriasMusicalesAsync();
            
            cb_CategoriaMusical.Items.Clear();
            foreach (var categoria in categorias)
            {
                cb_CategoriaMusical.Items.Add(categoria);
            }


        }

        private void Click_Volver(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private async void Click_Confirmar(object sender, RoutedEventArgs e)
        {
            
            if (string.IsNullOrEmpty(txb_Nombre.Text) || string.IsNullOrEmpty(txb_ArchivoSeleccionado.Text) || string.IsNullOrEmpty(cancionDTO.urlFoto) || cb_CategoriaMusical.SelectedItem == null) {
                MessageBox.Show("Por favor, complete todos los campos obligatorios.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            List<int> idArtistas = new List<int>();
            idArtistas.Add(idPerfilArtista);
            cancionDTO.idPerfilArtistas = idArtistas;
            cancionDTO.nombre = txb_Nombre.Text;
            cancionDTO.idCategoriaMusical = (int)cb_CategoriaMusical.SelectedValue;

            if (idAlbum > 0)
            {
                cancionDTO.idAlbum = idAlbum;
            }
            cancionDTO.posicionEnAlbum = 0; // Posición en sencillo



            CancionServicio cancionServicio = new CancionServicio();
            bool exito = await cancionServicio.SubirCancionAsync(cancionDTO);
            if (exito)
            {
                NavigationService.GoBack();
            }
        }

        private void Click_SubirFoto(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Imágenes (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
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
                cancionDTO.urlFoto = openFileDialog.FileName;
                Console.WriteLine("Foto seleccionada: " + cancionDTO.urlFoto);
            }
        }

        private void Click_SubirArchivo(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Archivos de audio (*.wav;*.mp3)|*.wav;*.mp3";
            if (openFileDialog.ShowDialog() == true)
            {
                FileInfo informacionFoto = new FileInfo(openFileDialog.FileName);
                const long tamanioMaximo = 20 * 1024 * 1024;

                if (informacionFoto.Length > tamanioMaximo)
                {
                    MessageBox.Show("La canción supera el tamaño máximo.");
                    return;
                }

                // Obtener duración del archivo de audio
                TimeSpan duracion;
                try
                {
                    using (var audioFile = new NAudio.Wave.AudioFileReader(openFileDialog.FileName))
                    {
                        duracion = audioFile.TotalTime;
                    }
                    
                    cancionDTO.archivoCancion = openFileDialog.FileName;
                    cancionDTO.duracionStr = duracion.ToString(@"mm\:ss"); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al leer el archivo de audio: {ex.Message}");
                }

                txb_NombreArchivo.Text = openFileDialog.SafeFileName;
                txb_ArchivoSeleccionado.Text = cancionDTO.archivoCancion;
                txb_Duracion.Text = cancionDTO.duracionStr;
            }
        }
    }
}
