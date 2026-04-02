// Cliente/src/ViewModel/Model/PageRolesVM.cs
using System.Collections.ObjectModel;
using Cliente.Default;
using Cliente.Obj.Model;
using Cliente.Services.Model;
using Cliente.View.Dialog;
using CommunityToolkit.Mvvm.Input;
using Shared.Extensions;
using Utilidades.Dialogs;
using Utilidades.Extencions;
using Utilidades.Factory;

namespace Cliente.ViewModel.Model;

/// <summary>
/// VM para gestión de roles.
///
/// Responsabilidades:
/// - CRUD estándar (heredado).
/// - Construcción del árbol de permisos para el diálogo.
/// - Sincronización de permisos seleccionados con el modelo.
///
/// El árbol de permisos se construye una vez y se reutiliza —
/// se "resetea" visualmente en cada apertura del diálogo.
/// App.AllPermsRoles es una dependencia que debería inyectarse;
/// por ahora se accede via propiedad estática para mantener
/// compatibilidad con el código existente.
/// </summary>
public partial class PageRolesVM : ViewModelServiceBase<Rol>
{
    private readonly ServiceRol _serviceRol;

    public PageRolesVM()
    {
        _serviceRol = (ServiceRol)ServicioBase;
    }

    // ==============================
    // CRUD SOBREESCRITO
    // ==============================

    public override async Task CreateAsync()
    {
        var entity = new Rol
        {
            Nombre = string.Empty,
            Permisos = new List<string>(),
            IsAdmin = false
        };

        await ShowRolDialogAsync(entity, "Nuevo Rol", ConfirmarCrearEntityAsync);
    }

    public override async Task UpdateAsync()
    {
        if (EntitySelect is null) return;
        await ShowRolDialogAsync(
            EntitySelect.Model.Clone(),
            "Editar Rol",
            ConfirmarEditarRolAsync);
    }

    // ==============================
    // ÁRBOL DE PERMISOS
    // ==============================

    /// <summary>
    /// Construye el árbol de nodos de permisos a partir de la lista global.
    /// Se preseleccionan los permisos que ya tiene el rol.
    /// La construcción se hace en cada apertura — O(n) sobre los permisos,
    /// aceptable dado el tamaño típico de una lista de permisos.
    /// </summary>
    private List<Nodos> BuildPermisosTree(IEnumerable<string> permisosActivos)
    {
        var tree = App.AllPermsRoles.ConstruirArbol();
        tree.SeleccionarNodos(permisosActivos);
        return tree;
    }

    // ==============================
    // HELPERS INTERNOS
    // ==============================

    private async Task ShowRolDialogAsync(
        Rol entity,
        string header,
        Func<Rol?, Task> onConfirm)
    {
        var permisos = BuildPermisosTree(entity.Permisos);

        var dialog = new RolDialog
        {
            Entity = entity,
            ListPerms = permisos,
            TextHeader = header,
            DialogOpenIdentifier = DialogDefaults.Main,
            DialogNameIdentifier = DialogDefaults.Sub01,
            AceptarCommand = new AsyncRelayCommand<Rol?>(onConfirm),
            CancelarCommand = new AsyncRelayCommand(
                () => DialogServiceI.CerrarSiEstaAbiertoYEsperar(
                    DialogDefaults.Main))
        };

        await DialogServiceI.MostrarDialogo(dialog);
    }

    private async Task ConfirmarEditarRolAsync(Rol? rol)
    {
        if (rol is null) return;

        await DialogServiceI.MostrarDialogoProgreso(async () =>
        {
            var result = await ServicioBase.UpdateAsync(rol.Id, rol);
            result.ObjInteration = typeof(Rol);
            await DialogServiceI.ValidarRespuesta(result);
            return result;
        }, DialogDefaults.Progress);
    }

    protected override void UpdateChanged() { }
}