using System.Windows;
using Cliente.Obj.Model;
using Cliente.Services;
using Cliente.Services.Model;
using Cliente.src.Services.Model;
using Cliente.View.Dialog;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Utilidades.Interfaces;
using Utilidades.Mvvm;

namespace Cliente.ViewModel;

public class MainVM : ViewModelBase, IBarNavegacion
{
    private DialogService DialogService => DialogService.Instance;
    public ServiceUsuario ServicioServiceUsuario => (ServiceUsuario)ServiceFactory.GetService<Usuario>();

    private ViewModelBase _pageSelectViewModel = null!;
    public ViewModelBase PageSelectViewModel
    {
        get => _pageSelectViewModel;
        set => SetProperty(ref _pageSelectViewModel, value);
    }

    private IItemNav _selectedItemNav = null!;
    public IItemNav SelectedItemNav
    {
        get => _selectedItemNav;
        set
        {
            if (SetProperty(ref _selectedItemNav, value))
            {
                PageSelectViewModel = value.Page;
            }
        }
    }

    public IAsyncRelayCommand AccountView { get; }

    public List<IItemNav> ListItemsNav { get; } = [
        new ItemNavigationM {
            Title = "Configuracion Sistema",
            SelectedIcon = PackIconKind.TablePlus,
            UnselectedIcon = PackIconKind.TablePlus,
            Page = new PageProcesosVM()
        }
    ];

    public MainVM()
    {
        SelectedItemNav = ListItemsNav.First();
        AccountView = new AsyncRelayCommand(VerPerfilUsuarioAsync);
    }

    private async Task VerPerfilUsuarioAsync()
    {
        var result = await ServicioServiceUsuario.GetThisUser();
        await DialogService.ValidarRespuesta(result);

        if (result.EntityGet == null)
            return;

        var dialog = new AccountDialog
        {
            Usuario = result.EntityGet,
            CloseSeccionCommand = new AsyncRelayCommand(CerrarSesionAsync),
            ChangedPasswordCommand = new AsyncRelayCommand<Usuario>(CambiarPasswordAsync),
            DialogOpenIdentifier = DialogService.DialogIdentifierMain,
            DialogNameIdentifier = DialogService.DialogSub01,
        };

        await DialogService.MostrarDialogo(dialog);
    }

    private async Task CerrarSesionAsync()
    {
        var confirmDialog = new ConfirmDialog
        {
            TextHeader = "Cerrar Sesión",
            Message = "¿Estás seguro de que quieres cerrar sesión?",
            AceptarCommand = new AsyncRelayCommand(EjecutarCierreSesionAsync),
            DialogOpenIdentifier = DialogService.DialogSub01,
            DialogNameIdentifier = DialogService.DialogSub02,
        };

        await DialogService.MostrarDialogo(confirmDialog);
    }

    private async Task EjecutarCierreSesionAsync()
    {
        AuthService.DeleteToken();
        await AuthService.BorrarCredenciales();
        App.ReiniciarAplicacion();
    }

    private async Task CambiarPasswordAsync(Usuario? usuario)
    {
        if (usuario == null)
            return;

        var dialog = new ChangePassDialog
        {
            AceptedCommand = new AsyncRelayCommand<ChangePassDialog>((changePass) => EjecutarCambioPasswordAsync(changePass, usuario)),
            OldPasswordRequired = Visibility.Visible,
            DialogOpenIdentifier = DialogService.DialogSub01,
            DialogNameIdentifier = DialogService.DialogSub02,
        };

        await DialogService.MostrarDialogo(dialog);
    }

    private async Task EjecutarCambioPasswordAsync(ChangePassDialog? changePass, Usuario? usuario)
    {
        if (changePass == null || usuario == null)
            return;

        await DialogService.MostrarDialogoProgreso(async () =>
        {
            var result = await ServicioServiceUsuario.ChangePasswordAsync(usuario.Id, changePass.OldPassword, changePass.NewPassword);
            await DialogService.ValidarRespuesta(result);
            return result;
        }, DialogService.DialogSub02);
    }

    protected override void UpdateChanged()
    {
    }
}