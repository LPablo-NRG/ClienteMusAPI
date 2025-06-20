using ClienteMusAPI.DTOs;
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
    /// Lógica de interacción para ucSeleccionarListaPopup.xaml
    /// </summary>
    public partial class ucSeleccionarListaPopup : UserControl
    {
        public event EventHandler<ListaDeReproduccionDTO> ListaSeleccionada;

        public ucSeleccionarListaPopup(List<ListaDeReproduccionDTO> listas)
        {
            InitializeComponent();
            CargarListas(listas);
        }

        private void CargarListas(List<ListaDeReproduccionDTO> listas)
        {
            foreach (var lista in listas)
            {
                var control = new ucSeleccionarLista(lista);
                control.ListaSeleccionada += (s, listaSeleccionada) =>
                {
                    ListaSeleccionada?.Invoke(this, listaSeleccionada);
                };
                sp_Listas.Children.Add(control);
            }
        }
    }
}
