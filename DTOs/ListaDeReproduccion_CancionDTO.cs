using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMusAPI.DTOs
{
    public class ListaDeReproduccion_CancionDTO
    {
        [JsonProperty("idCancion")]
        public int IdCancion { get; set; }

        [JsonProperty("idListaDeReproduccion")]
        public int IdListaDeReproduccion { get; set; }

        [JsonProperty("idUsuario")]
        public int IdUsuario { get; set; }
    }
}
