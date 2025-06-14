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
using ClienteMusAPI.Clases;
using System.Windows;

namespace ClienteMusAPI.Servicios
{
    public class AlbumServicio
    {
        public async Task<bool> CrearAlbumAsync(AlbumDTO album)
        {
            try
            {
                using (var form = new MultipartFormDataContent())
                {
                    form.Add(new StringContent(album.Nombre), "nombre");
                    form.Add(new StringContent(album.IdUsuario.ToString()), "idUsuario");

                    if (!string.IsNullOrEmpty(album.FotoPath))
                    {
                        var fileStream = new FileStream(album.FotoPath, FileMode.Open, FileAccess.Read);
                        var fileContent = new StreamContent(fileStream);
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                        form.Add(fileContent, "Foto", Path.GetFileName(album.FotoPath));
                    }

                    var response = await ClienteAPI.HttpClient.PostAsync("/api/albumes/crear", form);
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al crear álbum: " + ex.Message);
                return false;
            }
        }

        public async Task<List<InfoAlbumDTO>> ObtenerAlbumesPendientesAsync(int idPerfilArtista)
        {
            try
            {
                HttpResponseMessage response = await ClienteAPI.HttpClient.GetAsync($"albumes/pendientes?idPerfilArtista={idPerfilArtista}");
                string responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.Write($"Error: {response.StatusCode}\n{responseContent}");
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

                var albumes = datos.ToObject<List<InfoAlbumDTO>>();
                return albumes ?? new List<InfoAlbumDTO>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Excepción al obtener álbumes pendientes: {ex.Message}");
                return null;
            }
        }

        public async Task<List<BusquedaAlbumDTO>> ObtenerAlbumesPublicosAsync(int idPerfilArtista)
        {
            try
            {
                HttpResponseMessage response = await ClienteAPI.HttpClient.GetAsync($"albumes/artista?idPerfilArtista={idPerfilArtista}");
                string responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.Write($"Error: {response.StatusCode}\n{responseContent}");
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

                var albumes = datos.ToObject<List<BusquedaAlbumDTO>>();
                return albumes ?? new List<BusquedaAlbumDTO>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Excepción al obtener álbumes públicos: {ex.Message}");
                return null;
            }
        }

        public async Task<List<BusquedaAlbumDTO>> BuscarAlbum(string nombreAlbum)
        {
            try
            {
                HttpResponseMessage response = await ClienteAPI.HttpClient.GetAsync($"albumes/buscar?texto={nombreAlbum}");
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
                var albumes = datos.ToObject<List<BusquedaAlbumDTO>>(); 
                return albumes ?? new List<BusquedaAlbumDTO>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Excepción al buscar albumes: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> PublicarAlbum(long idAlbum)
        {
            try
            {
                HttpResponseMessage response = await ClienteAPI.HttpClient.PutAsync($"albumes/publicar/{idAlbum}",null);
                string responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: {response.StatusCode}\n{responseContent}");
                    return false;
                }

                var jsonObject = JsonConvert.DeserializeObject<JObject>(responseContent);

                var mensaje = jsonObject?["mensaje"]?.ToString();
                if (!string.IsNullOrEmpty(mensaje))
                {
                    Console.WriteLine($"Respuesta del servidor: {mensaje}");
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Excepción al publicar el álbum: {ex.Message}");
                return false;
            }
        }

    }
}
