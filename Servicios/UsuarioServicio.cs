using ClienteMusAPI.Modelo;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClienteMusAPI.Servicios
{
    public class UsuarioServicio
    {
        public async Task<bool> RegistrarUsuarioAsync(Usuario usuario)
        {
            try
            {
                var json = JsonConvert.SerializeObject(usuario);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await ClienteAPI.HttpClient.PostAsync("usuarios/registrar", content);

                string responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Error: {response.StatusCode}\n{responseContent}");
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Excepción: {ex.Message}");
                return false;
            }
        }

    }
}

