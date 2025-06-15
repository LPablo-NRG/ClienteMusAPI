using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMusAPI.DTOs
{
    public class ContenidoGuardadoDTO
    {
        [JsonProperty("idUsuario")]
        public int IdUsuario { get; set; }

        [JsonProperty("idContenidoGuardado")]
        public int IdContenidoGuardado { get; set; }

        [JsonProperty("tipoDeContenido")]
        public string TipoDeContenido { get; set; }
    }
}
