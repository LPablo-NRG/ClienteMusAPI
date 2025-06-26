using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMusAPI.DTOs
{
    public class CategoriaMusicalDTO
    {
        public string nombre { get; set; }
        public string descripcion { get; set; }

        public int? idCategoriaMusical { get; set; }
    }
}
