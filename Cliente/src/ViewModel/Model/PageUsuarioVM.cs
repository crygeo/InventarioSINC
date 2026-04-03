// Cliente/src/ViewModel/Model/PageUsuarioVM.cs
using Cliente.Default;
using Cliente.Obj.Model;
using Cliente.Services.Model;
using Cliente.View.Dialog;
using CommunityToolkit.Mvvm.Input;
using Shared.Extensions;
using Utilidades.Dialogs;
using System.Windows;
using Utilidades.Mvvm;

namespace Cliente.ViewModel.Model;

/// <summary>
/// VM para gestión de usuarios.
///
/// Responsabilidades:
/// - CRUD estándar (heredado de ViewModelServiceBase).
/// - Operaciones especiales: cambiar password, asignar/quitar rol.
///
/// Las operaciones especiales se delegan al ServiceUsuario —
/// el VM solo orquesta diálogo → servicio → feedback.
///
/// Principio: el VM no sabe cómo se muestra el diálogo,
/// solo sabe qué hacer con el resultado.
/// </summary>
public partial class PageUsuarioVM : ViewModelServiceBase<Usuario>
{
    private readonly ServiceUsuario _serviceUsuario;

    public PageUsuarioVM()
    {
        _serviceUsuario = (ServiceUsuario)ServicioBase;

        CambiarPasswordCommand    = new AsyncRelayCommand<Usuario>(
            CambiarPasswordAsync,
            wrapper => wrapper is not null);

        AsignarRolCommand         = new AsyncRelayCommand<Rol>(
            AsignarRolAsync,
            args => args is not null);
    }

    // ==============================
    // COMANDOS ESPECÍFICOS
    // ==============================

    /// <summary>
    /// Recibe el wrapper del usuario sobre el cual actuar —
    /// elimina el acoplamiento implícito a EntitySelect.
    /// </summary>
    public IAsyncRelayCommand<Usuario> CambiarPasswordCommand { get; }

    /// <summary>
    /// Args tipados para evitar object boxing y acoplamiento implícito.
    /// </summary>
    public IAsyncRelayCommand<Rol> AsignarRolCommand { get; }

    // ==============================
    // CRUD SOBREESCRITO — UI específica
    // ==============================

    public override async Task CreateAsync()
    {
        var entity = new Usuario { FechaNacimiento = DateTime.Today };
        await ShowUserDialogAsync(entity, "Nuevo Usuario", ConfirmarCrearEntityAsync);
    }

    public override async Task UpdateAsync()
    {
        if (EntitySelect is null) return;
        var clone = EntitySelect.Model.Clone();
        await ShowUserDialogAsync(clone, "Editar Usuario", ConfirmarEditarUsuarioAsync);
    }

    // ==============================
    // OPERACIONES ESPECÍFICAS
    // ==============================

    private async Task CambiarPasswordAsync(Usuario? model)
    {
        if (model is null) return;

        var dialog = new ChangePassDialog
        {
            // Admin no necesita contraseña vieja — Collapsed lo indica
            OldPasswordRequired = Visibility.Collapsed,
            DialogOpenIdentifier = DialogDefaults.Main,
            AceptarCommand = new AsyncRelayCommand<ChangePassDialog?>(
                changePass => EjecutarCambioPasswordAsync(changePass, model))
        };

        await DialogServiceI.MostrarDialogo(dialog);
    }

    private async Task EjecutarCambioPasswordAsync(
        ChangePassDialog? changePass, Usuario usuario)
    {
        if (changePass is null) return;

        await DialogServiceI.MostrarDialogoProgreso(async () =>
        {
            var result = await _serviceUsuario.ChangePasswordAsync(
                usuario.Id,
                changePass.OldPassword,
                changePass.NewPassword);

            result.ObjInteration = typeof(Usuario);
            await DialogServiceI.ValidarRespuesta(result);
            return result;
        }, DialogDefaults.Progress);
    }

    private async Task AsignarRolAsync(Rol? rol)
    {
        if (rol is null || EntitySelectModel is null) return;

        var result = await _serviceUsuario.AsignarRol(EntitySelectModel.Id, rol.Id);

        await DialogServiceI.ValidarRespuesta(result);

        // Solo actualizamos el estado visual si el servidor confirmó
        if (result.Success)
            rol.IsSelect = !rol.IsSelect;
    }

    // ==============================
    // HELPERS INTERNOS
    // ==============================

    private async Task ShowUserDialogAsync(
        Usuario entity,
        string header,
        Func<Usuario?, Task> onConfirm)
    {
        var dialog = new UsuarioDialog
        {
            Entity = entity,
            TextHeader = header,
            DialogOpenIdentifier = DialogDefaults.Main,
            AceptarCommand = new AsyncRelayCommand<Usuario?>(onConfirm),
            CancelarCommand = new AsyncRelayCommand(
                () => DialogServiceI.CerrarSiEstaAbiertoYEsperar(
                    DialogDefaults.Main))
        };

        await DialogServiceI.MostrarDialogo(dialog);
    }

    private async Task ConfirmarEditarUsuarioAsync(Usuario? user)
    {
        if (user is null) return;

        await DialogServiceI.MostrarDialogoProgreso(async () =>
        {
            var result = await ServicioBase.UpdateAsync(user.Id, user);
            result.ObjInteration = typeof(Usuario);
            await DialogServiceI.ValidarRespuesta(result);
            return result;
        }, DialogDefaults.Progress);
    }

    protected override void UpdateChanged() { }
}
