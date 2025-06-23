using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMusAPI.DTOs
{
    public class NotificacionDTO
    {
        public int idNotificacion { get; set; }
        public string mensaje { get; set; }
        public string fechaEnvio { get; set; }
    }
}
