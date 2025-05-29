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

        public async override Task CrearEntityAsync()
        {
<<<<<<< HEAD
=======
            var nuevoRol = new Rol();

>>>>>>> 29/05/2025
            var dialog = new RolDialog
            {
                AceptedCommand = new AsyncRelayCommand<Rol>(async (user) =>
                {
<<<<<<< HEAD
                    if (a is Rol user)
                    {

                        await DialogService.MostrarDialogoProgreso(async () =>
                        {
                            var result = await ServicioBase.CreateAsync(user);
                            await DialogService.ValidarRespuesta(result);
                            return result;
                        });

                    }
                }),
                Item = new Rol() { },
=======
                    await CrearNuevoRolAsync(user);
                }),
                Item = nuevoRol,
>>>>>>> 29/05/2025
                ListPerms = (await ServiceRol.GetAllPermisos()).Entity.ConstruirArbol(),
                TextHeader = "Nuevo Rol"
            };

            await DialogService.MostrarDialogo(dialog);
        }
        public async override Task DeleteEntityAsync()
        {
            if (EntitySelect == null)
                return; // No hay usuarios seleccionados, salir del método

            var confirmDialog = new ConfirmDialog
            {
                TextHeader = "Eliminar Rol",
                Message = "¿Estás seguro de que quieres eliminar el rol seleccionado?",
<<<<<<< HEAD
                AceptedCommand = new RelayCommand(async (_) =>
=======
                AceptedCommand = new AsyncRelayCommand(async (_) =>
>>>>>>> 29/05/2025
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
        public async override Task EditarEntityAsync()
        {
            if (EntitySelect == null)
<<<<<<< HEAD
                return; // No hay usuario seleccionado, salir del método
=======
                return; // No hay rol seleccionado, salir del método
>>>>>>> 29/05/2025

            var entityDialog = new RolDialog
            {
                AceptedCommand = new AsyncRelayCommand<Rol>(async (entity) =>
                {
<<<<<<< HEAD
                    if (a is Rol entity)
                    {
                        await DialogService.MostrarDialogoProgreso(async () =>
                        {
                            var result = await ServicioBase.UpdateAsync(entity.Id, entity);
                                await DialogService.ValidarRespuesta(result);
                            return result;
                        });

                    }
                }),
                Item = EntitySelect.Clone(),
                ListPerms = (await ServiceRol.GetAllPermisos()).Entity.ConstruirArbol().SeleccionarNodos(EntitySelect.Permisos),
=======
                    await EditarRolAsync(entity);
                }),
                Item = EntitySelect.Clone(),
                ListPerms = (await ServiceRol.GetAllPermisos())
                                .Entity
                                .ConstruirArbol()
                                .SeleccionarNodos(EntitySelect.Permisos),
>>>>>>> 29/05/2025
                TextHeader = "Editar Rol"
            };

            await DialogService.MostrarDialogo(entityDialog);
<<<<<<< HEAD
        }
        protected override void UpdateChanged()
        {
            throw new NotImplementedException();
        }
=======
        }

        private async Task CrearNuevoRolAsync(Rol? user)
        {
            if (user == null)
                return;

            await DialogService.MostrarDialogoProgreso(async () =>
            {
                var result = await ServicioBase.CreateAsync(user);
                await DialogService.ValidarRespuesta(result);
                return result;
            });
        }
        private async Task EditarRolAsync(Rol? entity)
        {
            if (entity == null)
                return;

            await DialogService.MostrarDialogoProgreso(async () =>
            {
                var result = await ServicioBase.UpdateAsync(entity.Id, entity);
                await DialogService.ValidarRespuesta(result);
                return result;
            });
        }

        protected override void UpdateChanged()
        {
            throw new NotImplementedException();
        }
>>>>>>> 29/05/2025
    }
}

