using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using Shared.Interfaces.Model;
using Utilidades.Interfaces;

namespace Utilidades.Dialogs;

public class DialogService : IDialogService
{
    private static readonly Lazy<DialogService> _instance = new(() => new DialogService());


    private readonly Dictionary<string, bool> _dialogosAbiertos = new();
    private string? idDialogDefault;
    public static DialogService Instance => _instance.Value;
    public SnackbarMessageQueue MensajeQueue { get; } = new(TimeSpan.FromSeconds(2));

    public async Task MostrarDialogo(IDialogBase dialogTo)
    {
        for (var intentos = 0; intentos < 2; intentos++)
        {
            if (!await CerrarSiEstaAbiertoYEsperar(dialogTo.DialogOpenIdentifier))
                break;

            await Task.Delay(100);
        }

        try
        {
            await DialogHost.Show(
                dialogTo,
                dialogTo.DialogOpenIdentifier,
                new DialogOpenedEventHandler((sender, args) =>
                {
                    if (dialogTo is IDialogLifecycle dialog)
                        dialog.OnOpened();
                }));
            EstablecerEstado(dialogTo.DialogOpenIdentifier, true);
        }
        catch (Exception ex)
        {
            MensajeQueue.Enqueue($"Error al mostrar el diálogo: {ex.Message}");
        }
        finally
        {
            EstablecerEstado(dialogTo.DialogOpenIdentifier, false);
        }
    }

    public async Task MostrarDialogo(string content, string dial)
    {
        var dialog = new MessageDialog { Message = content, DialogOpenIdentifier = dial };
        await MostrarDialogo(dialog);
    }

    public async Task MostrarDialogoError<T>(IResultResponse<T> content, string dial)
    {
        var dialog = new MessageDialogError
        {
            DialogOpenIdentifier = dial,
            ErrorResponse = (IResultResponse)content
        };
        await MostrarDialogo(dialog);
    }
    
    

    public async Task<IResultResponse<T>> MostrarDialogoProgreso<T>(Func<Task<IResultResponse<T>>> accion,
        string nameIdentifier)

    {
        IResultResponse<T>? resultado = null;

        if (await CerrarSiEstaAbiertoYEsperar(nameIdentifier))
            return await MostrarDialogoProgreso(accion, nameIdentifier); // retry limpio

        EstablecerEstado(nameIdentifier, true);
        try
        {
            var progressDialog = new ProgressDialog
            {
                DialogOpenIdentifier = nameIdentifier
            };

            await DialogHost.Show(progressDialog, progressDialog.DialogOpenIdentifier,
                new DialogOpenedEventHandler(async (sender, args) =>
                {
                    resultado = await accion();
                    await CerrarSiEstaAbiertoYEsperar(progressDialog);
                })
            );


            // Espera a que DialogHost.Show cierre
            while (resultado == null) await Task.Delay(50); // evita spinlock

            return resultado;
        }
        finally
        {
            EstablecerEstado(nameIdentifier, false);
        }
    }

    /// <summary>
    ///     Valida la respuesta de una operación y muestra mensajes o diálogos según corresponda.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="resp"></param>
    /// <param name="dialogOpen">Identificador donde el dialogo de error se abrira.</param>
    /// <param name="menssagePersonalizado"></param>
    /// <returns></returns>
    public async Task ValidarRespuesta<T>(IResultResponse<T> resp, string? dialogOpen = null,
        string? menssagePersonalizado = null)
    {
        if (string.IsNullOrEmpty(dialogOpen))
            dialogOpen = idDialogDefault;

        if (!resp.Success)
        {
            await MostrarDialogoError(resp, dialogOpen);
            return;
        }

        // Ignorar GET o si no hay tipo involucrado
        if (resp.Method == HttpMethod.Get || resp.ObjInteration is null)

            return;

        // Solo para tipos que implementen IModelObj
        if (!typeof(IModelObj).IsAssignableFrom(resp.ObjInteration))
            return;

        // Obtener el nombre de la entidad
        var nombreEntidad = resp.ObjInteration.Name;

        // Crear el mensaje en base al método
        string mensaje;

        if (resp.Method == HttpMethod.Post)
            mensaje = $"El {nombreEntidad} fue creado correctamente.";
        else if (resp.Method == HttpMethod.Put)
            mensaje = $"El {nombreEntidad} fue actualizado correctamente.";
        else if (resp.Method == HttpMethod.Delete)
            mensaje = $"El {nombreEntidad} fue eliminado correctamente.";
        else
            mensaje = "Operación exitosa.";

        if (!string.IsNullOrEmpty(menssagePersonalizado))
            mensaje = menssagePersonalizado;

        MensajeQueue.Enqueue(mensaje);
    }

    public void ReguisterIdDefault(string id)
    {
        idDialogDefault = id;
    }

    public async Task DialogTest(Dictionary<byte, object> dialogTo, string dial)
    {
        var dialog = new TestDialog { Message = dialogTo, DialogOpenIdentifier = dial };
        await MostrarDialogo(dialog);
    }

    public async Task<bool> CerrarSiEstaAbiertoYEsperar(IDialogBase dialogId)
    {
        return await CerrarSiEstaAbiertoYEsperar(dialogId.DialogOpenIdentifier);
    }

    public async Task<bool> CerrarSiEstaAbiertoYEsperar(string dialogId)
    {
        if (EstaAbierto(dialogId) || DialogHost.IsDialogOpen(dialogId))
        {
            DialogHost.Close(dialogId);
            EstablecerEstado(dialogId, false);

            await Task.Delay(100); // Previene loops u errores de render
            return true;
        }

        return false;
    }

    private bool EstaAbierto(string dialogId)
    {
        return _dialogosAbiertos.TryGetValue(dialogId, out var abierto) && abierto;
    }

    private void EstablecerEstado(string dialogId, bool abierto)
    {
        _dialogosAbiertos[dialogId] = abierto;
    }
}