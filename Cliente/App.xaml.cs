using Cliente.src.Services;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;
using Cliente.src.Extencions;

namespace Cliente
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            init();
        }

        private void init()
        {
            ComponetesHelp.RegistrarComponentesFormulario();
        }

        private void ContarDijitos(int numero)
        {
            int copia = numero;
            int contador = 0;

            numero = Math.Abs(numero); // Asegurarse que sea positivo
            if (numero == 0) contador = 1;
            else
            {
                // Usar un bucle while para contar los dígitos
                while (numero > 0)
                {
                    numero = numero / 10; // División entera
                    contador++;
                }
            }

            Console.WriteLine($"El número {copia} tiene {contador} dígitos.");
        }

        public static void ReiniciarAplicacion()
        {
            // Obtén la ruta del ejecutable actual
            string exePath = Process.GetCurrentProcess().MainModule?.FileName ?? "";

            if (string.IsNullOrEmpty(exePath))
                MessageBox.Show("Error Inesperado Ejecute la aplicacion de nuevo.");
            else
                Process.Start(exePath);

            // Lanza una nueva instancia

            // Cierra la instancia actual
            Application.Current.Shutdown();
        }
    }

}
