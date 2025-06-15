using ClienteMusAPI.DTOs;
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
    /// Lógica de interacción para vtCategoriasMusicales.xaml
    /// </summary>
    public partial class vtCategoriasMusicales : Page
    {
        public vtCategoriasMusicales()
        {
            InitializeComponent();
            this.Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CargarCategorias();
        }

        private async void CargarCategorias()
        {
            sp_Categorias.Children.Clear();
            CategoriaMusicalServicio categoriaMusicalServicio = new CategoriaMusicalServicio();
            List<CategoriaMusicalDTO> categorias = await categoriaMusicalServicio.ObtenerCategoriasMusicalesAsync();

            if (categorias != null)
            {
                foreach (var categoria in categorias)
                {
                    ucCategoriaMusical categoriaMusical = new ucCategoriaMusical(categoria);
                    sp_Categorias.Children.Add(categoriaMusical);
                }
            }
            else
                Console.WriteLine("Error");
        }

        private void Click_NuevaCategoria(object sender, RoutedEventArgs e)
        {
            vtCategoriaMusical nuevaCategoria = new vtCategoriaMusical();
            NavigationService.Navigate(nuevaCategoria);
        }

        private void Click_Volver(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
