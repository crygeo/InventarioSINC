using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Utilidades.Dialogs;

namespace Cliente.Extencions;

public static class CommandExtensions
{
    public static async Task TryEjecutarYCerrarDialogoAsync(this IAsyncRelayCommand? command, IDialog dialog,
        object? parametros = null)
    {
        await command.TryEjecutarAsync(parametros);

        await DialogService.Instance.CerrarSiEstaAbiertoYEsperar(dialog);
    }

    public static async Task TryEjecutarYCerrarDialogoAsync<T>(this IAsyncRelayCommand? command, IDialog<T> dialog,
        object? parametros = null)
    {
        await command.TryEjecutarAsync(parametros);

        await DialogService.Instance.CerrarSiEstaAbiertoYEsperar(dialog);
    }

    public static async Task TryEjecutarAsync(this IAsyncRelayCommand? command, object? parametros = null)
    {
        if (command?.CanExecute(parametros) == true)
            await command.ExecuteAsync(parametros);
    }
}