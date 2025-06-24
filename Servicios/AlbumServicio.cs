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
                    string responseContent = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        try
                        {
                            var jasonObject = JsonConvert.DeserializeObject<JObject>(responseContent);
                            string msj = jasonObject?["mensaje"]?.ToString() ?? "Error desconocido.";

                            if (msj == "ERROR_BD")
                            {
                                MessageBox.Show("El sistema falló al conectarse a la base de datos, favor de intentar más tarde.");
                            }
                            else if (msj == "ERROR_GENERAL")
                            {
                                MessageBox.Show("Ocurrió un error en el sistema, favor de intentar más tarde.");
                            }
                            else
                            {
                                MessageBox.Show($"Error: {msj}");
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Error inesperado. Intenta más tarde.");
                        }
                        return false;
                    }
                    return response.IsSuccessStatusCode;
                }
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("El sistema falló al conectarse con el servidor, favor de intentar más tarde.");
                return false;
            }
            catch (TaskCanceledException)
            {
                MessageBox.Show("La solicitud tardó demasiado. Verifica tu conexión e intenta más tarde.");
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error inesperado: {ex.Message}");
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
                    try
                    {
                        var jasonObject = JsonConvert.DeserializeObject<JObject>(responseContent);
                        string msj = jasonObject?["mensaje"]?.ToString() ?? "Error desconocido.";

                        if (msj == "ERROR_BD")
                        {
                            MessageBox.Show("El sistema falló al conectarse a la base de datos, favor de intentar más tarde.");
                        }
                        else if (msj == "ERROR_GENERAL")
                        {
                            MessageBox.Show("Ocurrió un error en el sistema, favor de intentar más tarde.");
                        }
                        else
                        {
                            MessageBox.Show($"Error: {msj}");
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Error inesperado. Intenta más tarde.");
                    }
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
            catch (HttpRequestException)
            {
                MessageBox.Show("El sistema falló al conectarse con el servidor, favor de intentar más tarde.");
                return null;
            }
            catch (TaskCanceledException)
            {
                MessageBox.Show("La solicitud tardó demasiado. Verifica tu conexión e intenta más tarde.");
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error inesperado: {ex.Message}");
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
                    try
                    {
                        var jasonObject = JsonConvert.DeserializeObject<JObject>(responseContent);
                        string msj = jasonObject?["mensaje"]?.ToString() ?? "Error desconocido.";

                        if (msj == "ERROR_BD")
                        {
                            MessageBox.Show("El sistema falló al conectarse a la base de datos, favor de intentar más tarde.");
                        }
                        else if (msj == "ERROR_GENERAL")
                        {
                            MessageBox.Show("Ocurrió un error en el sistema, favor de intentar más tarde.");
                        }
                        else
                        {
                            MessageBox.Show($"Error: {msj}");
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Error inesperado. Intenta más tarde.");
                    }
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
            catch (HttpRequestException)
            {
                MessageBox.Show("El sistema falló al conectarse con el servidor, favor de intentar más tarde.");
                return null;
            }
            catch (TaskCanceledException)
            {
                MessageBox.Show("La solicitud tardó demasiado. Verifica tu conexión e intenta más tarde.");
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error inesperado: {ex.Message}");
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
                    try
                    {
                        var jasonObject = JsonConvert.DeserializeObject<JObject>(responseContent);
                        string mensaje = jasonObject?["mensaje"]?.ToString() ?? "Error desconocido.";

                        if (mensaje == "ERROR_BD")
                        {
                            MessageBox.Show("El sistema falló al conectarse a la base de datos, favor de intentar más tarde.");
                        }
                        else if (mensaje == "ERROR_GENERAL")
                        {
                            MessageBox.Show("Ocurrió un error en el sistema, favor de intentar más tarde.");
                        }
                        else
                        {
                            MessageBox.Show($"Error: {mensaje}");
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Error inesperado. Intenta más tarde.");
                    }
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
            catch (HttpRequestException)
            {
                MessageBox.Show("El sistema falló al conectarse con el servidor, favor de intentar más tarde.");
                return null;
            }
            catch (TaskCanceledException)
            {
                MessageBox.Show("La solicitud tardó demasiado. Verifica tu conexión e intenta más tarde.");
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error inesperado: {ex.Message}");
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
                    try
                    {
                        var jasonObject = JsonConvert.DeserializeObject<JObject>(responseContent);
                        string msj = jasonObject?["mensaje"]?.ToString() ?? "Error desconocido.";

                        if (msj == "ERROR_BD")
                        {
                            MessageBox.Show("El sistema falló al conectarse a la base de datos, favor de intentar más tarde.");
                        }
                        else if (msj == "ERROR_GENERAL")
                        {
                            MessageBox.Show("Ocurrió un error en el sistema, favor de intentar más tarde.");
                        }
                        else
                        {
                            MessageBox.Show($"Error: {msj}");
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Error inesperado. Intenta más tarde.");
                    }
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
            catch (HttpRequestException)
            {
                MessageBox.Show("El sistema falló al conectarse con el servidor, favor de intentar más tarde.");
                return false;
            }
            catch (TaskCanceledException)
            {
                MessageBox.Show("La solicitud tardó demasiado. Verifica tu conexión e intenta más tarde.");
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error inesperado: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> EliminarAlbumAsync(int idAlbum)
        {
            try
            {
                HttpResponseMessage response = await ClienteAPI.HttpClient.DeleteAsync($"albumes/{idAlbum}/eliminar");
                string responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    try
                    {
                        var jasonObject = JsonConvert.DeserializeObject<JObject>(responseContent);
                        string msj = jasonObject?["mensaje"]?.ToString() ?? "Error desconocido.";

                        if (msj == "ERROR_BD")
                        {
                            MessageBox.Show("El sistema falló al conectarse a la base de datos, favor de intentar más tarde.");
                        }
                        else if (msj == "ERROR_GENERAL")
                        {
                            MessageBox.Show("Ocurrió un error en el sistema, favor de intentar más tarde.");
                        }
                        else
                        {
                            MessageBox.Show($"Error: {msj}");
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Error inesperado. Intenta más tarde.");
                    }
                    return false;
                }

                var jsonObject = JsonConvert.DeserializeObject<JObject>(responseContent);
                string mensaje = jsonObject?["mensajeDeApi"]?.ToString();
                return true;
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("El sistema falló al conectarse con el servidor, favor de intentar más tarde.");
                return false;
            }
            catch (TaskCanceledException)
            {
                MessageBox.Show("La solicitud tardó demasiado. Verifica tu conexión e intenta más tarde.");
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error inesperado: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> EditarAlbumAsync(AlbumDTO album)
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
                        form.Add(fileContent, "foto", Path.GetFileName(album.FotoPath));
                    }

                    HttpResponseMessage response = await ClienteAPI.HttpClient.PutAsync("albumes/editar", form);
                    string responseContent = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        try
                        {
                            var jasonObject = JsonConvert.DeserializeObject<JObject>(responseContent);
                            string mensaje = jasonObject?["mensaje"]?.ToString() ?? "Error desconocido.";

                            if (mensaje == "ERROR_BD")
                            {
                                MessageBox.Show("El sistema falló al conectarse a la base de datos, favor de intentar más tarde.");
                            }
                            else if (mensaje == "ERROR_GENERAL")
                            {
                                MessageBox.Show("Ocurrió un error en el sistema, favor de intentar más tarde.");
                            }
                            else
                            {
                                MessageBox.Show($"Error: {mensaje}");
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Error inesperado. Intenta más tarde.");
                        }
                        return false;
                    }

                    return true;
                }
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("El sistema falló al conectarse con el servidor, favor de intentar más tarde.");
                return false;
            }
            catch (TaskCanceledException)
            {
                MessageBox.Show("La solicitud tardó demasiado. Verifica tu conexión e intenta más tarde.");
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error inesperado: {ex.Message}");
                return false;
            }
        }

    }
}
