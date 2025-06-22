using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMusAPI.DTOs
{
    public class CancionMasEscuchadaDTO
    {
        public String nombreCancion { get; set; }
        public String nombreUsuarioArtista { get; set; }
        public long segundosEscuchados { get; set; }
    }
}
