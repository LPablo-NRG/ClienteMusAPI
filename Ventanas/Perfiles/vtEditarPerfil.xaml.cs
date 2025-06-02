using ClienteMusAPI.Clases;
using ClienteMusAPI.DTOs;
using ClienteMusAPI.Modelo;
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
    /// Lógica de interacción para vtEditarPerfil.xaml
    /// </summary>
    public partial class vtEditarPerfil : Page
    {
        private SolidColorBrush colorBordeCorrecto = (SolidColorBrush)(new BrushConverter().ConvertFrom("#4F959D"));
        private UsuarioServicio usuarioServicio = new UsuarioServicio();
        private EdicionPerfilDTO edicionPerfil = new EdicionPerfilDTO();
        private bool esArtista;

        public vtEditarPerfil(bool esArtista)
        {
            InitializeComponent();
            this.esArtista = esArtista;
            CargarDatos();
        }

        private void CargarDatos()
        {
            CargarPaises();
            txb_Nombre.Text = SesionUsuario.NombreUsuario;
            cb_pais.SelectedValue = SesionUsuario.Pais;
            if (esArtista)
            {
                sp_Pais.Visibility = Visibility.Collapsed;
            }
            else
            {
                sp_Descripcion.Visibility = Visibility.Collapsed;
                sp_Foto.Visibility = Visibility.Collapsed;
            }
        }

        private bool ValidarCampos()
        {
            bool nombreValido = NombreEsValido();
            if (cb_pais.SelectedItem != null)
            {
                cb_pais.BorderBrush = colorBordeCorrecto;
                if (nombreValido)
                {
                    return true;
                }
            }
            else
            {
                cb_pais.BorderBrush = Brushes.Crimson;
            }
            return false;
        }

        private bool NombreEsValido()
        {
            if (string.IsNullOrWhiteSpace(txb_Nombre.Text.Trim()))
            {
                txb_Nombre.BorderBrush = Brushes.Crimson;
                return false;
            }
            else
            {
                txb_Nombre.BorderBrush = colorBordeCorrecto;
                return true;
            }
        }

        private void Click_Confirmar(object sender, RoutedEventArgs e)
        {
            if (ValidarCampos())
            {
                EditarPerfil();
            }
            else
            {
                MessageBox.Show("Llene los campos correctamente.");
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
                edicionPerfil.foto = informacionFoto.FullName;
                Console.WriteLine("Foto seleccionada: " + edicionPerfil.foto);
            }
        }

        private async void EditarPerfil()
        {
            edicionPerfil.nombre = txb_Nombre.Text.Trim();
            edicionPerfil.pais = cb_pais.SelectedValue.ToString();
            edicionPerfil.nombreUsuario = SesionUsuario.NombreUsuario;
            if (SesionUsuario.EsArtista) {
                edicionPerfil.descripcion = txb_Descripcion.Text.Trim();
            }
            
            bool exito = await usuarioServicio.EditarPerfilAsync(edicionPerfil);

            if (exito)
            {
                MessageBox.Show("Usuario registrado correctamente.");
            }
            else
            {
                MessageBox.Show("Error al registrar usuario.");
            }
        }

        private void Click_Volver(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void CargarPaises()
        {
            var countries = new List<Pais>
    {
        // América
        new Pais { Codigo = "AR", Nombre = "Argentina" },
        new Pais { Codigo = "BO", Nombre = "Bolivia" },
        new Pais { Codigo = "BR", Nombre = "Brasil" },
        new Pais { Codigo = "CA", Nombre = "Canadá" },
        new Pais { Codigo = "CL", Nombre = "Chile" },
        new Pais { Codigo = "CO", Nombre = "Colombia" },
        new Pais { Codigo = "CR", Nombre = "Costa Rica" },
        new Pais { Codigo = "CU", Nombre = "Cuba" },
        new Pais { Codigo = "DO", Nombre = "República Dominicana" },
        new Pais { Codigo = "EC", Nombre = "Ecuador" },
        new Pais { Codigo = "SV", Nombre = "El Salvador" },
        new Pais { Codigo = "US", Nombre = "Estados Unidos" },
        new Pais { Codigo = "GT", Nombre = "Guatemala" },
        new Pais { Codigo = "HT", Nombre = "Haití" },
        new Pais { Codigo = "HN", Nombre = "Honduras" },
        new Pais { Codigo = "JM", Nombre = "Jamaica" },
        new Pais { Codigo = "MX", Nombre = "México" },
        new Pais { Codigo = "NI", Nombre = "Nicaragua" },
        new Pais { Codigo = "PA", Nombre = "Panamá" },
        new Pais { Codigo = "PY", Nombre = "Paraguay" },
        new Pais { Codigo = "PE", Nombre = "Perú" },
        new Pais { Codigo = "PR", Nombre = "Puerto Rico" }, // Territorio no soberano
        new Pais { Codigo = "UY", Nombre = "Uruguay" },
        new Pais { Codigo = "VE", Nombre = "Venezuela" },

        // Europa
        new Pais { Codigo = "AL", Nombre = "Albania" },
        new Pais { Codigo = "DE", Nombre = "Alemania" },
        new Pais { Codigo = "AD", Nombre = "Andorra" },
        new Pais { Codigo = "AT", Nombre = "Austria" },
        new Pais { Codigo = "BE", Nombre = "Bélgica" },
        new Pais { Codigo = "BY", Nombre = "Bielorrusia" },
        new Pais { Codigo = "BA", Nombre = "Bosnia y Herzegovina" },
        new Pais { Codigo = "BG", Nombre = "Bulgaria" },
        new Pais { Codigo = "CY", Nombre = "Chipre" },
        new Pais { Codigo = "HR", Nombre = "Croacia" },
        new Pais { Codigo = "DK", Nombre = "Dinamarca" },
        new Pais { Codigo = "SK", Nombre = "Eslovaquia" },
        new Pais { Codigo = "SI", Nombre = "Eslovenia" },
        new Pais { Codigo = "ES", Nombre = "España" },
        new Pais { Codigo = "EE", Nombre = "Estonia" },
        new Pais { Codigo = "FI", Nombre = "Finlandia" },
        new Pais { Codigo = "FR", Nombre = "Francia" },
        new Pais { Codigo = "GR", Nombre = "Grecia" },
        new Pais { Codigo = "HU", Nombre = "Hungría" },
        new Pais { Codigo = "IE", Nombre = "Irlanda" },
        new Pais { Codigo = "IS", Nombre = "Islandia" },
        new Pais { Codigo = "IT", Nombre = "Italia" },
        new Pais { Codigo = "LV", Nombre = "Letonia" },
        new Pais { Codigo = "LI", Nombre = "Liechtenstein" },
        new Pais { Codigo = "LT", Nombre = "Lituania" },
        new Pais { Codigo = "LU", Nombre = "Luxemburgo" },
        new Pais { Codigo = "MK", Nombre = "Macedonia del Norte" },
        new Pais { Codigo = "MT", Nombre = "Malta" },
        new Pais { Codigo = "MD", Nombre = "Moldavia" },
        new Pais { Codigo = "MC", Nombre = "Mónaco" },
        new Pais { Codigo = "ME", Nombre = "Montenegro" },
        new Pais { Codigo = "NO", Nombre = "Noruega" },
        new Pais { Codigo = "NL", Nombre = "Países Bajos" },
        new Pais { Codigo = "PL", Nombre = "Polonia" },
        new Pais { Codigo = "PT", Nombre = "Portugal" },
        new Pais { Codigo = "GB", Nombre = "Reino Unido" },
        new Pais { Codigo = "CZ", Nombre = "República Checa" },
        new Pais { Codigo = "RO", Nombre = "Rumanía" },
        new Pais { Codigo = "RU", Nombre = "Rusia" },
        new Pais { Codigo = "SM", Nombre = "San Marino" },
        new Pais { Codigo = "RS", Nombre = "Serbia" },
        new Pais { Codigo = "SE", Nombre = "Suecia" },
        new Pais { Codigo = "CH", Nombre = "Suiza" },
        new Pais { Codigo = "UA", Nombre = "Ucrania" },
        new Pais { Codigo = "VA", Nombre = "Vaticano" },

        // Asia
        new Pais { Codigo = "AF", Nombre = "Afganistán" },
        new Pais { Codigo = "SA", Nombre = "Arabia Saudita" },
        new Pais { Codigo = "AM", Nombre = "Armenia" },
        new Pais { Codigo = "AZ", Nombre = "Azerbaiyán" },
        new Pais { Codigo = "BD", Nombre = "Bangladés" },
        new Pais { Codigo = "BT", Nombre = "Bután" },
        new Pais { Codigo = "MM", Nombre = "Myanmar (Birmania)" },
        new Pais { Codigo = "KH", Nombre = "Camboya" },
        new Pais { Codigo = "CN", Nombre = "China" },
        new Pais { Codigo = "KP", Nombre = "Corea del Norte" },
        new Pais { Codigo = "KR", Nombre = "Corea del Sur" },
        new Pais { Codigo = "AE", Nombre = "Emiratos Árabes Unidos" },
        new Pais { Codigo = "PH", Nombre = "Filipinas" },
        new Pais { Codigo = "GE", Nombre = "Georgia" },
        new Pais { Codigo = "IN", Nombre = "India" },
        new Pais { Codigo = "ID", Nombre = "Indonesia" },
        new Pais { Codigo = "IQ", Nombre = "Irak" },
        new Pais { Codigo = "IR", Nombre = "Irán" },
        new Pais { Codigo = "IL", Nombre = "Israel" },
        new Pais { Codigo = "JP", Nombre = "Japón" },
        new Pais { Codigo = "JO", Nombre = "Jordania" },
        new Pais { Codigo = "KZ", Nombre = "Kazajistán" },
        new Pais { Codigo = "KG", Nombre = "Kirguistán" },
        new Pais { Codigo = "KW", Nombre = "Kuwait" },
        new Pais { Codigo = "LA", Nombre = "Laos" },
        new Pais { Codigo = "LB", Nombre = "Líbano" },
        new Pais { Codigo = "MY", Nombre = "Malasia" },
        new Pais { Codigo = "MV", Nombre = "Maldivas" },
        new Pais { Codigo = "MN", Nombre = "Mongolia" },
        new Pais { Codigo = "NP", Nombre = "Nepal" },
        new Pais { Codigo = "OM", Nombre = "Omán" },
        new Pais { Codigo = "PK", Nombre = "Pakistán" },
        new Pais { Codigo = "PS", Nombre = "Palestina" },
        new Pais { Codigo = "QA", Nombre = "Qatar" },
        new Pais { Codigo = "SG", Nombre = "Singapur" },
        new Pais { Codigo = "LK", Nombre = "Sri Lanka" },
        new Pais { Codigo = "SY", Nombre = "Siria" },
        new Pais { Codigo = "TH", Nombre = "Tailandia" },
        new Pais { Codigo = "TJ", Nombre = "Tayikistán" },
        new Pais { Codigo = "TL", Nombre = "Timor Oriental" },
        new Pais { Codigo = "TM", Nombre = "Turkmenistán" },
        new Pais { Codigo = "TR", Nombre = "Turquía" },
        new Pais { Codigo = "UZ", Nombre = "Uzbekistán" },
        new Pais { Codigo = "VN", Nombre = "Vietnam" },
        new Pais { Codigo = "YE", Nombre = "Yemen" },

        // África
        new Pais { Codigo = "DZ", Nombre = "Argelia" },
        new Pais { Codigo = "AO", Nombre = "Angola" },
        new Pais { Codigo = "BJ", Nombre = "Benín" },
        new Pais { Codigo = "BW", Nombre = "Botsuana" },
        new Pais { Codigo = "BF", Nombre = "Burkina Faso" },
        new Pais { Codigo = "BI", Nombre = "Burundi" },
        new Pais { Codigo = "CV", Nombre = "Cabo Verde" },
        new Pais { Codigo = "CM", Nombre = "Camerún" },
        new Pais { Codigo = "TD", Nombre = "Chad" },
        new Pais { Codigo = "KM", Nombre = "Comoras" },
        new Pais { Codigo = "CG", Nombre = "Congo" },
        new Pais { Codigo = "CD", Nombre = "Congo (RDC)" },
        new Pais { Codigo = "CI", Nombre = "Costa de Marfil" },
        new Pais { Codigo = "EG", Nombre = "Egipto" },
        new Pais { Codigo = "ET", Nombre = "Etiopía" },
        new Pais { Codigo = "GA", Nombre = "Gabón" },
        new Pais { Codigo = "GM", Nombre = "Gambia" },
        new Pais { Codigo = "GH", Nombre = "Ghana" },
        new Pais { Codigo = "GN", Nombre = "Guinea" },
        new Pais { Codigo = "GQ", Nombre = "Guinea Ecuatorial" },
        new Pais { Codigo = "KE", Nombre = "Kenia" },
        new Pais { Codigo = "LS", Nombre = "Lesoto" },
        new Pais { Codigo = "LR", Nombre = "Liberia" },
        new Pais { Codigo = "LY", Nombre = "Libia" },
        new Pais { Codigo = "MG", Nombre = "Madagascar" },
        new Pais { Codigo = "MW", Nombre = "Malaui" },
        new Pais { Codigo = "ML", Nombre = "Malí" },
        new Pais { Codigo = "MA", Nombre = "Marruecos" },
        new Pais { Codigo = "MU", Nombre = "Mauricio" },
        new Pais { Codigo = "MR", Nombre = "Mauritania" },
        new Pais { Codigo = "MZ", Nombre = "Mozambique" },
        new Pais { Codigo = "NA", Nombre = "Namibia" },
        new Pais { Codigo = "NE", Nombre = "Níger" },
        new Pais { Codigo = "NG", Nombre = "Nigeria" },
        new Pais { Codigo = "RW", Nombre = "Ruanda" },
        new Pais { Codigo = "ST", Nombre = "Santo Tomé y Príncipe" },
        new Pais { Codigo = "SN", Nombre = "Senegal" },
        new Pais { Codigo = "SC", Nombre = "Seychelles" },
        new Pais { Codigo = "SL", Nombre = "Sierra Leona" },
        new Pais { Codigo = "SO", Nombre = "Somalia" },
        new Pais { Codigo = "ZA", Nombre = "Sudáfrica" },
        new Pais { Codigo = "SS", Nombre = "Sudán del Sur" },
        new Pais { Codigo = "SD", Nombre = "Sudán" },
        new Pais { Codigo = "SZ", Nombre = "Suazilandia" },
        new Pais { Codigo = "TZ", Nombre = "Tanzania" },
        new Pais { Codigo = "TG", Nombre = "Togo" },
        new Pais { Codigo = "TN", Nombre = "Túnez" },
        new Pais { Codigo = "UG", Nombre = "Uganda" },
        new Pais { Codigo = "ZM", Nombre = "Zambia" },
        new Pais { Codigo = "ZW", Nombre = "Zimbabue" },

        // Oceanía
        new Pais { Codigo = "AU", Nombre = "Australia" },
        new Pais { Codigo = "FJ", Nombre = "Fiyi" },
        new Pais { Codigo = "KI", Nombre = "Kiribati" },
        new Pais { Codigo = "MH", Nombre = "Islas Marshall" },
        new Pais { Codigo = "FM", Nombre = "Micronesia" },
        new Pais { Codigo = "NR", Nombre = "Nauru" },
        new Pais { Codigo = "NZ", Nombre = "Nueva Zelanda" },
        new Pais { Codigo = "PW", Nombre = "Palaos" },
        new Pais { Codigo = "PG", Nombre = "Papúa Nueva Guinea" },
        new Pais { Codigo = "WS", Nombre = "Samoa" },
        new Pais { Codigo = "SB", Nombre = "Islas Salomón" },
        new Pais { Codigo = "TO", Nombre = "Tonga" },
        new Pais { Codigo = "TV", Nombre = "Tuvalu" },
        new Pais { Codigo = "VU", Nombre = "Vanuatu" }
    };

            // Ordenar alfabéticamente ignorando mayúsculas/tildes
            var paisesOrdenados = countries
                .OrderBy(p => p.Nombre, StringComparer.Create(new System.Globalization.CultureInfo("es-ES"), true))
                .ToList();

            cb_pais.ItemsSource = paisesOrdenados;
        } 
    }
}
