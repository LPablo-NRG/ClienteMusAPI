using ClienteMusAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ClienteMusAPI.Servicios
{
    public class ListaServicio
    {
        public async Task<bool> CrearListaReproduccionAsync(ListaReproduccionDTO dto)
        {
            try
            {
                var form = new MultipartFormDataContent();
                form.Add(new StringContent(dto.Nombre), "nombre");
                form.Add(new StringContent(dto.Descripcion), "descripcion");
                form.Add(new StringContent(dto.IdUsuario.ToString()), "idUsuario");

                if (!string.IsNullOrEmpty(dto.FotoPath) && File.Exists(dto.FotoPath))
                {
                    var bytes = File.ReadAllBytes(dto.FotoPath);
                    var fileContent = new ByteArrayContent(bytes);
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                    form.Add(fileContent, "foto", Path.GetFileName(dto.FotoPath));
                }

                var response = await ClienteAPI.HttpClient.PostAsync("listasDeReproduccion/crear", form);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Error al crear lista: {response.StatusCode}\n{responseContent}");
                    return false;
                }

                MessageBox.Show("Lista de reproducción creada con éxito.");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Excepción: {ex.Message}");
                return false;
            }
        }

        public async Task<List<ListaDeReproduccionDTO>> ObtenerListasPorUsuarioAsync(int idUsuario)
        {
            try
            {
                HttpResponseMessage response = await ClienteAPI.HttpClient.GetAsync($"listasDeReproduccion/usuario/{idUsuario}");
                string responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: {response.StatusCode}\n{responseContent}");
                    return null;
                }

                var jsonObject = JsonConvert.DeserializeObject<JObject>(responseContent);

                var mensaje = jsonObject?["mensaje"]?.ToString();
                var datos = jsonObject?["datos"];
                if (datos == null)
                {
                    Console.WriteLine("No se encontró el objeto 'datos' en la respuesta.");
                    return null;
                }

                var listas = datos.ToObject<List<ListaDeReproduccionDTO>>();
                return listas ?? new List<ListaDeReproduccionDTO>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Excepción al obtener listas de reproducción: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> AgregarCancionAListaAsync(ListaDeReproduccion_CancionDTO dto)
        {
            try
            {
                var json = JsonConvert.SerializeObject(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await ClienteAPI.HttpClient.PostAsync("listasDeReproduccion/agregar-cancion", content);
                string responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: {response.StatusCode}\n{responseContent}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar canción a la lista: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> EditarListaAsync(ListaReproduccionDTO lista)
        {
            try
            {
                using (var form = new MultipartFormDataContent())
                {
                    form.Add(new StringContent(lista.Nombre), "nombre");
                    form.Add(new StringContent(lista.Descripcion), "descripcion");
                    form.Add(new StringContent(lista.IdUsuario.ToString()), "idUsuario");

                    if (!string.IsNullOrEmpty(lista.FotoPath))
                    {
                        var fileStream = new FileStream(lista.FotoPath, FileMode.Open, FileAccess.Read);
                        var fileContent = new StreamContent(fileStream);
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                        form.Add(fileContent, "foto", Path.GetFileName(lista.FotoPath));
                    }

                    HttpResponseMessage response = await ClienteAPI.HttpClient.PutAsync("listasDeReproduccion/editar", form);
                    string responseContent = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Error: {response.StatusCode}\n{responseContent}");
                        return false;
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar la lista: " + ex.Message);
                return false;
            }
        }
    }
}
