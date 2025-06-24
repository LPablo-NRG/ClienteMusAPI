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

                MessageBox.Show("Lista de reproducción creada con éxito.");
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

        public async Task<List<ListaDeReproduccionDTO>> ObtenerListasPorUsuarioAsync(int idUsuario)
        {
            try
            {
                HttpResponseMessage response = await ClienteAPI.HttpClient.GetAsync($"listasDeReproduccion/usuario/{idUsuario}");
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

                var listas = datos.ToObject<List<ListaDeReproduccionDTO>>();
                return listas ?? new List<ListaDeReproduccionDTO>();
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
    }
}
