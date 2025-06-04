using Cliente.src.Command;
using Cliente.src.Model;
using Cliente.src.Services;
using Cliente.src.Services.Model;
using Cliente.src.View.Dialog;
using Cliente.src.View.Items;
using Cliente.src.ViewModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Utilidades.Command;
using Utilidades.Interfaces;
using Utilidades.Mvvm;

namespace Cliente.src.ViewModel
{
    public class MainVM : ViewModelBase, IBarNavegacion
    {
        private DialogService DialogService => DialogService.Instance;
        public UsuarioService ServicioUsuario => (UsuarioService)ServiceFactory.GetService<Usuario>();

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
            Title = "Usuarios",
            SelectedIcon = PackIconKind.AccountCog,
            UnselectedIcon = PackIconKind.AccountCogOutline,
            Notification = 1,
            Page = new PageUsuarioVM()
        },
        new ItemNavigationM {
            Title = "Roles",
            SelectedIcon = PackIconKind.ShieldAccount,
            UnselectedIcon = PackIconKind.ShieldAccountOutline,
            Page = new PageRolesVM()
        },
        new ItemNavigationM {
            Title = "Propiedades",
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
            var result = await ServicioUsuario.GetThisUser();
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
                var result = await ServicioUsuario.ChangePasswordAsync(usuario.Id, changePass.OldPassword, changePass.NewPassword);
                await DialogService.ValidarRespuesta(result);
                return result;
            }, DialogService.DialogSub02);
        }

        protected override void UpdateChanged()
        {
            throw new NotImplementedException();
        }
    }


}
