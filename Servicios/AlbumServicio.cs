using ClienteMusAPI.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
                        form.Add(fileContent, "foto", Path.GetFileName(album.FotoPath));
                    }

                    var response = await ClienteAPI.HttpClient.PostAsync("/api/albumes/crear", form);
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al crear álbum: " + ex.Message);
                return false;
            }
        }

    }
}
