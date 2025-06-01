using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMusAPI.DTOs
{
    public class LoginRequest
    {
        public String correo { get; set; }
        public String contrasenia { get; set; }
    }
}
