using ClienteMusAPI.DTOs;
using ClienteMusAPI.Servicios;
using ClienteMusAPI.Ventanas.Perfiles;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ClienteMusAPI.Clases
{
    public static class Reproductor
    {
        public static List<BusquedaCancionDTO> listaCanciones;
        private static IWavePlayer waveOutDevice;
        private static AudioFileReader audioFileReader;
        public static event Action OnReproduccionIniciada;
        public static event Action OnReproductorPausado;
        public static event Action OnReproduccionReaunudada;
        public static event Action OnReproduccionFinalizada;
        public static int indiceActual = 0;
        private static float volumenActual = 1.0f;
        public static TimeSpan Duracion => audioFileReader?.TotalTime ?? TimeSpan.Zero;
        public static TimeSpan Posicion
        {
            get => audioFileReader?.CurrentTime ?? TimeSpan.Zero;
            set
            {
                if (audioFileReader != null && value < audioFileReader.TotalTime)
                    audioFileReader.CurrentTime = value;
            }
        }

        public static void Detener()
        {
            OnReproductorPausado?.Invoke();
            waveOutDevice?.Stop();
            waveOutDevice?.Dispose();
            waveOutDevice = null;

            audioFileReader?.Dispose();
            audioFileReader = null;
        }

        public static void PausarReanudar()
        {
            if (waveOutDevice == null) return;

            switch (waveOutDevice.PlaybackState)
            {
                case PlaybackState.Playing:
                    waveOutDevice.Pause();
                    OnReproductorPausado?.Invoke();
                    break;
                case PlaybackState.Paused:
                    waveOutDevice.Play();
                    OnReproduccionReaunudada?.Invoke();
                    break;
                case PlaybackState.Stopped:
                    waveOutDevice.Play();
                    OnReproduccionIniciada?.Invoke();
                    break;
            }
        }

        public static float Volumen
        {
            get => audioFileReader?.Volume ?? 1.0f;
            set
            {
                volumenActual = value < 0 ? 0f : value > 1 ? 1f : value;

                if (audioFileReader != null)
                    audioFileReader.Volume = volumenActual;
            }
        }

        public static async Task ReproducirCancionAsync(List<BusquedaCancionDTO> canciones, int indice)
        {
            if (canciones == null || canciones.Count == 0 || indice < 0 || indice >= canciones.Count)
                return;
            listaCanciones = canciones;
            indiceActual = indice;

            Detener();

            try
            {
                var cancion = canciones[indice];
                var respuesta = await ClienteAPI.HttpClient.GetAsync(Constantes.URL_BASE + cancion.urlArchivo);

                var tempPath = Path.Combine(Path.GetTempPath(), "cancion_temp.mp3");
                using (var fs = new FileStream(tempPath, FileMode.Create, FileAccess.Write))
                {
                    await respuesta.Content.CopyToAsync(fs);
                }

                audioFileReader = new AudioFileReader(tempPath);
                audioFileReader.Volume = volumenActual;
                waveOutDevice = new WaveOutEvent();
                waveOutDevice.Init(audioFileReader);
                waveOutDevice.Play();

                OnReproduccionIniciada?.Invoke();

                waveOutDevice.PlaybackStopped += async (s, e) =>
                {
                    if (audioFileReader != null)
                    {
                        audioFileReader.Position = 0;
                        OnReproductorPausado?.Invoke();
                        OnReproduccionFinalizada?.Invoke();
                        // Avanzar automáticamente a la siguiente si hay más
                        if (indiceActual + 1 < listaCanciones.Count)
                        {
                            audioFileReader.Position = 0;
                            indiceActual++;
                            await ReproducirCancionAsync(listaCanciones, indiceActual);
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al reproducir canción: " + ex.Message);
            }
        }

        public static async Task SiguienteCancionAsync()
        {
            if (indiceActual + 1 < listaCanciones.Count)
            {
                audioFileReader.Position = 0;
                indiceActual++;
                await ReproducirCancionAsync(listaCanciones, indiceActual);
            }
        }
        public static async Task CancionAnteriorAsync()
        {
            if (indiceActual > 0)
            {
                audioFileReader.Position = 0;
                indiceActual--;
                await ReproducirCancionAsync(listaCanciones, indiceActual);
            }
        }

    }
}
