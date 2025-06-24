using ClienteMusAPI.Clases;
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
    /// Lógica de interacción para ucEvaluarArtista.xaml
    /// </summary>
    public partial class ucEvaluarArtista : UserControl
    {
        private int calificacion = 0;
        private int idPerfilArtista = -1;
        public ucEvaluarArtista(int idPerfilArtista)
        {
            InitializeComponent();
            this.idPerfilArtista = idPerfilArtista;
        }

        private async Task<bool> RegistrarEvaluacion()
        {
            if (calificacion == 0)
            {
                MessageBox.Show("Debe seleccionar una calificación antes de continuar.");
                return false;
            }
            UsuarioServicio usuarioServicio = new UsuarioServicio();
            EvaluacionDTO evaluacion = new EvaluacionDTO
            {
                idUsuario = SesionUsuario.IdUsuario,
                idArtista = idPerfilArtista,
                comentario = txb_Comentario.Text,
                calificacion = calificacion
            };
            string repuesta = await usuarioServicio.EvaluarArtista(evaluacion);

            if(repuesta != null)
                MessageBox.Show(repuesta);
            return true;
        }

        private void Click_Estrella1(object sender, RoutedEventArgs e)
        {
            calificacion = 1;
            img_Estrella1.Source = new BitmapImage(new Uri("pack://application:,,,/Recursos/Iconos/iconoEstrellaCompleta.png"));
            img_Estrella2.Source = new BitmapImage(new Uri("pack://application:,,,/Recursos/Iconos/iconoEstrellaVacia.png"));
            img_Estrella3.Source = new BitmapImage(new Uri("pack://application:,,,/Recursos/Iconos/iconoEstrellaVacia.png"));
            img_Estrella4.Source = new BitmapImage(new Uri("pack://application:,,,/Recursos/Iconos/iconoEstrellaVacia.png"));
            img_Estrella5.Source = new BitmapImage(new Uri("pack://application:,,,/Recursos/Iconos/iconoEstrellaVacia.png"));
        }

        private void Click_Estrella2(object sender, RoutedEventArgs e)
        {
            calificacion = 2;
            img_Estrella1.Source = new BitmapImage(new Uri("pack://application:,,,/Recursos/Iconos/iconoEstrellaCompleta.png"));
            img_Estrella2.Source = new BitmapImage(new Uri("pack://application:,,,/Recursos/Iconos/iconoEstrellaCompleta.png"));
            img_Estrella3.Source = new BitmapImage(new Uri("pack://application:,,,/Recursos/Iconos/iconoEstrellaVacia.png"));
            img_Estrella4.Source = new BitmapImage(new Uri("pack://application:,,,/Recursos/Iconos/iconoEstrellaVacia.png"));
            img_Estrella5.Source = new BitmapImage(new Uri("pack://application:,,,/Recursos/Iconos/iconoEstrellaVacia.png"));
        }

        private void Click_Estrella3(object sender, RoutedEventArgs e)
        {
            calificacion = 3;
            img_Estrella1.Source = new BitmapImage(new Uri("pack://application:,,,/Recursos/Iconos/iconoEstrellaCompleta.png"));
            img_Estrella2.Source = new BitmapImage(new Uri("pack://application:,,,/Recursos/Iconos/iconoEstrellaCompleta.png"));
            img_Estrella3.Source = new BitmapImage(new Uri("pack://application:,,,/Recursos/Iconos/iconoEstrellaCompleta.png"));
            img_Estrella4.Source = new BitmapImage(new Uri("pack://application:,,,/Recursos/Iconos/iconoEstrellaVacia.png"));
            img_Estrella5.Source = new BitmapImage(new Uri("pack://application:,,,/Recursos/Iconos/iconoEstrellaVacia.png"));
        }

        private void Click_Estrella4(object sender, RoutedEventArgs e)
        {
            calificacion = 4;
            img_Estrella1.Source = new BitmapImage(new Uri("pack://application:,,,/Recursos/Iconos/iconoEstrellaCompleta.png"));
            img_Estrella2.Source = new BitmapImage(new Uri("pack://application:,,,/Recursos/Iconos/iconoEstrellaCompleta.png"));
            img_Estrella3.Source = new BitmapImage(new Uri("pack://application:,,,/Recursos/Iconos/iconoEstrellaCompleta.png"));
            img_Estrella4.Source = new BitmapImage(new Uri("pack://application:,,,/Recursos/Iconos/iconoEstrellaCompleta.png"));
            img_Estrella5.Source = new BitmapImage(new Uri("pack://application:,,,/Recursos/Iconos/iconoEstrellaVacia.png"));
        }

        private void Click_Estrella5(object sender, RoutedEventArgs e)
        {
            calificacion = 5;
            img_Estrella1.Source = new BitmapImage(new Uri("pack://application:,,,/Recursos/Iconos/iconoEstrellaCompleta.png"));
            img_Estrella2.Source = new BitmapImage(new Uri("pack://application:,,,/Recursos/Iconos/iconoEstrellaCompleta.png"));
            img_Estrella3.Source = new BitmapImage(new Uri("pack://application:,,,/Recursos/Iconos/iconoEstrellaCompleta.png"));
            img_Estrella4.Source = new BitmapImage(new Uri("pack://application:,,,/Recursos/Iconos/iconoEstrellaCompleta.png"));
            img_Estrella5.Source = new BitmapImage(new Uri("pack://application:,,,/Recursos/Iconos/iconoEstrellaCompleta.png"));
        }

        private async void Click_Confirmar(object sender, RoutedEventArgs e)
        {
            bool exito = await RegistrarEvaluacion();
            if(exito)
            {
                this.Visibility = Visibility.Collapsed;
                Click_Cerrar(sender, e);
            }
            
        }

        private void Click_Cerrar(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
