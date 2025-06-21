using ClienteMusAPI.Clases;
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
using System.Windows.Threading;

namespace ClienteMusAPI
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class VentanaPrincipal : Window
    {
        public MediaPlayer reproductor = new MediaPlayer();
        
        public VentanaPrincipal()
        {
            InitializeComponent();
            Reproductor.OnReproductorPausadoODetenido += CambiarABotonPlay;
            Reproductor.OnReproduccionIniciada += CambiarABotonPause;
            Reproductor.OnReproduccionIniciada += IniciarTimer;
            Reproductor.OnReproduccionIniciada += CargarDatosCancion;

            reproductor.Open(new Uri("pack://siteoforigin:,,,/Recursos/Sonidos/MusAPI.wav"));
            reproductor.Volume = 0.3; 
            MarcoPrincipal.Navigate(new Uri("/Ventanas/Inicio/vtInicioSesion.xaml", UriKind.Relative));
            reproductor.Play();

        }

        DispatcherTimer timer;
        bool usuarioArrastrando = false;

        private void IniciarTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void CargarDatosCancion()
        {
            txb_Artista.Text = Reproductor.listaCanciones[Reproductor.indiceActual].nombreArtista;
            txb_Cancion.Text = Reproductor.listaCanciones[Reproductor.indiceActual].nombre;
            CargarImagen(Reproductor.listaCanciones[Reproductor.indiceActual].urlFoto);
        }

        private async void CargarImagen(string url)
        {
            if (!String.IsNullOrEmpty(url))
            {
                var bytes = await ClienteAPI.HttpClient.GetByteArrayAsync(Constantes.URL_BASE + url);
                using (var stream = new MemoryStream(bytes))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = stream;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();
                    img_Cancion.Source = image;
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!usuarioArrastrando && Reproductor.Duracion.TotalSeconds > 0)
            {
                sld_Tiempo.Maximum = Reproductor.Duracion.TotalSeconds;
                sld_Tiempo.Value = Reproductor.Posicion.TotalSeconds;
                txb_TiempoActual.Text = FormatearTiempo(Reproductor.Posicion);
                txb_Duracion.Text = FormatearTiempo(Reproductor.Duracion);
            }
        }

        private string FormatearTiempo(TimeSpan tiempo)
        {
            return tiempo.ToString(@"mm\:ss"); // Formato "minutos:segundos"
        }

        private void sld_Tiempo_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            usuarioArrastrando = false;
            var slider = (Slider)sender;
            Reproductor.Posicion = TimeSpan.FromSeconds(slider.Value);
        }

        private void sld_Tiempo_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            usuarioArrastrando = true;
        } 

        private void sld_Volumen_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Reproductor.Volumen = (float)sld_Volumen.Value;
        }

        private void CambiarABotonPlay()
        {
            img_PausarReaunudar.Source = new BitmapImage(new Uri("pack://application:,,,/Recursos/Iconos/iconoReproducir.png"));
            
        }

        private void CambiarABotonPause()
        {
            img_PausarReaunudar.Source = new BitmapImage(new Uri("pack://application:,,,/Recursos/Iconos/iconoPausa.png"));
        }

        public void LimpiarInterfazReproductor()
        {
            txb_Artista.Text = "...";
            txb_Cancion.Text = "Reproduce una Canción!";
            img_Cancion.Source = new BitmapImage(new Uri("pack://application:,,,/Recursos/Iconos/iconoDisco.png"));
            sld_Tiempo.Value = 0;
            sld_Tiempo.Maximum = 100;
            txb_TiempoActual.Text = "00:00";
            txb_Duracion.Text = "00:00";
        }

        private void Click_PausarReaunudar(object sender, RoutedEventArgs e)
        {
            Reproductor.PausarReanudar();
        }

        private async void Click_Anterior(object sender, RoutedEventArgs e)
        {
            await Reproductor.CancionAnteriorAsync();
        }

        private async void Click_Siguiente(object sender, RoutedEventArgs e)
        {
            await Reproductor.SiguienteCancionAsync();
        }
    }
}
