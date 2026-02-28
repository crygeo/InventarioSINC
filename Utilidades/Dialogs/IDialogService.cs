using System;
using System.Threading.Tasks;
using Utilidades.Interfaces;

namespace Utilidades.Dialogs;

public interface IDialogService
{
    Task MostrarDialogo(IDialogBase dialogTo);
    Task MostrarDialogo(string content, string dial);
    Task MostrarDialogoError<T>(IResultResponse<T> content, string dial);
    Task<IResultResponse<T>> MostrarDialogoProgreso<T>(Func<Task<IResultResponse<T>>> accion, string nameIdentifier);
    Task ValidarRespuesta<T>(IResultResponse<T> resp, string? dialogOpen = null, string? mensajePersonalizado = null);
}