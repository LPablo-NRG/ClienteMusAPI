using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMusAPI.DTOs
{
    public class ListaDeReproduccionDTO
    {
        public int IdListaDeReproduccion { get; set; }
        public string Nombre { get; set; }
        public string UrlFoto { get; set; }
        public string Descripcion { get; set; }
        public List<BusquedaCancionDTO> Canciones { get; set; }
    }
}
