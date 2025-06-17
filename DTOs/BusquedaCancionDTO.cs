using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMusAPI.DTOs
{
    public class BusquedaCancionDTO
    {
        public int idCancion { get; set; }
        public String nombre { get; set; }
        public String duracion { get; set; }
        public String urlArchivo { get; set; }
        public String urlFoto { get; set; }
        public String nombreArtista { get; set; }
        public String fechaPublicacion { get; set; }
        public String nombreAlbum { get; set; }
        public String categoriaMusical { get; set; }

    }
}
