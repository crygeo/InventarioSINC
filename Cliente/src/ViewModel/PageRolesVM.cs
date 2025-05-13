using Cliente.src.Model;
using Cliente.src.Services;
using Cliente.src.View.Dialog;
using Cliente.src.View.Items;
using MaterialDesignThemes.Wpf;
using Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Utilidades.Extencions;
using Utilidades.Factory;
using Utilidades.Mvvm;

namespace Cliente.src.ViewModel
{
    public class PageRolesVM : ViewModelServiceBase<Rol>
    {
        public override ServiceBase<Rol> ServicioBase => RolService.Instance;
        public RolService ServiceRol => (RolService)ServicioBase;
        public override ObservableCollection<Rol> Entitys => ServicioBase.Collection;

        public PageRolesVM()
        {
        }


        public async override void CrearEnttiy()
        {
            var usuarioDialog = new RolDialog
            {
                AceptedCommand = new RelayCommand(async (a) =>
                {
                    if (a is Rol user)
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
                Item = new Rol() { },
                ListPerms = (await ServiceRol.GetAllPermisos()).Item1.ConstruirArbol(),
                TextHeader = "Nuevo Rol"
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
                TextHeader = "Eliminar Rol",
                Message = "¿Estás seguro de que quieres eliminar los roles seleccionados?",
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
                            ValidarRespuesta(result);
                            await Task.Delay(100);
                        }
                        DialogHost.Close("MainView"); // Cierra el diálogo progreso
                    });

                })
            };

            await DialogHost.Show(confirmDialog, "MainView");
        }

        public async override void EditarEntity()
        {
            var entitySeleccionado = Entitys.FirstOrDefault(user => user.IsSelect);

            if (entitySeleccionado == null)
                return; // No hay usuario seleccionado, salir del método

            var entityDialog = new RolDialog
            {
                AceptedCommand = new RelayCommand(async (a) =>
                {
                    if (a is Rol entity)
                    {
                        DialogHost.Close("MainView"); // Cierra el diálogo de edición
                                                      //await Task.Delay(300);

                        var progressDialog = new ProgressDialog();
                        await DialogHost.Show(progressDialog, "MainView", openedEventHandler: async (sender, args) =>
                        {
                            var result = await ServicioBase.UpdateAsync(entity.Id, entity);
                            ValidarRespuesta(result);
                            DialogHost.Close("MainView"); // Cierra el diálogo progreso
                        });

                    }
                }),
                Item = entitySeleccionado.Clone(),
                ListPerms = (await ServiceRol.GetAllPermisos()).Item1.ConstruirArbol().SeleccionarNodos(entitySeleccionado.Permisos),
                TextHeader = "Editar Rol"
            };

            await DialogHost.Show(entityDialog, "MainView");
        }

        
    }
}

