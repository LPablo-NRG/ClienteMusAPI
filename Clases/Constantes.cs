using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMusAPI.Clases
{
    public class Constantes
    {
        public static string PUERTO = "8080";
        public static string IP = "192.168.160.244";
        public static string URL_BASE = "http://" + IP + ":" +PUERTO;
        
        //public static string URL_BASE = "http://" + IP;
        public static string URL_API = URL_BASE+ "/api/";
        public static string URL_CHAT = "ws://" + IP + ":" + PUERTO + "/ws/";

        
    }
}
