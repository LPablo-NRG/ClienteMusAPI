using ClienteMusAPI.DTOs;
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

namespace ClienteMusAPI.UserControls
{
    /// <summary>
    /// Lógica de interacción para ucCategoriaMusical.xaml
    /// </summary>
    public partial class ucCategoriaMusical : UserControl
    { 
        private CategoriaMusicalDTO categoriaMusical;
        public ucCategoriaMusical(CategoriaMusicalDTO categoriaMusical)
        {
            InitializeComponent();
            this.categoriaMusical = categoriaMusical;
            CargarDatos();
        }
        public ucCategoriaMusical()
        {
            InitializeComponent();
        }

        private void CargarDatos()
        {
            if (categoriaMusical != null)
            {
                txb_Nombre.Text = categoriaMusical.nombre;
                txb_Descripcion.Text = categoriaMusical.descripcion;
            }
        }

        private void Click_Editar(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Editar Categoria Musical: "+categoriaMusical.idCategoriaMusical);
            vtCategoriaMusical editarCategoria = new vtCategoriaMusical(categoriaMusical);
            NavigationService.GetNavigationService(this).Navigate(editarCategoria);
        }
    }
}
