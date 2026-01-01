using System.Windows;
using Cliente.Default;
using Cliente.Obj.Model;
using Cliente.Services.Model;
using Cliente.View.Dialog;
using CommunityToolkit.Mvvm.Input;
using Shared.Extensions;
using Utilidades.Dialogs;

namespace Cliente.ViewModel.Model;

public partial class PageUsuarioVM : ViewModelServiceBase<Usuario>
{
    public PageUsuarioVM()
    {
        CambiarPasswordCommand = new AsyncRelayCommand(CambiarPasswordAsync);
        AsignarRolCommand = new AsyncRelayCommand<Rol>(AsignarRolAsync);
    }

    public IAsyncRelayCommand CambiarPasswordCommand { get; }
    public IAsyncRelayCommand<Rol> AsignarRolCommand { get; }

    public ServiceUsuario ServicioServiceUsuario => (ServiceUsuario)ServiceFactory.GetService<Usuario>();

    private async Task CambiarPasswordAsync()
    {
        if (EntitySelect == null)
            return;

        var dialog = new ChangePassDialog
        {
            AceptarCommand = new AsyncRelayCommand<object?>(ConfirmarCambioPasswordAsync),
            OldPasswordRequired = Visibility.Collapsed,
            DialogOpenIdentifier = DialogDefaults.Main
        };

        await DialogServiceI.MostrarDialogo(dialog);
    }

    private async Task ConfirmarCambioPasswordAsync(object? a)
    {
        if (a is not ChangePassDialog changePass)
            return;

        if (EntitySelect == null) return;

        await DialogServiceI.MostrarDialogoProgreso(async () =>
        {
            var result = await ServicioServiceUsuario.ChangePasswordAsync(EntitySelect.Id, changePass.OldPassword,
                changePass.NewPassword);
            await DialogServiceI.ValidarRespuesta(result);
            return result;
        }, DialogDefaults.Progress);
    }

    private async Task AsignarRolAsync(Rol? rol)
    {
        if (EntitySelect == null || rol == null)
            return;

        var result = await ServicioServiceUsuario.AsignarRol(EntitySelect.Id, rol.Id);
        await DialogServiceI.ValidarRespuesta(result);

        if (result.EntityGet)
            rol.IsSelect = !rol.IsSelect;
    }

    public override async Task CreateAsync()
    {
        var usuarioDialog = new UsuarioDialog
        {
            AceptarCommand = new AsyncRelayCommand<object?>(ConfirmarCrearUsuarioAsync),
            Entity = new Usuario { FechaNacimiento = DateTime.Today },
            TextHeader = "Nuevo Entity",
            DialogOpenIdentifier = DialogDefaults.Main
        };

        await DialogServiceI.MostrarDialogo(usuarioDialog);
    }

    public override async Task UpdateAsync()
    {
        if (EntitySelect == null)
            return;

        var usuarioDialog = new UsuarioDialog
        {
            AceptarCommand = new AsyncRelayCommand<object?>(ConfirmarEditarUsuarioAsync),
            Entity = EntitySelect.Clone(),
            TextHeader = "Editar Entity",
            DialogOpenIdentifier = DialogDefaults.Main
        };

        await DialogServiceI.MostrarDialogo(usuarioDialog);
    }

    public override async Task DeleteAsync()
    {
        if (EntitySelect == null)
            return;

        var confirmDialog = new ConfirmDialog
        {
            TextHeader = "Eliminar Usuarios",
            Message = "¿Estás seguro de que quieres eliminar los usuarios seleccionados?",
            AceptarCommand = new AsyncRelayCommand(ConfirmarEliminarUsuarioAsync),
            DialogOpenIdentifier = DialogDefaults.Main
        };

        await DialogServiceI.MostrarDialogo(confirmDialog);
    }


    private async Task ConfirmarCrearUsuarioAsync(object? a)
    {
        if (a is not Usuario user)
            return;

        await DialogServiceI.MostrarDialogoProgreso(async () =>
        {
            var result = await ServicioBase.CreateAsync(user);
            await DialogServiceI.ValidarRespuesta(result);
            return result;
        }, DialogDefaults.Progress);
    }

    private async Task ConfirmarEditarUsuarioAsync(object? a)
    {
        if (a is not Usuario user)
            return;

        await DialogServiceI.MostrarDialogoProgreso(async () =>
        {
            var result = await ServicioBase.UpdateAsync(user.Id, user);
            await DialogServiceI.ValidarRespuesta(result);
            return result;
        }, DialogDefaults.Progress);
    }

    private async Task ConfirmarEliminarUsuarioAsync()
    {
        if (EntitySelect == null) return;

        await DialogServiceI.MostrarDialogoProgreso(async () =>
        {
            var result = await ServicioBase.DeleteAsync(EntitySelect.Id);
            await DialogServiceI.ValidarRespuesta(result);
            return result;
        }, DialogDefaults.Progress);
    }

    protected override void UpdateChanged()
    {
    }
}