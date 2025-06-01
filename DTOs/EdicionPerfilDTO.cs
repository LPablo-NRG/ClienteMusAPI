using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMusAPI.DTOs
{
    public class EdicionPerfilDTO
    {
        public string nombre { get; set; }
        public string nombreUsuario { get; set; }
        public string pais { get; set; }

        // Solo para artistas
        public string descripcion { get; set; }
        public string foto { get; set; } 
    }

}
