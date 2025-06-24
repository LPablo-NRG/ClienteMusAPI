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

        public async Task<List<BusquedaCancionDTO>> ObtenerCancionesPorAlbumAsync(int idAlbum)
        {
            try
            {
                HttpResponseMessage response = await ClienteAPI.HttpClient.GetAsync($"canciones/album/{idAlbum}/canciones");
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

        public async Task<List<BusquedaCancionDTO>> ObtenerSencillosPorArtistaAsync(int idPerfilArtista)
        {
            try
            {
                HttpResponseMessage response = await ClienteAPI.HttpClient.GetAsync($"canciones/artista?idPerfilArtista={idPerfilArtista}");
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

        public async Task<List<BusquedaCancionDTO>> BuscarCancion(string nombreCancion)
        {
            try
            {
                HttpResponseMessage response = await ClienteAPI.HttpClient.GetAsync($"canciones/buscar?texto={nombreCancion}");
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

                var datos = jsonObject?["datos"];
                if (datos == null)
                {
                    Console.WriteLine("No se encontró el objeto 'datos' en la respuesta.");
                    return null;
                }

                var canciones = datos.ToObject<List<BusquedaCancionDTO>>();


                return canciones ?? new List<BusquedaCancionDTO>();
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

        public async Task<bool> EliminarCancionAsync(int idCancion)
        {
            try
            {
                HttpResponseMessage response = await ClienteAPI.HttpClient.DeleteAsync($"canciones/{idCancion}/eliminar");
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
                string mensaje = jsonObject?["mensaje"]?.ToString();
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


        public async Task RegistrarEscucha(EscuchaDTO escuchaDTO) {
            try
            {
                var json = JsonConvert.SerializeObject(escuchaDTO);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await ClienteAPI.HttpClient.PostAsync("escucha/registrar", content);
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
                }
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("El sistema falló al conectarse con el servidor, favor de intentar más tarde.");
            }
            catch (TaskCanceledException)
            {
                MessageBox.Show("La solicitud tardó demasiado. Verifica tu conexión e intenta más tarde.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error inesperado: {ex.Message}");
            }
        }
    }
}
