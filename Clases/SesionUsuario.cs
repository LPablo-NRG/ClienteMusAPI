using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMusAPI.Clases
{
    public static class SesionUsuario
    {
        public static int IdUsuario { get; set; }
        public static string NombreUsuario { get; set; } = string.Empty;
        public static string Nombre { get; set; } = string.Empty;
        public static string Pais { get; set; } = string.Empty;
        public static string Correo { get; set; } = string.Empty;
        public static bool EsAdmin { get; set; }
        public static bool EsArtista { get; set; }
        public static string Token { get; set; } = string.Empty;


        public static void Limpiar()
        {
            IdUsuario = 0;
            NombreUsuario = Nombre = Pais = Correo = Token = string.Empty;
            EsAdmin = EsArtista = false;
        }
    }

}
