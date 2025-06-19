using ClienteMusAPI.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClienteMusAPI.Ventanas.Perfiles.Chat
{
    /// <summary>
    /// Lógica de interacción para vtChat.xaml
    /// </summary>
    public partial class vtChat : Page
    {
        private ClientWebSocket _webSocket;
        private int _idPerfilArtista;
        private string _nombreUsuario;

        public vtChat(int idPerfilArtista, string nombreUsuario)
        {
            InitializeComponent();
            _idPerfilArtista = idPerfilArtista;
            _nombreUsuario = nombreUsuario;
            ConectarWebSocket();
        }

        private async void ConectarWebSocket()
        {
            _webSocket = new ClientWebSocket();

            // monta la URL con el id de perfil, p.e. /ws/42
            var url = $"ws://localhost:8080/ws/{_idPerfilArtista}";
            await _webSocket.ConnectAsync(new Uri(url), CancellationToken.None);

            _ = EscucharMensajes();
        }



        private async Task EscucharMensajes()
        {
            var buffer = new byte[4096];

            while (_webSocket.State == WebSocketState.Open)
            {
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                var mensajeJson = Encoding.UTF8.GetString(buffer, 0, result.Count);
                var mensaje = JsonConvert.DeserializeObject<ChatMessageDTO>(mensajeJson);

                Dispatcher.Invoke(() =>
                {
                    var bloque = new TextBlock
                    {
                        Text = $"{mensaje.NombreUsuario}: {mensaje.Mensaje}",
                        Margin = new Thickness(5)
                    };
                    sp_Mensajes.Children.Add(bloque);
                });
            }
        }

        private async void EnviarMensaje_Click(object sender, RoutedEventArgs e)
        {
            if (_webSocket.State == WebSocketState.Open && !string.IsNullOrWhiteSpace(txtMensaje.Text))
            {
                var msg = new ChatMessageDTO
                {
                    NombreUsuario = _nombreUsuario,
                    Mensaje = txtMensaje.Text,
                    IdPerfilArtista = _idPerfilArtista
                };

                var json = JsonConvert.SerializeObject(msg);
                var buffer = Encoding.UTF8.GetBytes(json);
                await _webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                txtMensaje.Clear();
            }
        }
    }
}
