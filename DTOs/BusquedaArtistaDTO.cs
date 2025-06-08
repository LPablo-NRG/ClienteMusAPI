using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMusAPI.DTOs
{
    public class BusquedaArtistaDTO
    {
        public int idArtista { get; set; }
        public String nombre { get; set; }
        public String nombreUsuario { get; set; }
        public String descripcion { get; set; }
        public String urlFoto { get; set; }
        public List<BusquedaCancionDTO> canciones { get; set; }
    }
}
