using Cliente.src.View.Dialog;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Shared.Interfaces.ModelsBase;
using System.Net.Http;
using Utilidades.Interfaces;

namespace Cliente.src.Services
{
    public class DialogService : IDialogService
    {
        private static readonly Lazy<DialogService> _instance = new(() => new DialogService());
        public static DialogService Instance => _instance.Value;

        public static string DialogIdentifierMain { get; } = $"Dialog_ViewMain";
        public static string DialogIdentifierProgress { get; } = $"Dialog_ViewProgress";
        public static string DialogSub01 { get; } = $"Dialog_Sub01";
        public static string DialogSub02 { get; } = $"Dialog_Sub02";

        public SnackbarMessageQueue MensajeQueue { get; } = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));


        // Múltiples banderas por identificador
        private readonly Dictionary<string, bool> _dialogosAbiertos = new();

        private DialogService() { }
        public async Task MostrarDialogoError<T>(IResultResponse<T> content, string dial = "")
        {
            string diID = "";
            if (string.IsNullOrEmpty(dial))
                diID = DialogIdentifierMain;
            else
                diID = dial;

            MessageDialogError dialog = new MessageDialogError
            {
                DialogOpenIdentifier = diID,
                ErrorResponse = content
            };
            await MostrarDialogo(dialog);
        }
        public async Task MostrarDialogo(string content, string dial = "")
        {
            string diID = "";
            if (string.IsNullOrEmpty(dial))
                diID = DialogIdentifierMain;
            else
                diID = dial;

            MessageDialog dialog = new MessageDialog { Message = content, DialogOpenIdentifier = diID };
            await MostrarDialogo(dialog);
        }
        public async Task MostrarDialogo(IDialog dialogTo)
        {
            if (await CerrarSiEstaAbiertoYEsperar(dialogTo.DialogOpenIdentifier))
            {
                await MostrarDialogo(dialogTo);
                return;
            }

            try
            {
                await DialogHost.Show(dialogTo, dialogTo.DialogOpenIdentifier);
                EstablecerEstado(dialogTo.DialogOpenIdentifier, true);
            }
            catch (Exception ex)
            {
                EstablecerEstado(dialogTo.DialogOpenIdentifier, false);
                MensajeQueue.Enqueue($"Error al mostrar el diálogo: {ex.Message}");
            }
            finally
            {
                EstablecerEstado(dialogTo.DialogOpenIdentifier, false);
            }
        }
        public async Task<IResultResponse<T>> MostrarDialogoProgreso<T>(Func<Task<IResultResponse<T>>> accion, string? nameIdentifier = null)

        {
            var dialogId = nameIdentifier ?? DialogIdentifierProgress;
            IResultResponse<T>? resultado = null;

            if (await CerrarSiEstaAbiertoYEsperar(dialogId))
            {
                return await MostrarDialogoProgreso(accion, nameIdentifier); // retry limpio
            }

            EstablecerEstado(dialogId, true);
            try
            {
                var progressDialog = new ProgressDialog()
                {
                    DialogOpenIdentifier = nameIdentifier ?? DialogIdentifierProgress,
                };

                await DialogHost.Show(progressDialog, progressDialog.DialogOpenIdentifier,
                    openedEventHandler: new DialogOpenedEventHandler(async (sender, args) =>
                    {
                        resultado = await accion();
                        await CerrarSiEstaAbiertoYEsperar(progressDialog);
                    })
                );


                // Espera a que DialogHost.Show cierre
                while (resultado == null)
                {
                    await Task.Delay(50); // evita spinlock
                }

                return resultado;
            }
            finally
            {
                EstablecerEstado(dialogId, false);
            }
        }


        public static async Task<T?> MostrarFormularioDinamicoAsyncMain<T>(T instancia, string header, Func<T, Task> aceptar)
            where T : class
        {
            await MostrarFormularioDinamicoAsync(instancia, header, new AsyncRelayCommand<T>(aceptar),
                DialogIdentifierMain, DialogSub01);

            return instancia;
        }
        public static async Task<T?> MostrarFormularioDinamicoAsyncSubDialog01<T>(T instancia, string header, Func<T, Task> aceptar)
            where T : class
        {
            await MostrarFormularioDinamicoAsync(instancia, header, new AsyncRelayCommand<T>(aceptar),
                DialogSub01, DialogSub02);

            return instancia;
        }
        public static async Task<T?> MostrarFormularioDinamicoAsync<T>(T instancia, string header, IAsyncRelayCommand<T> aceptarCommand, string openId, string nameId)
            where T : class
        {
            var form = new FormularioDinamico(instancia)
            {
                TextHeader = header,
                AceptarCommand = aceptarCommand,
                DialogOpenIdentifier = openId,
                DialogNameIdentifier = nameId
            };

            await Instance.MostrarDialogo(form);
            return instancia;
        }



        public async Task<bool> CerrarSiEstaAbiertoYEsperar(IDialog dialogId)
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



        private bool EstaAbierto(string dialogId) => _dialogosAbiertos.TryGetValue(dialogId, out var abierto) && abierto;
        private void EstablecerEstado(string dialogId, bool abierto) =>
            _dialogosAbiertos[dialogId] = abierto;


        public async Task ValidarRespuesta<T>(IResultResponse<T> resp, string dialog = "")
        {
            if (!resp.Success)
            {
                await MostrarDialogoError(resp);
                return;
            }

            // Ignorar GET o si no hay tipo involucrado
            if (resp.Method == HttpMethod.Get || resp.ObjInteration is null)
                return;

            // Solo para tipos que implementen IModelObj
            if (!typeof(IModelObj).IsAssignableFrom(resp.ObjInteration))
                return;

            // Obtener el nombre de la entidad
            string nombreEntidad = resp.ObjInteration.Name;

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

            if (!string.IsNullOrEmpty(dialog))
                mensaje = dialog;

            MensajeQueue.Enqueue(mensaje);
        }

    }
}
