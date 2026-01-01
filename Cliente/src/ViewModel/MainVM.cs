using System.Windows;
using Cliente.Default;
using Cliente.Obj;
using Cliente.Obj.Model;
using Cliente.Services;
using Cliente.Services.Model;
using Cliente.View.Dialog;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Utilidades.Dialogs;
using Utilidades.Interfaces;
using Utilidades.Mvvm;

namespace Cliente.ViewModel;

public class MainVM : ViewModelBase, IBarNavegacion
{
    private ViewModelBase _pageSelectViewModel = null!;

    private IItemNav _selectedItemNav = null!;

    public MainVM()
    {
        SelectedItemNav = ListItemsNav.First();
        AccountView = new AsyncRelayCommand(VerPerfilUsuarioAsync);
    }

    private DialogService DialogService => DialogService.Instance;
    public ServiceUsuario ServicioServiceUsuario => (ServiceUsuario)ServiceFactory.GetService<Usuario>();

    public ViewModelBase PageSelectViewModel
    {
        get => _pageSelectViewModel;
        set => SetProperty(ref _pageSelectViewModel, value);
    }

    public IAsyncRelayCommand AccountView { get; }

    public IItemNav SelectedItemNav
    {
        get => _selectedItemNav;
        set
        {
            if (SetProperty(ref _selectedItemNav, value)) PageSelectViewModel = value.Page;
        }
    }

    public List<IItemNav> ListItemsNav { get; } =
    [
        new ItemNavigationM
        {
            Title = "Configuracion Sistema",
            SelectedIcon = PackIconKind.TablePlus,
            UnselectedIcon = PackIconKind.TablePlus,
            Page = new PageProcesosVM()
        },
        new ItemNavigationM
        {
            Title = "Gestion de Personal",
            SelectedIcon = PackIconKind.AccountSupervisor,
            UnselectedIcon = PackIconKind.AccountSupervisor,
            Page = new PageGestionEmployerVM()
        }
    ];

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
            DialogOpenIdentifier = DialogDefaults.Main,
            DialogNameIdentifier = DialogDefaults.Sub01
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
            DialogOpenIdentifier = DialogDefaults.Sub01,
            DialogNameIdentifier = DialogDefaults.Sub02
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
            AceptarCommand =
                new AsyncRelayCommand<ChangePassDialog>(changePass =>
                    EjecutarCambioPasswordAsync(changePass, usuario)),
            OldPasswordRequired = Visibility.Visible,
            DialogOpenIdentifier = DialogDefaults.Sub01,
            DialogNameIdentifier = DialogDefaults.Sub02
        };

        await DialogService.MostrarDialogo(dialog);
    }

    private async Task EjecutarCambioPasswordAsync(ChangePassDialog? changePass, Usuario? usuario)
    {
        if (changePass == null || usuario == null)
            return;

        await DialogService.MostrarDialogoProgreso(async () =>
        {
            var result =
                await ServicioServiceUsuario.ChangePasswordAsync(usuario.Id, changePass.OldPassword,
                    changePass.NewPassword);
            await DialogService.ValidarRespuesta(result);
            return result;
        }, DialogDefaults.Sub02);
    }

    protected override void UpdateChanged()
    {
    }
}