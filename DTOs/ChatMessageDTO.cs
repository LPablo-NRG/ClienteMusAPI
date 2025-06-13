using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMusAPI.DTOs
{
    public class ChatMessageDTO
    {
        [JsonProperty("nombreUsuario")]
        public string NombreUsuario { get; set; }

        [JsonProperty("mensaje")]
        public string Mensaje { get; set; }

        [JsonProperty("idPerfilArtista")]
        public int? IdPerfilArtista { get; set; }
    }
}
