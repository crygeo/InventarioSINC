using System.Windows;
using Cliente.Obj.Model;
using Cliente.Services;
using Cliente.Services.Model;
using Cliente.src.Services.Model;
using Cliente.View.Dialog;
using CommunityToolkit.Mvvm.Input;
using Shared.Extensions;

namespace Cliente.src.ViewModel.Model;

public class PageUsuarioVM : ViewModelServiceBase<Usuario>
{
    public IAsyncRelayCommand CambiarPasswordCommand { get; }
    public IAsyncRelayCommand<object?> AsignarRolCommand { get; }

    public ServiceUsuario ServicioServiceUsuario => (ServiceUsuario)ServiceFactory.GetService<Usuario>();

    public PageUsuarioVM()
    {
        CambiarPasswordCommand = new AsyncRelayCommand(CambiarPasswordAsync);
        AsignarRolCommand = new AsyncRelayCommand<object?>(AsignarRolAsync);
    }

    private async Task CambiarPasswordAsync()
    {
        if (EntitySelect == null)
            return;

        var dialog = new ChangePassDialog
        {
            AceptedCommand = new AsyncRelayCommand<object?>(ConfirmarCambioPasswordAsync),
            OldPasswordRequired = Visibility.Collapsed,
            DialogOpenIdentifier = DialogService.DialogIdentifierMain,
        };

        await DialogServiceI.MostrarDialogo(dialog);
    }
    private async Task ConfirmarCambioPasswordAsync(object? a)
    {
        if (a is not ChangePassDialog changePass)
            return;

        if(EntitySelect == null) return;

        await DialogServiceI.MostrarDialogoProgreso(async () =>
        {
            var result = await ServicioServiceUsuario.ChangePasswordAsync(EntitySelect.Id, changePass.OldPassword, changePass.NewPassword);
            await DialogServiceI.ValidarRespuesta(result);
            return result;
        });
            
    }
    private async Task AsignarRolAsync(object? arg)
    {
        if (EntitySelect == null || arg is not Rol rol)
            return;

        var result = await ServicioServiceUsuario.AsignarRol(EntitySelect.Id, rol.Id);
        await DialogServiceI.ValidarRespuesta(result);

        if (result.EntityGet)
            rol.IsSelect = !rol.IsSelect;
    }

    public override async Task CrearEntityAsync()
    {
        var usuarioDialog = new UsuarioDialog
        {
            AceptedCommand = new AsyncRelayCommand<object?>(ConfirmarCrearUsuarioAsync),
            Usuario = new Usuario { FechaNacimiento = DateTime.Today },
            TextHeader = "Nuevo Usuario",
            DialogOpenIdentifier = DialogService.DialogIdentifierMain
        };

        await DialogServiceI.MostrarDialogo(usuarioDialog);
    }
    public override async Task EditarEntityAsync()
    {
        if (EntitySelect == null)
            return;

        var usuarioDialog = new UsuarioDialog
        {
            AceptedCommand = new AsyncRelayCommand<object?>(ConfirmarEditarUsuarioAsync),
            Usuario = EntitySelect.Clone(),
            TextHeader = "Editar Usuario",
            DialogOpenIdentifier = DialogService.DialogIdentifierMain
        };

        await DialogServiceI.MostrarDialogo(usuarioDialog);
    }
    public override async Task DeleteEntityAsync()
    {
        if (EntitySelect == null)
            return;

        var confirmDialog = new ConfirmDialog
        {
            TextHeader = "Eliminar Usuarios",
            Message = "¿Estás seguro de que quieres eliminar los usuarios seleccionados?",
            AceptarCommand = new AsyncRelayCommand(ConfirmarEliminarUsuarioAsync),
            DialogOpenIdentifier = DialogService.DialogIdentifierMain
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
        });
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
        });
    }
    private async Task ConfirmarEliminarUsuarioAsync()
    {

        if (EntitySelect == null) return;

        await DialogServiceI.MostrarDialogoProgreso(async () =>
        {
            var result = await ServicioBase.DeleteAsync(EntitySelect.Id);
            await DialogServiceI.ValidarRespuesta(result);
            return result;
        });
    }

    protected override void UpdateChanged()
    {
    }
}