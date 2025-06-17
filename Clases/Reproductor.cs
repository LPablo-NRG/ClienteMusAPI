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
        public static event Action OnReproductorPausadoODetenido;



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

        public static async Task ReproducirDesdeAPIAsync()
        {
            Detener();

            try
            {
                var respuesta = await ClienteAPI.HttpClient.GetAsync(Constantes.URL_BASE + listaCanciones[0].urlArchivo);

                var tempPath = Path.Combine(Path.GetTempPath(), "cancion_temp.mp3");
                using (var fs = new FileStream(tempPath, FileMode.Create, FileAccess.Write))
                {
                    await respuesta.Content.CopyToAsync(fs);
                }

                audioFileReader = new AudioFileReader(tempPath);
                waveOutDevice = new WaveOutEvent();
                waveOutDevice.Init(audioFileReader);
                waveOutDevice.Play();
                OnReproduccionIniciada?.Invoke();

                Console.WriteLine("Reproduciendo archivo descargado");
                waveOutDevice.PlaybackStopped += (s, e) =>
                {
                    if (audioFileReader != null)
                    {
                        // Reinicia a 00:00, pero no reproduce
                        audioFileReader.Position = 0;
                        Console.WriteLine("Canción finalizada. Lista para repetir.");
                        OnReproductorPausadoODetenido?.Invoke();
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al reproducir: " + ex.Message);
            }
        }



        public static void Detener()
        {
            OnReproductorPausadoODetenido?.Invoke();
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
                    OnReproductorPausadoODetenido?.Invoke();
                    break;
                case PlaybackState.Paused:
                    waveOutDevice.Play();
                    OnReproduccionIniciada?.Invoke();
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
                if (audioFileReader != null)
                {
                    audioFileReader.Volume = value < 0 ? 0f : value > 1 ? 1f : value;
                }

            }
        }
    }
}
