using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMusAPI.DTOs
{
    public class EstadisticasPersonalesDTO
    {
        public List<String> topCanciones { get; set; }
        public List<String> topArtistas { get; set; }
        public long segundosEscuchados { get; set; }
    }
}
