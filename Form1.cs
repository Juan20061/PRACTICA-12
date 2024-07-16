using System.IO;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Policy;
using System.Data.SqlClient;
using static System.Net.WebRequestMethods;

namespace DemoIntroAsync3716370
{
    public partial class Form1 : Form
    {
        HttpClient httpClient = new HttpClient();
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;

            var directorioActual = AppDomain.CurrentDomain.BaseDirectory;
            var destinoBaseSecuencial = Path.Combine(directorioActual, @"Imagenes\resultado-secuencial");
            var destinoBaseParalelo = Path.Combine(directorioActual, @"Imagenes\resultado-paralelo");
            PrepararEjecucion(destinoBaseParalelo, destinoBaseSecuencial);

            Console.WriteLine("inicio");
            List<Imagen> imagens = ObtenerImagenes();

            var sw = new Stopwatch();
            sw.Start();            

            foreach(var imagen in imagens)
            {
                await ProcesarImagen(destinoBaseSecuencial, imagen);
                
            }

            Console.WriteLine("Secuencial - duracion en segundos: {0}",
            sw.ElapsedMilliseconds / 1000.0);

            sw.Reset();

            sw.Start();

            var tareasEnumerable = imagens.Select(async imagen =>
            {
                await ProcesarImagen(destinoBaseParalelo, imagen);
            });

            await Task.WhenAll(tareasEnumerable);
            Console.WriteLine("Paralelo - duracion en segundos: {0}",
                sw.ElapsedMilliseconds / 1000.0);

            sw.Stop();
            pictureBox1.Visible = false;
        }

        private async Task ProcesarImagen(string directorio, Imagen imagen)
        {
            var respuesta = await httpClient.GetAsync(imagen.URL);
            var contenido = await respuesta.Content.ReadAsByteArrayAsync();

            Bitmap bitmap;
            using (var ms = new MemoryStream(contenido))
            {
                bitmap = new Bitmap(ms);
            }

            bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
            var destino = Path.Combine(directorio, imagen.Nombre);
            bitmap.Save(destino);
        }

        private static List<Imagen> ObtenerImagenes()
        {
            var imagenes = new List<Imagen>();

            for (int i = 0; i < 7; i++)
            {
                imagenes.Add(
                  new Imagen()
                  {
                      Nombre = $"Capibara{i}.png",
                      URL = "https://th.bing.com/th/id/OIP.fuvJEGZZoaG8v6ntgG9jEwHaE7?w=3800&h=2533&rs=1&pid=ImgDetMain"
                  });
                imagenes.Add(
                  new Imagen()
                  {
                      Nombre = $"Mapache {i}.jpg",
                      URL = "https://th.bing.com/th/id/OIP.Alwu7QGPD3TRXXpbuQnLeAHaFk?rs=1&pid=ImgDetMain"
                  });
                imagenes.Add(
                 new Imagen()
                 {
                     Nombre = $"Loro {i}.jpg",
                     URL = "https://th.bing.com/th/id/OIP.Alwu7QGPD3TRXXpbuQnLeAHaFk?rs=1&pid=ImgDetMain"
                 });

            }
            return imagenes;
        }

        private void BorrarArchivos(string directorio)
        {
            var archivos = Directory.EnumerateFiles(directorio);
            foreach(var archivo in archivos)
            {
                System.IO.File.Delete(archivo);
            }
        }

        private void PrepararEjecucion(string destinoBaseParalelo,
            string destinoBaseSecuencial)
        {
            if (!Directory.Exists(destinoBaseParalelo))
            {
                Directory.CreateDirectory(destinoBaseParalelo);

            }
            if (!Directory.Exists(destinoBaseSecuencial))
            {
                Directory.CreateDirectory(destinoBaseSecuencial);

            }
            BorrarArchivos(destinoBaseSecuencial);
            BorrarArchivos(destinoBaseParalelo);
        }
        
        private async Task<string> ProcesamientoLargo()
        {
            await Task.Delay(3000);
            return "Felipe";
        }

        private async Task RealizarProcesamientoLargoA()
        {
            await Task.Delay(1000);
            Console.WriteLine("El proceso A finalizado");
        }

        private async Task RealizarProcesamientoLargoB()
        {
            await Task.Delay(1000);
            Console.WriteLine("El proceso B finalizado");
        }

        private async Task RealizarProcesamientoLargoC()
        {
            await Task.Delay(1000);
            Console.WriteLine("El proceso C finalizado");
        }
    }
}
