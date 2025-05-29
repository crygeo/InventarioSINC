using Cliente.src.Model;
using Cliente.src.Services;
using Cliente.src.View.Dialog;
using Cliente.src.View.Items;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Utilidades.Mvvm;

namespace Cliente.src.ViewModel
{
    public class PageUsuarioVM : ViewModelServiceBase<Usuario>
    {
        public IAsyncRelayCommand CambiarPasswordCommand { get; }
        public IAsyncRelayCommand<object?> AsignarRolCommand { get; }

        public override ServiceBase<Usuario> ServicioBase => UsuarioService.Instance;
<<<<<<< HEAD

=======
>>>>>>> 29/05/2025

        public PageUsuarioVM()
        {
            CambiarPasswordCommand = new AsyncRelayCommand(CambiarPasswordAsync);
            AsignarRolCommand = new AsyncRelayCommand<object?>(AsignarRolAsync);
        }

        private async Task CambiarPasswordAsync()
        {
            if (EntitySelect == null)
<<<<<<< HEAD
                return; // No hay usuario seleccionado, salir del método
=======
                return;
>>>>>>> 29/05/2025

            var dialog = new ChangePassDialog
            {
<<<<<<< HEAD
                AceptedCommand = new RelayCommand(async (a) =>
                {
                    if (a is ChangePassDialog changePass)
                    {
                        DialogHost.Close("MainView"); // Cierra el diálogo de cambio de contraseña
                                                      //await Task.Delay(300);
                        var progressDialog = new ProgressDialog();
                        await DialogHost.Show(progressDialog, "MainView", openedEventHandler: async (sender, args) =>
                        {
                            var result = await UsuarioService.Instance.ChangePasswordAsync(EntitySelect.Id, changePass.OldPassword, changePass.NewPassword);
                            await DialogService.ValidarRespuesta(result);
                            DialogHost.Close("MainView"); // Cierra el diálogo de progreso
                        });
                    }
                }),
                OldPasswordRequired = Visibility.Collapsed,
=======
                AceptedCommand = new AsyncRelayCommand<object?>(ConfirmarCambioPasswordAsync),
                OldPasswordRequired = Visibility.Collapsed
>>>>>>> 29/05/2025
            };

            await DialogHost.Show(dialog, "MainView");
        }
        private async Task ConfirmarCambioPasswordAsync(object? a)
        {
<<<<<<< HEAD
            if (EntitySelect == null)
                return; // No hay usuario seleccionado, salir del método

            if (arg is not Rol rol)
                return; // No hay rol seleccionado, salir del método

            var result = await UsuarioService.Instance.AsignarRol(EntitySelect.Id, rol.Id);
            await DialogService.ValidarRespuesta(result);
            if (result.Entity)
                rol.IsSelect = !rol.IsSelect; // Cambia el estado de selección del rol
=======
            if (a is not ChangePassDialog changePass)
                return;

            if(EntitySelect == null) return;

            DialogHost.Close("MainView");

            var progressDialog = new ProgressDialog();
            await DialogHost.Show(progressDialog, "MainView", openedEventHandler: async (_, __) =>
            {
                var result = await UsuarioService.Instance.ChangePasswordAsync(EntitySelect.Id, changePass.OldPassword, changePass.NewPassword);
                await DialogService.ValidarRespuesta(result);
                DialogHost.Close("MainView");
            });
        }
        private async Task AsignarRolAsync(object? arg)
        {
            if (EntitySelect == null || arg is not Rol rol)
                return;

            var result = await UsuarioService.Instance.AsignarRol(EntitySelect.Id, rol.Id);
            await DialogService.ValidarRespuesta(result);
>>>>>>> 29/05/2025

            if (result.Entity)
                rol.IsSelect = !rol.IsSelect;
        }

<<<<<<< HEAD
        public async override Task CrearEntityAsync()
        {
            var usuarioDialog = new UsuarioDialog
            {
                AceptedCommand = new RelayCommand(async (a) =>
                {
                    if (a is Usuario user)
                    {
                        await DialogService.MostrarDialogoProgreso(async () =>
                        {
                            var result = await ServicioBase.CreateAsync(user);
                            await DialogService.ValidarRespuesta(result); // Aquí sigue funcionando tu mensaje
                            return result;
                        });
                    }
                }),
                Usuario = new Usuario() { FechaNacimiento = DateTime.Today },
=======
        public override async Task CrearEntityAsync()
        {
            var usuarioDialog = new UsuarioDialog
            {
                AceptedCommand = new AsyncRelayCommand<object?>(ConfirmarCrearUsuarioAsync),
                Usuario = new Usuario { FechaNacimiento = DateTime.Today },
>>>>>>> 29/05/2025
                TextHeader = "Nuevo Usuario"
            };

            await DialogService.MostrarDialogo(usuarioDialog);
        }
<<<<<<< HEAD
        public async override Task EditarEntityAsync()
        {
            if (EntitySelect == null)
                return; // No hay usuario seleccionado, salir del método

            var usuarioDialog = new UsuarioDialog
            {
                AceptedCommand = new RelayCommand(async (a) =>
                {
                    if (a is Usuario user)
                    {
                        await DialogService.MostrarDialogoProgreso(async () =>
                        {
                            var result = await ServicioBase.UpdateAsync(user.Id, user);
                            await DialogService.ValidarRespuesta(result); // Aquí sigue funcionando tu mensaje
                            return result;
                        });
                    }
                }),
=======
        public override async Task EditarEntityAsync()
        {
            if (EntitySelect == null)
                return;

            var usuarioDialog = new UsuarioDialog
            {
                AceptedCommand = new AsyncRelayCommand<object?>(ConfirmarEditarUsuarioAsync),
>>>>>>> 29/05/2025
                Usuario = EntitySelect.Clone(),
                TextHeader = "Editar Usuario"
            };

            await DialogService.MostrarDialogo(usuarioDialog);
        }
<<<<<<< HEAD
        public async override Task DeleteEntityAsync()
        {
            if (EntitySelect == null)
                return; // No hay usuarios seleccionados, salir del método
=======
        public override async Task DeleteEntityAsync()
        {
            if (EntitySelect == null)
                return;
>>>>>>> 29/05/2025

            var confirmDialog = new ConfirmDialog
            {
                TextHeader = "Eliminar Usuarios",
                Message = "¿Estás seguro de que quieres eliminar los usuarios seleccionados?",
<<<<<<< HEAD
                AceptedCommand = new RelayCommand(async (_) =>
                {

                    await DialogService.MostrarDialogoProgreso(async () =>
                    {
                        var result = await ServicioBase.DeleteAsync(EntitySelect.Id);
                        await DialogService.ValidarRespuesta(result);
                        return result;
                    });

                })
=======
                AceptedCommand = new AsyncRelayCommand(ConfirmarEliminarUsuarioAsync)
>>>>>>> 29/05/2025
            };

            await DialogService.MostrarDialogo(confirmDialog);
        }

<<<<<<< HEAD
=======

        private async Task ConfirmarCrearUsuarioAsync(object? a)
        {
            if (a is not Usuario user)
                return;

            await DialogService.MostrarDialogoProgreso(async () =>
            {
                var result = await ServicioBase.CreateAsync(user);
                await DialogService.ValidarRespuesta(result);
                return result;
            });
        }
        private async Task ConfirmarEditarUsuarioAsync(object? a)
        {
            if (a is not Usuario user)
                return;

            await DialogService.MostrarDialogoProgreso(async () =>
            {
                var result = await ServicioBase.UpdateAsync(user.Id, user);
                await DialogService.ValidarRespuesta(result);
                return result;
            });
        }
        private async Task ConfirmarEliminarUsuarioAsync()
        {

            if (EntitySelect == null) return;

            await DialogService.MostrarDialogoProgreso(async () =>
            {
                var result = await ServicioBase.DeleteAsync(EntitySelect.Id);
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
