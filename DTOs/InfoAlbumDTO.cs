using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMusAPI.DTOs
{
    public class InfoAlbumDTO
    {
        public int idAlbum { get; set; }
        public String nombreArtista { get; set; }
        public String nombre { get; set; }
        public String urlFoto { get; set; }
        public String fechaPublicacion { get; set; }
    }
}
