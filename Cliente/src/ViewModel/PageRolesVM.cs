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

        public async override Task CrearEntityAsync()
        {
            var dialog = new RolDialog
            {
                AceptedCommand = new RelayCommand(async (a) =>
                {
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
        public async override Task EditarEntityAsync()
        {
            if (EntitySelect == null)
                return; // No hay usuario seleccionado, salir del método

            var entityDialog = new RolDialog
            {
                AceptedCommand = new RelayCommand(async (a) =>
                {
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
                TextHeader = "Editar Rol"
            };

            await DialogService.MostrarDialogo(entityDialog);
        }
        protected override void UpdateChanged()
        {
            throw new NotImplementedException();
        }
    }
}

