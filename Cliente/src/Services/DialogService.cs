using Cliente.src.View.Dialog;
using MaterialDesignThemes.Wpf;
<<<<<<< HEAD
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Utilidades.Interfaces;

namespace Cliente.src.Services
{
    public class DialogService : IDialogService
    {
        private static readonly Lazy<DialogService> _instance = new(() => new DialogService());
        public static DialogService Instance => _instance.Value;

        private bool _dialogoAbierto = false;

        public static string DialogIdentifier => "MainView";

        private DialogService()
        {
        }

        public async Task MostrarDialogo(object content)
        {
            if (_dialogoAbierto || DialogHost.IsDialogOpen(DialogIdentifier))
                return;

            _dialogoAbierto = true;
            try
            {
                await DialogHost.Show(content, DialogIdentifier);
            }
            finally
            {
                _dialogoAbierto = false;
            }
        }

        public async Task MostrarDialogo(string content)
        {
            MessageDialog dialog = new MessageDialog { Message = content };
            await MostrarDialogo(dialog);
        }

        public async Task MostrarDialogoProgreso<T>(Func<Task<IResultResponse<T>>> accion, string? nameIdentifier = null)
        {
            var dialogId = nameIdentifier ?? DialogIdentifier;

            if (_dialogoAbierto || DialogHost.IsDialogOpen(dialogId))
            {
                DialogHost.Close(dialogId);
                _dialogoAbierto = false;

                await Task.Delay(100); // Prevent loop risk
                await MostrarDialogoProgreso(accion, nameIdentifier);
                return;
            }

            _dialogoAbierto = true;
            try
            {
                var progressDialog = new ProgressDialog();

                await DialogHost.Show(progressDialog, dialogId, openedEventHandler: async (sender, args) =>
                {
                    var result = await accion();
                    DialogHost.Close(dialogId);
                });
            }
            finally
            {
                _dialogoAbierto = false;
            }
        }





        public async Task ValidarRespuesta<T>(IResultResponse<T> resp)
        {
            if (resp.Success)
                return;
            else
                await MostrarDialogo(resp.Message);
        }
    }
=======
using Utilidades.Interfaces;

public class DialogService : IDialogService
{
    private static readonly Lazy<DialogService> _instance = new(() => new DialogService());
    public static DialogService Instance => _instance.Value;

    public static string DialogIdentifierMain => "DialogMain";
    public static string DialogIdentifierProgress => "DialogProgrees";

    // Múltiples banderas por identificador
    private readonly Dictionary<string, bool> _dialogosAbiertos = new();

    private DialogService() { }

    public async Task MostrarDialogo(string content)
    {
        MessageDialog dialog = new MessageDialog { Message = content };
        await MostrarDialogo(dialog);
    }

    private bool EstaAbierto(string dialogId) =>
        _dialogosAbiertos.TryGetValue(dialogId, out var abierto) && abierto;

    private void EstablecerEstado(string dialogId, bool abierto) =>
        _dialogosAbiertos[dialogId] = abierto;

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

    public async Task MostrarDialogo(object content, string? nameIdentifier = null)
    {
        var dialogId = nameIdentifier ?? DialogIdentifierMain;

        if (await CerrarSiEstaAbiertoYEsperar(dialogId))
        {
            await MostrarDialogo(content, nameIdentifier);
            return;
        }

        EstablecerEstado(dialogId, true);
        try
        {
            await DialogHost.Show(content, dialogId);
        }
        finally
        {
            EstablecerEstado(dialogId, false);
        }
    }

    public async Task MostrarDialogoProgreso<T>(Func<Task<IResultResponse<T>>> accion, string? nameIdentifier = null)
    {
        var dialogId = nameIdentifier ?? DialogIdentifierProgress;

        if (await CerrarSiEstaAbiertoYEsperar(dialogId))
        {
            await MostrarDialogoProgreso(accion, nameIdentifier);
            return;
        }

        EstablecerEstado(dialogId, true);
        try
        {
            var progressDialog = new ProgressDialog();

            await DialogHost.Show(progressDialog, dialogId, openedEventHandler: async (sender, args) =>
            {
                var result = await accion();
                DialogHost.Close(dialogId);
            });
        }
        finally
        {
            EstablecerEstado(dialogId, false);
        }
    }

    public async Task ValidarRespuesta<T>(IResultResponse<T> resp)
    {
        if (!resp.Success)
            await MostrarDialogo(resp.Message);
    }
>>>>>>> 29/05/2025
}
