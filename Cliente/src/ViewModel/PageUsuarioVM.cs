using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Cliente.src.Model;
using Cliente.src.Services;
using Cliente.src.View.Dialog;
using Cliente.src.View.Items;
using MaterialDesignThemes.Wpf;
using Shared.Extensions;
using Utilidades.Mvvm;

namespace Cliente.src.ViewModel
{
    public class PageUsuarioVM : ViewModelServiceBase<Usuario>
    {
        public ICommand CambiarPasswordCommand { get; }
        public ICommand AsignarRolCommand { get; }

        public override ServiceBase<Usuario> ServicioBase => UsuarioService.Instance;


        // Constructor, inicializa el servicio y carga los usuarios
        public PageUsuarioVM()
        {
            CambiarPasswordCommand = new RelayCommand(CambiarPassword);
            AsignarRolCommand = new RelayCommand(AsignarRol);
        }


        private async void CambiarPassword(object arg)
        {
            if (EntitySelect == null)
                return; // No hay usuario seleccionado, salir del método

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
                            var result = await UsuarioService.Instance.ChangePasswordAsync(EntitySelect.Id, changePass.OldPassword, changePass.NewPassword);
                            await DialogService.ValidarRespuesta(result);
                            DialogHost.Close("MainView"); // Cierra el diálogo de progreso
                        });
                    }
                }),
                OldPasswordRequired = Visibility.Collapsed,
            };

            await DialogHost.Show(changedPassword, "MainView");

        }
        private async void AsignarRol(object arg)
        {
            if (EntitySelect == null)
                return; // No hay usuario seleccionado, salir del método

            if (arg is not Rol rol)
                return; // No hay rol seleccionado, salir del método

            var result = await UsuarioService.Instance.AsignarRol(EntitySelect.Id, rol.Id);
            await DialogService.ValidarRespuesta(result);
            if (result.Entity)
                rol.IsSelect = !rol.IsSelect; // Cambia el estado de selección del rol

        }

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
                TextHeader = "Nuevo Usuario"
            };

            await DialogService.MostrarDialogo(usuarioDialog);
        }
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
                Usuario = EntitySelect.Clone(),
                TextHeader = "Editar Usuario"
            };

            await DialogService.MostrarDialogo(usuarioDialog);
        }
        public async override Task DeleteEntityAsync()
        {
            if (EntitySelect == null)
                return; // No hay usuarios seleccionados, salir del método

            var confirmDialog = new ConfirmDialog
            {
                TextHeader = "Eliminar Usuarios",
                Message = "¿Estás seguro de que quieres eliminar los usuarios seleccionados?",
                AceptedCommand = new RelayCommand(async (_) =>
                {

                    await DialogService.MostrarDialogoProgreso(async () =>
                    {
                        var result = await ServicioBase.DeleteAsync(EntitySelect.Id);
                        await DialogService.ValidarRespuesta(result);
                        return result;
                    });

                })
            };

            await DialogService.MostrarDialogo(confirmDialog);
        }

        protected override void UpdateChanged()
        {
            throw new NotImplementedException();
        }
    }
}
