﻿using ClienteMusAPI.Clases;
using ClienteMusAPI.DTOs;
using ClienteMusAPI.Servicios;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
        private BusquedaArtistaDTO perfilArtista { get; set; }

        public vtChat(BusquedaArtistaDTO perfilArtista, string nombreUsuario)
        {
            InitializeComponent();
            _idPerfilArtista = perfilArtista.idArtista;
            _nombreUsuario = nombreUsuario;
            this.perfilArtista = perfilArtista;
            ConectarWebSocket();
            CargarDatosChat();
        }

        public void CargarDatosChat()
        {
            txb_NombreChat.Text = "Chat de artista: " + perfilArtista.nombreUsuario;
            CargarImagen(perfilArtista.urlFoto);

        }

        private async void CargarImagen(string url)
        {
            if (!String.IsNullOrEmpty(url))
            {
                var bytes = await ClienteAPI.HttpClient.GetByteArrayAsync(Constantes.URL_BASE + url);
                using (var stream = new MemoryStream(bytes))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = stream;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();
                    img_PerfilChat.Source = image;
                }
            }
        }

        private async void ConectarWebSocket()
        {
            _webSocket = new ClientWebSocket();

            // monta la URL con el id de perfil, p.e. /ws/42
            var url = Constantes.URL_CHAT+_idPerfilArtista;
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

        private void Click_Volver(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
