using ClienteMusAPI.DTOs;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ClienteMusAPI.Ventanas.Perfiles;
using System.Windows.Forms;

namespace ClienteMusAPI.Servicios
{
    internal class EstadisticasServicio
    {
        public async Task<EstadisticasContenidoSubidoDTO> obtenerEstadisticasContenidoSubido(int idPerfilArtista, string tipoContenido)
        {
            try
            {
                HttpResponseMessage response = await ClienteAPI.HttpClient.GetAsync($"estadisticas/contenidoSubido?idPerfilArtista={idPerfilArtista}&tipoContenido={tipoContenido}");
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

                var estadisticas = datos.ToObject<EstadisticasContenidoSubidoDTO>();


                return estadisticas ?? new EstadisticasContenidoSubidoDTO();
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

        public async Task<EstadisticasPersonalesDTO> obtenerEstadisticasPersonales(int idUsuario, string fechaInicio, string fechaFin)
        {
            try
            {
                HttpResponseMessage response = await ClienteAPI.HttpClient.GetAsync($"estadisticas/personales?idUsuario={idUsuario}&fechaInicio={fechaInicio}&fechaFin={fechaFin}");
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

                var estadisticas = datos.ToObject<EstadisticasPersonalesDTO>();
                return estadisticas ?? new EstadisticasPersonalesDTO();
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

        public async Task<EstadisticasNumeroUsuariosDTO> obtenerEstadisticasUsuarios()
        {
            try
            {
                HttpResponseMessage response = await ClienteAPI.HttpClient.GetAsync("estadisticas/numeroUsuarios");
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

                var estadisticas = datos.ToObject<EstadisticasNumeroUsuariosDTO>();
                return estadisticas ?? new EstadisticasNumeroUsuariosDTO();
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

        public async Task<List<ArtistaMasEscuchadoDTO>> obtenerEstadisticasArtistas(string fechaInicio, string fechaFin)
        {
            try
            {
                HttpResponseMessage response = await ClienteAPI.HttpClient.GetAsync($"estadisticas/topArtistas?fechaInicio={fechaInicio}&fechaFin={fechaFin}");
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

                var estadisticas = datos.ToObject<List<ArtistaMasEscuchadoDTO>>();
                return estadisticas ?? new List<ArtistaMasEscuchadoDTO>();
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

        public async Task<List<CancionMasEscuchadaDTO>> obtenerEstadisticasCanciones(string fechaInicio, string fechaFin)
        {
            try
            {
                HttpResponseMessage response = await ClienteAPI.HttpClient.GetAsync($"estadisticas/topCanciones?fechaInicio={fechaInicio}&fechaFin={fechaFin}");
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

                var estadisticas = datos.ToObject<List<CancionMasEscuchadaDTO>>();
                return estadisticas ?? new List<CancionMasEscuchadaDTO>();
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
