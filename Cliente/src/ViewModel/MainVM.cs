using Cliente.src.Command;
using Cliente.src.Model;
using Cliente.src.Services;
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

        public IAsyncRelayCommand AccountView { get; }

        public List<IItemNav> ListItemsNav { get; } = [
<<<<<<< HEAD
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

=======
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
>>>>>>> 29/05/2025

        public MainVM()
        {
            SelectedItemNav = ListItemsNav.First();
            AccountView = new AsyncRelayCommand(VerPerfilUsuarioAsync);
        }

        private async Task VerPerfilUsuarioAsync()
        {
            var result = await ServicioUsuario.GetThisUser();
<<<<<<< HEAD

=======
>>>>>>> 29/05/2025
            await DialogService.ValidarRespuesta(result);

            if (result.Entity == null)
                return;

<<<<<<< HEAD
            var dialog = new AccountDialog()
            {
                Usuario = result.Entity,
                CloseSeccionCommand = new RelayCommand(ConfirmarCierreSeccion),
                ChangedPasswordCommand = new RelayCommand(CambiarPassword),
=======
            var dialog = new AccountDialog
            {
                Usuario = result.Entity,
                CloseSeccionCommand = new AsyncRelayCommand(CerrarSesionAsync),
                ChangedPasswordCommand = new AsyncRelayCommand(param => CambiarPasswordAsync(param))
>>>>>>> 29/05/2025
            };

            await DialogService.MostrarDialogo(dialog);
        }

        private async Task CerrarSesionAsync()
        {
            var confirmDialog = new ConfirmDialog
            {
                TextHeader = "Cerrar Sesión",
                Message = "¿Estás seguro de que quieres cerrar sesión?",
                AceptedCommand = new AsyncRelayCommand(EjecutarCierreSesionAsync),
                CancelCommand = new AsyncRelayCommand(VerPerfilUsuarioAsync)
            };

            await DialogService.MostrarDialogo(confirmDialog);
        }

        private async Task EjecutarCierreSesionAsync()
        {
            AuthService.DeleteToken();
            await AuthService.BorrarCredenciales();
            App.ReiniciarAplicacion();
        }

<<<<<<< HEAD
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
=======
        private async Task CambiarPasswordAsync(object? param)
        {
            if (param is not Usuario user)
                return;
>>>>>>> 29/05/2025

            var dialog = new ChangePassDialog
            {
                AceptedCommand = new AsyncRelayCommand(arg => EjecutarCambioPasswordAsync(arg, user)),
                OldPasswordRequired = Visibility.Visible
            };

            await DialogService.MostrarDialogo(dialog);
        }
<<<<<<< HEAD
=======

        private async Task EjecutarCambioPasswordAsync(object? arg, Usuario user)
        {
            if (arg is not ChangePassDialog changePass)
                return;

            await DialogService.MostrarDialogoProgreso(async () =>
            {
                var result = await UsuarioService.Instance.ChangePasswordAsync(user.Id, changePass.OldPassword, changePass.NewPassword);
                await DialogService.ValidarRespuesta(result);
                return result;
            });
        }
>>>>>>> 29/05/2025

        protected override void UpdateChanged()
        {
            throw new NotImplementedException();
        }
    }


}
