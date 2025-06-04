using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cliente.src.Services;
using Utilidades.Interfaces;

namespace Cliente.src.Extencions
{
    public static class CommandExtensions
    {
        public static async Task TryEjecutarYCerrarDialogoAsync(this IAsyncRelayCommand? command, IDialog dialog, object? parametros = null)
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
}
