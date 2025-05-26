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

namespace ClienteMusAPI.Ventanas.Contenido
{
    /// <summary>
    /// Lógica de interacción para vtAlbum.xaml
    /// </summary>
    public partial class vtAlbum : Page
    {
        public vtAlbum()
        {
            InitializeComponent();

            ucContenido jumpsuit = new ucContenido("Cancion");
            jumpsuit.txb_Nombre.Text = "Jumpsuit";
            ucContenido levitate = new ucContenido("Cancion");
            levitate.txb_Nombre.Text = "Levitate";
            ucContenido morph = new ucContenido("Cancion");
            morph.txb_Nombre.Text = "Morph";
            ucContenido myblood = new ucContenido("Cancion");
            myblood.txb_Nombre.Text = "My Blood";
            ucContenido chlorine = new ucContenido("Cancion");
            chlorine.txb_Nombre.Text = "Chlorine";
            ucContenido smithereens = new ucContenido("Cancion");
            smithereens.txb_Nombre.Text = "Smithereens";
            ucContenido ng = new ucContenido("Cancion");
            ng.txb_Nombre.Text = "Neon Gravestones";
            ucContenido th = new ucContenido("Cancion");
            th.txb_Nombre.Text = "The Hype";
            ucContenido natn = new ucContenido("Cancion");
            natn.txb_Nombre.Text = "Nico and the Niners";
            ucContenido cml = new ucContenido("Cancion");
            cml.txb_Nombre.Text = "Cut My Lip";
            ucContenido bandito = new ucContenido("Cancion");
            bandito.txb_Nombre.Text = "Bandito";
            ucContenido pet = new ucContenido("Cancion");
            pet.txb_Nombre.Text = "Pet Cheetah";
            ucContenido legend = new ucContenido("Cancion");
            legend.txb_Nombre.Text = "Legend";
            ucContenido ltc = new ucContenido("Cancion");
            ltc.txb_Nombre.Text = "Leave the City";

            
            sp_Canciones.Children.Add(jumpsuit);
            sp_Canciones.Children.Add(levitate);
            sp_Canciones.Children.Add(morph);
            sp_Canciones.Children.Add(myblood);
            sp_Canciones.Children.Add(chlorine);
            sp_Canciones.Children.Add(smithereens);
            sp_Canciones.Children.Add(ng);
            sp_Canciones.Children.Add(th);
            sp_Canciones.Children.Add(natn);
            sp_Canciones.Children.Add(cml);
            sp_Canciones.Children.Add(bandito);
            sp_Canciones.Children.Add(pet);
            sp_Canciones.Children.Add(legend);
            sp_Canciones.Children.Add(ltc);


        }
        private void Click_Volver(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Click_VerEstadisticas(object sender, RoutedEventArgs e)
        {

        }

        private void Click_EditarAlbum(object sender, RoutedEventArgs e)
        {

        }

        private void Click_GuardarAlbum(object sender, RoutedEventArgs e)
        {

        }
    }
}
