using ClienteMusAPI.Clases;
using ClienteMusAPI.DTOs;
using ClienteMusAPI.Modelo;
using ClienteMusAPI.Ventanas.Perfiles;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClienteMusAPI.Servicios
{
    public class UsuarioServicio
    {
        public async Task<bool> RegistrarUsuarioAsync(UsuarioDTO usuario)
        {
            try
            {
                var json = JsonConvert.SerializeObject(usuario);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await ClienteAPI.HttpClient.PostAsync("usuarios/registrar", content);
                string responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    try
                    {
                        var jasonObject = JsonConvert.DeserializeObject<JObject>(responseContent);
                        string mensaje = jasonObject?["mensaje"]?.ToString() ?? "Error desconocido.";

                        // Mostrar directamente el mensaje que envía el servidor
                        MessageBox.Show(mensaje);

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


        public async Task<bool> IniciarSesionAsync(LoginRequest login)
        {
            try
            {
                var json = JsonConvert.SerializeObject(login);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await ClienteAPI.HttpClient.PostAsync("usuarios/login", content);
                string responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    try
                    {
                        var jasonObject = JsonConvert.DeserializeObject<JObject>(responseContent);
                        string mensaje = jasonObject?["mensaje"]?.ToString() ?? "Error desconocido.";

                        // Muestra el mensaje exacto del servidor: correo no encontrado, contraseña incorrecta, etc.
                        MessageBox.Show(mensaje);
                    }
                    catch
                    {
                        MessageBox.Show("Error inesperado. Intenta más tarde.");
                    }
                    return false;
                }

                var jsonObject = JsonConvert.DeserializeObject<JObject>(responseContent);
                var datos = jsonObject?["datos"];
                if (datos == null)
                {
                    MessageBox.Show("No se encontró el objeto 'datos' en la respuesta.");
                    return false;
                }

                string token = datos["token"]?.ToString() ?? "";
                if (string.IsNullOrWhiteSpace(token))
                {
                    MessageBox.Show("No se recibió un token válido.");
                    return false;
                }

                SesionUsuario.IdUsuario = datos["idUsuario"]?.ToObject<int>() ?? 0;
                SesionUsuario.NombreUsuario = datos["nombreUsuario"]?.ToString() ?? "";
                SesionUsuario.Nombre = datos["nombre"]?.ToString() ?? "";
                SesionUsuario.Pais = datos["pais"]?.ToString() ?? "";
                SesionUsuario.Correo = datos["correo"]?.ToString() ?? "";
                SesionUsuario.EsAdmin = datos["esAdmin"]?.ToObject<bool>() ?? false;
                SesionUsuario.EsArtista = datos["esArtista"]?.ToObject<bool>() ?? false;
                SesionUsuario.Token = token;

                ClienteAPI.EstablecerToken(SesionUsuario.Token);
                ClienteAPI.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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


        public async Task<bool> EditarPerfilAsync(EdicionPerfilDTO perfil)
        {
            try
            {
                int idUsuario = SesionUsuario.IdUsuario;
                string url = $"usuarios/{idUsuario}/editar-perfil";

                var form = new MultipartFormDataContent();

                if (!string.IsNullOrWhiteSpace(perfil.nombre))
                    form.Add(new StringContent(perfil.nombre), "nombre");

                if (!string.IsNullOrWhiteSpace(perfil.nombreUsuario))
                    form.Add(new StringContent(perfil.nombreUsuario), "nombreUsuario");

                if (!string.IsNullOrWhiteSpace(perfil.pais))
                    form.Add(new StringContent(perfil.pais), "pais");

                if (!string.IsNullOrWhiteSpace(perfil.descripcion))
                    form.Add(new StringContent(perfil.descripcion), "descripcion");

                if (!string.IsNullOrWhiteSpace(perfil.foto) && File.Exists(perfil.foto))
                {
                    var fileBytes = File.ReadAllBytes(perfil.foto);
                    var fileContent = new ByteArrayContent(fileBytes);
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                    form.Add(fileContent, "foto", Path.GetFileName(perfil.foto)); 
                }

                Console.WriteLine($"Token enviado: {SesionUsuario.Token}"); // Verifica que no sea null o vacío
                Console.WriteLine($"Headers del HttpClient: {ClienteAPI.HttpClient.DefaultRequestHeaders.Authorization}"); // Debe mostrar "Bearer [token]

                HttpResponseMessage response = await ClienteAPI.HttpClient.PutAsync(url, form);
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
                }

                MessageBox.Show("Perfil editado con éxito.");
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

        public async Task<bool> CrearPerfilArtistaAsync(PerfilArtistaDTO perfil)
        {
            try
            {
                var form = new MultipartFormDataContent();

                form.Add(new StringContent(perfil.IdUsuario.ToString()), "idUsuario");
                form.Add(new StringContent(perfil.Descripcion ?? ""), "descripcion");

                if (!string.IsNullOrEmpty(perfil.FotoPath) && File.Exists(perfil.FotoPath))
                {
                    var fotoBytes = File.ReadAllBytes(perfil.FotoPath);
                    var byteArrayContent = new ByteArrayContent(fotoBytes);
                    byteArrayContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                    form.Add(byteArrayContent, "foto", Path.GetFileName(perfil.FotoPath));
                }

                HttpResponseMessage response = await ClienteAPI.HttpClient.PostAsync("usuarios/crear-perfilArtista", form);
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
                }

                SesionUsuario.EsArtista = true;

                return true;
            }
            catch(HttpRequestException)
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

        public async Task<BusquedaArtistaDTO> ObtenerPerfilArtistaAsync(int idArtista)
        {
            try
            { 
                HttpResponseMessage response = await ClienteAPI.HttpClient.GetAsync($"usuarios/artista/{idArtista}");
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
                    MessageBox.Show("No se encontró el objeto 'datos' en la respuesta.");
                    return null;
                }
                 
                var perfil = new BusquedaArtistaDTO
                {
                    idArtista = datos["idArtista"]?.ToObject<int>() ?? 0,
                    nombre = datos["nombre"]?.ToString() ?? "",
                    nombreUsuario = datos["nombreUsuario"]?.ToString() ?? "",
                    descripcion = datos["descripcion"]?.ToString() ?? "",
                    urlFoto = datos["urlFoto"]?.ToString() ?? "",
                    canciones = datos["canciones"]?.ToObject<List<BusquedaCancionDTO>>() ?? new List<BusquedaCancionDTO>()
                };

                return perfil;
            }
            catch(HttpRequestException)
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

        public async Task<List<BusquedaArtistaDTO>> BuscarArtista (string nombreArtista)
        {
            try
            {
                HttpResponseMessage response = await ClienteAPI.HttpClient.GetAsync($"usuarios/artistas/buscar?texto={nombreArtista}");
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

                var artistas = datos.ToObject<List<BusquedaArtistaDTO>>();


                return artistas ?? new List<BusquedaArtistaDTO>();
            }
            catch(HttpRequestException)
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

        public async Task<string> EvaluarArtista (EvaluacionDTO evaluacionDTO)
        {
            try
            {
                var json = JsonConvert.SerializeObject(evaluacionDTO);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await ClienteAPI.HttpClient.PostAsync("evaluaciones/registrar", content);

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

                String respuesta = datos.ToObject<String>();


                return respuesta;
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

        public async Task<List<BusquedaUsuarioDTO>> BuscarUsuarioAsync(string nombreUsuario, int idUsuario)
        {
            try
            {
                string url = $"usuarios/buscar?texto={nombreUsuario}&idUsuario={idUsuario}";
                HttpResponseMessage response = await ClienteAPI.HttpClient.GetAsync(url);
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

                var usuarios = datos.ToObject<List<BusquedaUsuarioDTO>>();
                return usuarios ?? new List<BusquedaUsuarioDTO>();
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


        public async Task<bool> EliminarUsuarioAsync(int idUsuario, string motivo)
        {
            try
            {
                string url = $"usuarios/{idUsuario}/eliminar?motivo={Uri.EscapeDataString(motivo)}";

                HttpResponseMessage response = await ClienteAPI.HttpClient.DeleteAsync(url);
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

                MessageBox.Show("Usuario eliminado con éxito.");
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

