using Cliente.src.Model;
using Cliente.src.Services.Model;
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
using Cliente.src.Services;
using Utilidades.Extencions;
using Utilidades.Factory;
using Utilidades.Mvvm;

namespace Cliente.src.ViewModel
{
    public class PageRolesVM : ViewModelServiceBase<Rol>
    {
        public RolObjs ObjsRol => (RolObjs)ServicioBase;

        public async override Task CrearEntityAsync()
        {
            var nuevoRol = new Rol();

            var dialog = new RolDialog
            {
                AceptedCommand = new AsyncRelayCommand<Rol>(async (user) =>
                {
                    await CrearNuevoRolAsync(user);
                }),
                Item = nuevoRol,
                ListPerms = (await ObjsRol.GetAllPermisos()).EntityGet.ConstruirArbol(),
                TextHeader = "Nuevo Rol",
                DialogOpenIdentifier = DialogService.DialogIdentifierMain
            };

            await DialogServiceI.MostrarDialogo(dialog);
        }
        public async override Task DeleteEntityAsync()
        {
            if (EntitySelect == null)
                return; // No hay usuarios seleccionados, salir del método

            var confirmDialog = new ConfirmDialog
            {
                TextHeader = "Eliminar Rol",
                Message = "¿Estás seguro de que quieres eliminar el rol seleccionado?",
                AceptarCommand = new AsyncRelayCommand(async (_) =>
                {
                    await DialogServiceI.MostrarDialogoProgreso(async () =>
                    {
                        var result = await ServicioBase.DeleteAsync(EntitySelect.Id);
                        await DialogServiceI.ValidarRespuesta(result);
                        return result;
                    });

                }),
                DialogOpenIdentifier = DialogService.DialogIdentifierMain
            };

            await DialogServiceI.MostrarDialogo(confirmDialog);
        }
        public async override Task EditarEntityAsync()
        {
            if (EntitySelect == null)
                return; // No hay rol seleccionado, salir del método

            var entityDialog = new RolDialog
            {
                AceptedCommand = new AsyncRelayCommand<Rol>(async (entity) =>
                {
                    await EditarRolAsync(entity);
                }),
                Item = EntitySelect.Clone(),
                ListPerms = (await ObjsRol.GetAllPermisos())
                                .EntityGet
                                .ConstruirArbol()
                                .SeleccionarNodos(EntitySelect.Permisos),
                TextHeader = "Editar Rol",
                DialogOpenIdentifier = DialogService.DialogIdentifierMain

            };

            await DialogServiceI.MostrarDialogo(entityDialog);
        }

        private async Task CrearNuevoRolAsync(Rol? user)
        {
            if (user == null)
                return;

            await DialogServiceI.MostrarDialogoProgreso(async () =>
            {
                var result = await ServicioBase.CreateAsync(user);
                await DialogServiceI.ValidarRespuesta(result);
                return result;
            });
        }
        private async Task EditarRolAsync(Rol? entity)
        {
            if (entity == null)
                return;

            await DialogServiceI.MostrarDialogoProgreso(async () =>
            {
                var result = await ServicioBase.UpdateAsync(entity.Id, entity);
                await DialogServiceI.ValidarRespuesta(result);
                return result;
            });
        }

        protected override void UpdateChanged()
        {
            throw new NotImplementedException();
        }
    }
}

