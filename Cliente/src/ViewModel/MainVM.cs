using Cliente.src.Command;
using Cliente.src.Model;
using Cliente.src.Services;
using Cliente.src.View.Dialog;
using Cliente.src.View.Items;
using Cliente.src.ViewModel;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Utilidades.Command;
using Utilidades.Mvvm;

namespace Cliente.src.ViewModel
{
    public class MainVM : ViewModelBase
    {
        public UsuarioService ServicioUsuario => UsuarioService.Instance;

        private ViewModelBase _pageSelectViewModel = null!;
        public ViewModelBase PageSelectViewModel
        {
            get => _pageSelectViewModel;
            set => SetProperty(ref _pageSelectViewModel, value);
        }

        private ItemNavigationM _selectedItemNav = null!;
        public ItemNavigationM SelectedItemNav
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

        public ICommand AccountView { get; set; }

        public List<ItemNavigationM> ListItemsNav { get; } = [
            new()
        {
            Title = "Usuarios",
            SelectedIcon = PackIconKind.AccountCog,
            UnselectedIcon = PackIconKind.AccountCogOutline,
            Notification = 1,
            Page = new PageUsuarioVM()
        },
        new()
        {
            Title = "Roles",
            SelectedIcon = PackIconKind.ShieldAccount,
            UnselectedIcon = PackIconKind.ShieldAccountOutline,
            Page = new PageRolesVM()
        }
        ];

        public MainVM()
        {
            SelectedItemNav = ListItemsNav.First(); // 🔹 Se inicializa directamente
            AccountView = new RelayCommand(VerPerfilUsuario);
        }


        private async void VerPerfilUsuario(object parm)
        {
            var usuario = await ServicioUsuario.GetThisUser();
            if (usuario.Item1 == null)
            {
                MessageDialog messageDialog = new()
                {
                    Message = usuario.Item2,
                };
                await DialogHost.Show(messageDialog, "MainView");
                return;
            }
            var dialog = new AccountDialog()
            {
                Usuario = usuario.Item1,
                CloseSeccionCommand = new RelayCommand(ConfirmarCierreSeccion),
                ChangedPasswordCommand = new RelayCommand(CambiarPassword),
            };

            await DialogHost.Show(dialog, "MainView");
        }

        private async void ConfirmarCierreSeccion(object parm)
        {
            var confirmDialog = new ConfirmDialog
            {
                TextHeader = "Cerrar Sesión",
                Message = "¿Estás seguro de que quieres cerrar sesión?",
                AceptedCommand = new RelayCommand(async (arg) =>
                {
                    AuthService.DeleteToken();
                    await AuthService.BorrarCredenciales();
                    App.ReiniciarAplicacion();
                }),
                CancelCommand = new RelayCommand(VerPerfilUsuario)
            };

            DialogHost.Close("MainView");
            await Task.Delay(300);
            await DialogHost.Show(confirmDialog, "MainView");
        }

        private async void CambiarPassword(object param)
        {
            if (param is Usuario user)
            {

                var changedPassword = new ChangePassDialog
                {
                    AceptedCommand = new RelayCommand(async (a) =>
                    {
                        if (a is ChangePassDialog changePass)
                        {
                            DialogHost.Close("MainView"); // Cierra el diálogo de cambio de contraseña
                                                          //await Task.Delay(300);
                            var progressDialog = new ProgressDialog();
                            await DialogHost.Show(progressDialog, "MainView", openedEventHandler: async (sender, args) =>
                            {
                                var result = await UsuarioService.Instance.ChangePasswordAsync(user.Id, changePass.OldPassword, changePass.NewPassword);
                                ValidarRespuesta(result);
                                DialogHost.Close("MainView"); // Cierra el diálogo de progreso
                            });
                        }
                    }),
                    OldPasswordRequired = Visibility.Visible,
                };

                DialogHost.Close("MainView");
                await Task.Delay(300);
                await DialogHost.Show(changedPassword, "MainView");
            }

        }
        public async void ValidarRespuesta((bool, string) resp)
        {
            if (resp.Item1)
                return;
            else
            {
                var errorDialog = new MessageDialog
                {
                    Message = resp.Item2,
                };

                await Task.Delay(300);
                await DialogHost.Show(errorDialog, "MainView");
            }
        }
    }

}
