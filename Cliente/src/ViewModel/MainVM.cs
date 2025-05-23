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
using Utilidades.Interfaces;
using Utilidades.Mvvm;

namespace Cliente.src.ViewModel
{
    public class MainVM : ViewModelBase, IBarNavegacion
    {
        private DialogService DialogService => DialogService.Instance;
        public UsuarioService ServicioUsuario => UsuarioService.Instance;

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

        public ICommand AccountView { get; set; }

        public List<IItemNav> ListItemsNav { get; } = [
            new ItemNavigationM() {
                Title = "Usuarios",
                SelectedIcon = PackIconKind.AccountCog,
                UnselectedIcon = PackIconKind.AccountCogOutline,
                Notification = 1,
                Page = new PageUsuarioVM()
            },
            new ItemNavigationM() {
                Title = "Roles",
                SelectedIcon = PackIconKind.ShieldAccount,
                UnselectedIcon = PackIconKind.ShieldAccountOutline,
                Page = new PageRolesVM()
            },
            new ItemNavigationM() {
                Title = "Propiedades",
                SelectedIcon = PackIconKind.TablePlus,
                UnselectedIcon = PackIconKind.TablePlus,
                Page = new PageProcesosVM()
            }
            ];


        public MainVM()
        {
            SelectedItemNav = ListItemsNav.First(); // 🔹 Se inicializa directamente
            AccountView = new RelayCommand(VerPerfilUsuario);
        }


        private async void VerPerfilUsuario(object parm)
        {
            var result = await ServicioUsuario.GetThisUser();

            await DialogService.ValidarRespuesta(result);

            if (result.Entity == null)
                return;

            var dialog = new AccountDialog()
            {
                Usuario = result.Entity,
                CloseSeccionCommand = new RelayCommand(ConfirmarCierreSeccion),
                ChangedPasswordCommand = new RelayCommand(CambiarPassword),
            };

            await DialogService.MostrarDialogo(dialog);
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

            await DialogService.MostrarDialogo(confirmDialog);
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
                            await DialogService.MostrarDialogoProgreso(async () =>
                            {
                                var result = await UsuarioService.Instance.ChangePasswordAsync(user.Id, changePass.OldPassword, changePass.NewPassword);
                                await DialogService.ValidarRespuesta(result);
                                return result;

                            });
                        }
                    }),
                    OldPasswordRequired = Visibility.Visible,
                };

                await DialogService.MostrarDialogo(changedPassword);
            }

        }

        protected override void UpdateChanged()
        {
            throw new NotImplementedException();
        }
    }

}
