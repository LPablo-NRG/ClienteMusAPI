using ClienteMusAPI.DTOs;
using ClienteMusAPI.Ventanas.Contenido;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClienteMusAPI.Servicios
{
    public class CategoriaMusicalServicio
    {
        public async Task<List<CategoriaMusicalDTO>> ObtenerCategoriasMusicalesAsync()
        {
            try
            {
                HttpResponseMessage response = await ClienteAPI.HttpClient.GetAsync("categoriasMusicales");
                string responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Error: {response.StatusCode}\n{responseContent}");
                    return null;
                }
                var jsonObject = JsonConvert.DeserializeObject<JObject>(responseContent);

                var mensaje = jsonObject?["mensaje"]?.ToString();

                var datos = jsonObject?["datos"];
                if (datos == null)
                {
                    MessageBox.Show("No se encontró el objeto 'datos' en la respuesta.");
                    return null;
                }

                var categorias = datos.ToObject<List<CategoriaMusicalDTO>>();
                return categorias ?? new List<CategoriaMusicalDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener las categorias: " + ex.Message);
                return null;
            }
        }

        public async Task<bool> RegistrarCategoriaMusicalAsync(CategoriaMusicalDTO categoria)
        {
            try
            {
                var json = JsonConvert.SerializeObject(categoria);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await ClienteAPI.HttpClient.PostAsync("categoriasMusicales/registrar", content);

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

        public async Task<bool> EditarCategoriaMusicalAsync(CategoriaMusicalDTO categoria)
        {
            try
            {
                int idCategoria = categoria.idCategoriaMusical.GetValueOrDefault();
                categoria.idCategoriaMusical = null;
                var json = JsonConvert.SerializeObject(categoria);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                Console.Write("id primero: "+idCategoria);
                Console.Write("id despues: "+categoria.idCategoriaMusical);
                HttpResponseMessage response = await ClienteAPI.HttpClient.PutAsync($"categoriasMusicales/{idCategoria}", content);

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
