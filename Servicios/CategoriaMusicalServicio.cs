using ClienteMusAPI.DTOs;
using ClienteMusAPI.Ventanas.Contenido;
using Grpc.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Musapi.Grpc;

namespace ClienteMusAPI.Servicios
{
    public class CategoriaMusicalServicio
    {
        private CategoriaMusicalService.CategoriaMusicalServiceClient grpcClient;

        public CategoriaMusicalServicio()
        {
            // Crea un canal sin TLS (inseguro)
            Channel channel = new Channel("localhost:9090", ChannelCredentials.Insecure);
            grpcClient = new CategoriaMusicalService.CategoriaMusicalServiceClient(channel);
        }

        public async Task<bool> RegistrarCategoriaMusicalAsync(CategoriaMusicalDTO categoria)
        {
            try
            {
                var request = new CategoriaMusicalRequest
                {
                    Nombre = categoria.nombre,
                    Descripcion = categoria.descripcion
                };

                var response = await grpcClient.RegistrarCategoriaMusicalAsync(request);

                if (string.IsNullOrWhiteSpace(response.Nombre))
                {
                    MessageBox.Show("No se pudo registrar la categoría. Verifica los datos.");
                    return false;
                }

                MessageBox.Show($"Categoría '{response.Nombre}' registrada con éxito.");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de comunicación gRPC: " + ex.Message);
                return false;
            }
        }

        public async Task<List<CategoriaMusicalDTO>> ObtenerCategoriasMusicalesAsync()
        {
            try
            {
                var response = await grpcClient.ObtenerCategoriasMusicalesAsync(new Empty());

                return response.Categorias
                    .Select(cat => new CategoriaMusicalDTO
                    {
                        idCategoriaMusical = cat.IdCategoriaMusical,
                        nombre = cat.Nombre,
                        descripcion = cat.Descripcion
                    })
                    .ToList();
            }
            catch (RpcException ex)
            {
                MessageBox.Show($"Error gRPC: {ex.Status.Detail}");
                return new List<CategoriaMusicalDTO>();
            }
        }

        public async Task<bool> EditarCategoriaMusicalAsync(CategoriaMusicalDTO categoria)
        {
            try
            {
                var request = new CategoriaMusicalEditRequest
                {
                    IdCategoriaMusical = categoria.idCategoriaMusical ?? 0,
                    Nombre = categoria.nombre,
                    Descripcion = categoria.descripcion
                };

                var response = await grpcClient.EditarCategoriaMusicalAsync(request);

                MessageBox.Show($"Categoría actualizada: {response.Nombre}");
                return true;
            }
            catch (RpcException ex)
            {
                MessageBox.Show($"Error al editar categoría: {ex.Status.Detail}");
                return false;
            }
        }

        /*public async Task<List<CategoriaMusicalDTO>> ObtenerCategoriasMusicalesAsync()
        {
            try
            {
                HttpResponseMessage response = await ClienteAPI.HttpClient.GetAsync("categoriasMusicales");
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
                    MessageBox.Show("No se encontró el objeto 'datos' en la respuesta.");
                    return null;
                }

                var categorias = datos.ToObject<List<CategoriaMusicalDTO>>();
                return categorias ?? new List<CategoriaMusicalDTO>();
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

        /*public async Task<bool> RegistrarCategoriaMusicalAsync(CategoriaMusicalDTO categoria)
        {
            try
            {
                var json = JsonConvert.SerializeObject(categoria);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await ClienteAPI.HttpClient.PostAsync("categoriasMusicales/registrar", content);

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

        public async Task<bool> EditarCategoriaMusicalAsync(CategoriaMusicalDTO categoria)
        {
            try
            {
                int idCategoria = categoria.idCategoriaMusical.GetValueOrDefault();
                categoria.idCategoriaMusical = null;
                var json = JsonConvert.SerializeObject(categoria);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                Console.Write("id primero: " + idCategoria);
                Console.Write("id despues: " + categoria.idCategoriaMusical);
                HttpResponseMessage response = await ClienteAPI.HttpClient.PutAsync($"categoriasMusicales/{idCategoria}", content);

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
        }*/
    }
}
