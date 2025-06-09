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
                    MessageBox.Show($"Error al crear lista: {response.StatusCode}\n{responseContent}");
                    return false;
                }

                MessageBox.Show("Lista de reproducción creada con éxito.");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Excepción: {ex.Message}");
                return false;
            }
        }
    }
}
