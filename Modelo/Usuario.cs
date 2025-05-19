using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMusAPI.Modelo
{
    public class Usuario
    {
        [JsonIgnore]
        public int idUsuario {  get; set; }
        public string nombre { get; set; }
        public string correo { get; set; }
        public string nombreUsuario { get; set; }
        public string pais { get; set; }
        public bool esAdmin { get; set; }
        public bool esArtista { get; set; }
    }
}
