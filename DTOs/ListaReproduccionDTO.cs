using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMusAPI.DTOs
{
    public class ListaReproduccionDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int IdUsuario { get; set; }
        public string FotoPath { get; set; }
    }
}
