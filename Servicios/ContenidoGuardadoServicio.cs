using ClienteMusAPI.DTOs;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClienteMusAPI.Servicios
{
    public class ContenidoGuardadoServicio
    {
        public async Task<string> GuardarContenidoAsync(ContenidoGuardadoDTO dto)
        {
            try
            {
                var json = JsonConvert.SerializeObject(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await ClienteAPI.HttpClient.PostAsync("contenidoGuardado/guardar", content);
                string responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Error: {response.StatusCode}\n{responseContent}");
                    return null;
                }

                var jsonObject = JsonConvert.DeserializeObject<JObject>(responseContent);
                var mensaje = jsonObject?["mensaje"]?.ToString();

                return mensaje ?? "Respuesta sin mensaje.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Excepción: {ex.Message}");
                return null;
            }
        }
    }
}
