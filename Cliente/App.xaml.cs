using Cliente.src.Services;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;

namespace Cliente
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            //init();
        }

        private async void init()
        {
            await AuthService.BorrarCredenciales();
            AuthService.DeleteToken();

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
