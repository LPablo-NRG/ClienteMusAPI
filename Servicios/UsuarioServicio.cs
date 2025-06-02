using ClienteMusAPI.Clases;
using ClienteMusAPI.DTOs;
using ClienteMusAPI.Modelo;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
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
                    MessageBox.Show($"Error: {response.StatusCode}\n{responseContent}");
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Excepción: {ex.Message}");
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
                    MessageBox.Show($"Error: {response.StatusCode}\n{responseContent}");
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
                SesionUsuario.Token = datos["token"]?.ToString() ?? "";
                ClienteAPI.EstablecerToken(SesionUsuario.Token);

                ClienteAPI.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Excepción: {ex.Message}");
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
                    MessageBox.Show($"Error al editar perfil: {response.StatusCode}\n{responseContent}");
                    return false;
                }

                MessageBox.Show("Perfil editado con éxito.");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Excepción: {ex.Message}");
                return false;
            }
        }
        
        public async Task<bool> CrearPerfilArtistaAsync(PerfilArtistaDTO perfil)
        {
            try
            {

                var form = new MultipartFormDataContent();

                form.Add(new StringContent(SesionUsuario.IdUsuario.ToString()), "idUsuario");
                form.Add(new StringContent(perfil.descripcion), "descripcion");

                if (!string.IsNullOrWhiteSpace(perfil.foto) && File.Exists(perfil.foto))
                {
                    var fileBytes = File.ReadAllBytes(perfil.foto);
                    var fileContent = new ByteArrayContent(fileBytes);
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                    form.Add(fileContent, "foto", Path.GetFileName(perfil.foto));
                }

                var response = await ClienteAPI.HttpClient.PostAsync("usuarios/crear-perfilArtista", form);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Error al crear perfil: {response.StatusCode}\n{responseContent}");
                    return false;
                }

                MessageBox.Show("Perfil de artista creado con éxito!");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}");
                return false;
            }
        }

    }
}

