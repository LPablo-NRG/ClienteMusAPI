using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMusAPI.Servicios
{
    public static class ClienteAPI
    {
        private static readonly HttpClient httpCliente = new HttpClient();

        static ClienteAPI()
        {
            httpCliente.BaseAddress = new Uri("http://localhost:8080/api/");
            httpCliente.DefaultRequestHeaders.Accept.Clear();
            httpCliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static HttpClient HttpClient => httpCliente;
    }
}
