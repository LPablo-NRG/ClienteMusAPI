using ClienteMusAPI.DTOs;
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
                    MessageBox.Show($"Error al obtener notificaciones: {response.StatusCode}\n{responseContent}");
                    return null;
                }

                var notificaciones = JsonConvert.DeserializeObject<List<NotificacionDTO>>(responseContent);
                return notificaciones ?? new List<NotificacionDTO>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Excepción al obtener notificaciones: {ex.Message}");
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
                    MessageBox.Show($"Error al marcar como leída: {response.StatusCode}\n{responseContent}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Excepción al marcar como leída: {ex.Message}");
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
                    MessageBox.Show($"Error al marcar todas como leídas: {response.StatusCode}\n{responseContent}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Excepción al marcar todas como leídas: {ex.Message}");
                return false;
            }
        }

    }
}
