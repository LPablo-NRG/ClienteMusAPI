using ClienteMusAPI.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ClienteMusAPI.Ventanas.Perfiles;
using System.Windows;
using System.Windows.Navigation;

namespace ClienteMusAPI.Servicios
{
    public class CancionServicio
    {
        public async Task<bool> SubirCancionAsync(CancionDTO cancion)
        {
            try
            {
                using (var form = new MultipartFormDataContent())
                {
                    form.Add(new StringContent(cancion.nombre), "nombre");
                    form.Add(new StringContent(string.Join(",", cancion.idPerfilArtistas)), "idPerfilArtistas");
                    form.Add(new StringContent(cancion.duracionStr), "duracionStr");
                    form.Add(new StringContent(cancion.idCategoriaMusical.ToString()), "idCategoriaMusical");
                    if (cancion.idAlbum != -1)
                    {
                        form.Add(new StringContent(cancion.idAlbum.ToString()), "idAlbum");
                        form.Add(new StringContent(cancion.posicionEnAlbum.ToString()), "posicionEnAlbum");
                    }

                    if (!string.IsNullOrEmpty(cancion.urlFoto))
                    {
                        var fileStream = new FileStream(cancion.urlFoto, FileMode.Open, FileAccess.Read);
                        var fileContent = new StreamContent(fileStream);
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                        form.Add(fileContent, "urlFoto", Path.GetFileName(cancion.urlFoto));
                    }

                    if (!string.IsNullOrEmpty(cancion.archivoCancion))
                    {
                        var fileStream = new FileStream(cancion.archivoCancion, FileMode.Open, FileAccess.Read);
                        var fileContent = new StreamContent(fileStream);
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue("audio/mp3");
                        form.Add(fileContent, "archivoCancion", Path.GetFileName(cancion.archivoCancion));
                    }

                    var response = await ClienteAPI.HttpClient.PostAsync("canciones/subir", form);
                    bool exito = response.IsSuccessStatusCode;
                    if (exito)
                    {
                        MessageBox.Show("Canción subida exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        
                    }
                    else
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Error: {response.StatusCode}\n{responseContent}");
                    }
                    return exito;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al subir la canción: " + ex.Message);
                return false;
            }
        }

        public async Task<List<BusquedaCancionDTO>> ObtenerCancionesPorAlbumAsync(int idAlbum)
        {
            try
            {
                HttpResponseMessage response = await ClienteAPI.HttpClient.GetAsync($"canciones/album/{idAlbum}/canciones");
                string responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.Write($"Error: {response.StatusCode}\n{responseContent}");
                    return null;
                }
                var jsonObject = JsonConvert.DeserializeObject<JObject>(responseContent);

                var mensaje = jsonObject?["mensaje"]?.ToString();
                if (string.IsNullOrEmpty(mensaje))
                {
                    Console.Write(mensaje);
                    return null;
                }

                var datos = jsonObject?["datos"];
                if (datos == null)
                {
                    MessageBox.Show("No se encontró el objeto 'datos' en la respuesta.");
                    return null;
                }

                var albumes = datos.ToObject<List<BusquedaCancionDTO>>();
                return albumes ?? new List<BusquedaCancionDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener las canciones: " + ex.Message);
                return null;
            }

        }

        public async Task<List<BusquedaCancionDTO>> ObtenerSencillosPorArtistaAsync(int idPerfilArtista)
        {
            try
            {
                HttpResponseMessage response = await ClienteAPI.HttpClient.GetAsync($"canciones/artista?idPerfilArtista={idPerfilArtista}");
                string responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    //MessageBox.Show($"Error: {response.StatusCode}\n{responseContent}");
                    return null;
                }

                var jsonObject = JsonConvert.DeserializeObject<JObject>(responseContent);

                var mensaje = jsonObject?["mensaje"]?.ToString();
                if (!string.IsNullOrEmpty(mensaje) && mensaje != "Sencillos recuperados exitosamente")
                {
                    MessageBox.Show(mensaje);
                    return null;
                }

                var datos = jsonObject?["datos"];
                if (datos == null)
                {
                    MessageBox.Show("No se encontró el objeto 'datos' en la respuesta.");
                    return null;
                }

                var sencillos = datos.ToObject<List<BusquedaCancionDTO>>();
                return sencillos ?? new List<BusquedaCancionDTO>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Excepción al obtener sencillos: {ex.Message}");
                return null;
            }
        }

        public async Task<List<BusquedaCancionDTO>> BuscarCancion(string nombreCancion)
        {
            try
            {
                HttpResponseMessage response = await ClienteAPI.HttpClient.GetAsync($"canciones/buscar?texto={nombreCancion}");
                string responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: {response.StatusCode}\n{responseContent}");
                    return null;
                }

                var jsonObject = JsonConvert.DeserializeObject<JObject>(responseContent);

                var datos = jsonObject?["datos"];
                if (datos == null)
                {
                    Console.WriteLine("No se encontró el objeto 'datos' en la respuesta.");
                    return null;
                }

                var canciones = datos.ToObject<List<BusquedaCancionDTO>>();


                return canciones ?? new List<BusquedaCancionDTO>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Excepción al buscar canciones: {ex.Message}");
                return null;
            }
        }

        public async Task RegistrarEscucha(EscuchaDTO escuchaDTO) {
            try
            {
                var json = JsonConvert.SerializeObject(escuchaDTO);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await ClienteAPI.HttpClient.PostAsync("escucha/registrar", content);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Escucha registrada exitosamente.");
                }
                else
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error al registrar escucha: {response.StatusCode}\n{responseContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al registrar escucha: {ex.Message}");
            }
        }
    }
}
