using System.Linq;
using System.Windows;

namespace Utilidades.Services;

public class NavigationService
{
    public static void CloseWindow<T>() where T : Window
    {
        var ventana = Application.Current.Windows.OfType<T>().FirstOrDefault();
        ventana?.Close();
    }

    public static void OpenWindow<T>() where T : Window, new()
    {
        new T().Show();
    }
}