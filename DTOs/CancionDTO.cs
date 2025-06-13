using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMusAPI.DTOs
{
    public class CancionDTO
    {
        public String nombre { get; set; }
        public String archivoCancion { get; set; }
        public String urlFoto { get; set; }
        public String duracionStr { get; set; }
        public int idCategoriaMusical { get; set; }
        public int idAlbum { get; set; }
        public int posicionEnAlbum { get; set; }
        public List<int> idPerfilArtistas { get; set; } = new List<int>();
    }
}
