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
        public override ObservableCollection<Usuario> Entitys => ServicioBase.Collection;


        // Constructor, inicializa el servicio y carga los usuarios
        public PageUsuarioVM()
        {
            CambiarPasswordCommand = new RelayCommand(CambiarPassword);
            AsignarRolCommand = new RelayCommand(AsignarRol);
        }


        private async void CambiarPassword(object arg)
        {
            if (EntiteSelect == null)
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
                            var result = await UsuarioService.Instance.ChangePasswordAsync(EntiteSelect.Id, changePass.OldPassword, changePass.NewPassword);
                            ValidarRespuesta(result);
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
            if (EntiteSelect == null)
                return; // No hay usuario seleccionado, salir del método

            if(arg is not Rol rol)
                return; // No hay rol seleccionado, salir del método

            var result = await UsuarioService.Instance.AsignarRol(EntiteSelect.Id, rol.Id);
            ValidarRespuesta(result);
            if(result.Item1)
                rol.IsSelect = !rol.IsSelect; // Cambia el estado de selección del rol

        }

        public async override void CrearEnttiy()
        {
            var usuarioDialog = new UsuarioDialog
            {
                AceptedCommand = new RelayCommand(async (a) =>
                {
                    if (a is Usuario user)
                    {
                        DialogHost.Close("MainView"); // Cierra el diálogo de creación después de confirmar
                        //await Task.Delay(300);

                        var progressDialog = new ProgressDialog();
                        await DialogHost.Show(progressDialog, "MainView", openedEventHandler: async (sender, args) =>
                        {
                            var result = await ServicioBase.CreateAsync(user);
                            ValidarRespuesta(result);
                            DialogHost.Close("MainView"); // Cierra el diálogo de progreso
                        });

                    }
                }),
                Usuario = new Usuario() { FechaNacimiento = DateTime.Today },
                TextHeader = "Nuevo Usuario"
            };

            await DialogHost.Show(usuarioDialog, "MainView");
        }

        public async override void EditarEntity()
        {
            var usuarioSeleccionado = Entitys.FirstOrDefault(user => user.IsSelect);

            if (usuarioSeleccionado == null)
                return; // No hay usuario seleccionado, salir del método

            var usuarioDialog = new UsuarioDialog
            {
                AceptedCommand = new RelayCommand(async (a) =>
                {
                    if (a is Usuario user)
                    {
                        DialogHost.Close("MainView"); // Cierra el diálogo de edición
                        //await Task.Delay(300);

                        var progressDialog = new ProgressDialog();
                        await DialogHost.Show(progressDialog, "MainView", openedEventHandler: async (sender, args) =>
                        {
                            var result = await ServicioBase.UpdateAsync(user.Id, user);
                            ValidarRespuesta(result);
                            DialogHost.Close("MainView"); // Cierra el diálogo progreso
                        });

                    }
                }),
                Usuario = usuarioSeleccionado.Clone(),
                TextHeader = "Editar Usuario"
            };

            await DialogHost.Show(usuarioDialog, "MainView");
        }

        public async override void DeleteEntity()
        {
            var usuariosSeleccionados = Entitys.Where(user => user.IsSelect).ToList();

            if (usuariosSeleccionados.Count == 0)
                return; // No hay usuarios seleccionados, salir del método

            var confirmDialog = new ConfirmDialog
            {
                TextHeader = "Eliminar Usuarios",
                Message = "¿Estás seguro de que quieres eliminar los usuarios seleccionados?",
                AceptedCommand = new RelayCommand(async (_) =>
                {
                    DialogHost.Close("MainView"); // Cierra el diálogo de confirmación
                    //await Task.Delay(300);

                    var progressDialog = new ProgressDialog();

                    await DialogHost.Show(progressDialog, "MainView", openedEventHandler: async (sender, args) =>
                    {
                        foreach (var usuario in usuariosSeleccionados)
                        {
                            var result = await ServicioBase.DeleteAsync(usuario.Id);
                            DialogHost.Close("MainView"); // Cierra el diálogo progreso

                            ValidarRespuesta(result);
                            await Task.Delay(100);
                        }
                    });

                })
            };

            await DialogHost.Show(confirmDialog, "MainView");
        }
    }
}
