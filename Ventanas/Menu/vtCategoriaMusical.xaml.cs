using ClienteMusAPI.DTOs;
using ClienteMusAPI.Servicios;
using ClienteMusAPI.Ventanas.Contenido;
using ClienteMusAPI.Ventanas.Perfiles;
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
    /// Lógica de interacción para vtCategoriaMusical.xaml
    /// </summary>
    public partial class vtCategoriaMusical : Page
    {
        private CategoriaMusicalDTO categoriaMusical;
        public vtCategoriaMusical()
        {
            InitializeComponent();
        }

        public vtCategoriaMusical(CategoriaMusicalDTO categoriaMusical)
        {
            InitializeComponent();
            this.categoriaMusical = categoriaMusical;
            CargarDatos();
        }

        private void CargarDatos()
        {
            txb_Descipcion.Text = categoriaMusical.descripcion;
            txb_Nombre.Text = categoriaMusical.nombre;
        }

        private void Click_Volver(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private async void Click_Guardar(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txb_Nombre.Text) || string.IsNullOrEmpty(txb_Descipcion.Text))
            {

                MessageBox.Show("Por favor, complete todos los campos obligatorios.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            CategoriaMusicalDTO categoriaMusicalDTO = new CategoriaMusicalDTO
            {
                nombre = txb_Nombre.Text,
                descripcion = txb_Descipcion.Text
            };
            CategoriaMusicalServicio categoriaMusicalServicio = new CategoriaMusicalServicio();
            bool exito = false;
            if (categoriaMusical != null)
            {
                txb_Nombre.Text = categoriaMusical.nombre;
                txb_Descipcion.Text = categoriaMusical.descripcion;
                categoriaMusicalDTO.idCategoriaMusical = categoriaMusical.idCategoriaMusical;
                exito = await categoriaMusicalServicio.EditarCategoriaMusicalAsync(categoriaMusicalDTO);
            }
            else
            {
                exito = await categoriaMusicalServicio.RegistrarCategoriaMusicalAsync(categoriaMusicalDTO);
            }


            if (exito)
            {
                NavigationService.GoBack();
            }
        }
    }
}
