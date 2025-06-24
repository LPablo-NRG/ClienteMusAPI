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
                            MessageBox.Show(msj);
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

                return mensaje ?? "Respuesta sin mensaje.";
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

        public async Task<List<BusquedaAlbumDTO>> ObtenerAlbumesGuardadosAsync(int idUsuario)
        {
            try
            {
                HttpResponseMessage response = await ClienteAPI.HttpClient.GetAsync($"contenidoGuardado/albumes/{idUsuario}");
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
                    return new List<BusquedaAlbumDTO>();
                }

                var jsonObject = JsonConvert.DeserializeObject<JObject>(responseContent);
                var datos = jsonObject?["datos"]?.ToObject<List<BusquedaAlbumDTO>>();

                return datos ?? new List<BusquedaAlbumDTO>();
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

        public async Task<List<ListaDeReproduccionDTO>> ObtenerListasGuardadasAsync(int idUsuario)
        {
            try
            {
                HttpResponseMessage response = await ClienteAPI.HttpClient.GetAsync($"contenidoGuardado/listas/{idUsuario}");
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
                    return new List<ListaDeReproduccionDTO>();
                }

                var jsonObject = JsonConvert.DeserializeObject<JObject>(responseContent);
                var datos = jsonObject?["datos"]?.ToObject<List<ListaDeReproduccionDTO>>();

                return datos ?? new List<ListaDeReproduccionDTO>();
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

        public async Task<List<BusquedaArtistaDTO>> ObtenerArtistasGuardadosAsync(int idUsuario)
        {
            try
            {
                HttpResponseMessage response = await ClienteAPI.HttpClient.GetAsync($"contenidoGuardado/artistas/{idUsuario}");
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
                    return new List<BusquedaArtistaDTO>();
                }

                var jsonObject = JsonConvert.DeserializeObject<JObject>(responseContent);
                var datos = jsonObject?["datos"]?.ToObject<List<BusquedaArtistaDTO>>();

                return datos ?? new List<BusquedaArtistaDTO>();
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
    }
}
