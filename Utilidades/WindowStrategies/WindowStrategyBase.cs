using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Utilidades.Mvvm;

namespace Utilidades.WindowStrategies;

public abstract class WindowStrategyBase<T> : IWindowStrategy where T : Window
{
    private Window? Window;

    public void CloseWindow()
    {
        if (Window == null) Window.Close();
    }

    public void OpenWindow()
    {
        if (Window == null)
        {
            Window = Activator.CreateInstance<T>();
            //Window.Closing += Win_Closing;
            Window.Closed += (sender, args) => Window = null;
            Window.Show();
        }
        else
        {
            if (Window.WindowState == WindowState.Minimized) Window.WindowState = WindowState.Normal;

            Window.Activate();
        }
    }

    public void ShowWindow()
    {
        if (Window == null) Window.Show();
    }

    public static void CloseWindow<T>() where T : Window
    {
        var ventana = Application.Current.Windows.OfType<T>().FirstOrDefault();
        ventana?.Close();
    }

    private void Win_Closing(object? sender, CancelEventArgs e)
    {
        if (Window == null) return;

        var data = (FrameworkElement)Window.FindName("MainDataContex");

        if (data != null)
        {
            if (data.DataContext is ViewModelBase viewModel) viewModel.CerrarVentanaCommand.Execute(null);
            //throw new InvalidOperationException("El contenedor no contiene ViewModel indicado.");
        }
        else
        {
            // Manejar el caso en que el Grid no se encuentre
            throw new InvalidOperationException("No existe ningun contenedor con el nombre MainDataContex");
        }
    }
}