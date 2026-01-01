using System.Diagnostics;
using System.Windows;
using Cliente.Default;
using Cliente.Helpers;
using Cliente.Obj.Model;
using Cliente.Services.Model;
using Cliente.ServicesHub;
using Utilidades.Dialogs;

namespace Cliente;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        Init();

        // PruebaTemp.EjecutarPrueba(); // Descomentar para ejecutar pruebas temporales
        // PruebaTemp.LuhnValidator("376653003447747"); // Descomentar para ejecutar pruebas temporales
    }

    public static List<string> AllPermsRoles { get; private set; } = new();

    private async void Init()
    {
        // IMPORTANTE:
        // Iniciar todos los hubs ANTES de crear cualquier ViewModel.
        // Esto garantiza que las colecciones ya estén listas 
        // y que ningún evento de SignalR se pierda.

       
        
        ComponetesHelp.RegistrarComponentesFormulario();
        DialogService.Instance.ReguisterIdDefault(DialogDefaults.Main);

        var resul = await ((ServiceRol)ServiceFactory.GetService<Rol>()).GetAllPermisos();
        AllPermsRoles = resul.EntityGet;
    }


    public static void ReiniciarAplicacion()
    {
        // Obtén la ruta del ejecutable actual
        var exePath = Process.GetCurrentProcess().MainModule?.FileName ?? "";

        if (string.IsNullOrEmpty(exePath))
            MessageBox.Show("Error Inesperado Ejecute la aplicacion de nuevo.");
        else
            Process.Start(exePath);

        // Lanza una nueva instancia

        // Cierra la instancia actual
        Current.Shutdown();
    }
}