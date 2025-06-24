using ClienteMusAPI.DTOs;
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
    public class NotificacionServicio
    {
        public async Task<List<NotificacionDTO>> ObtenerNotificacionesPendientesAsync(int idUsuario)
        {
            try
            {
                string url = $"notificaciones/pendientes/{idUsuario}";

                HttpResponseMessage response = await ClienteAPI.HttpClient.GetAsync(url);
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

                var notificaciones = JsonConvert.DeserializeObject<List<NotificacionDTO>>(responseContent);
                return notificaciones ?? new List<NotificacionDTO>();
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

        public async Task<bool> MarcarNotificacionComoLeidaAsync(int idNotificacion)
        {
            try
            {
                string url = $"notificaciones/marcar-leida/{idNotificacion}";
                var content = new StringContent("", Encoding.UTF8, "application/json");

                HttpResponseMessage response = await ClienteAPI.HttpClient.PutAsync(url, content);
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

        public async Task<bool> MarcarTodasNotificacionesComoLeidasAsync(int idUsuario)
        {
            try
            {
                string url = $"notificaciones/marcar-todas-leidas/{idUsuario}";
                var content = new StringContent("", Encoding.UTF8, "application/json");

                HttpResponseMessage response = await ClienteAPI.HttpClient.PutAsync(url, content);
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
