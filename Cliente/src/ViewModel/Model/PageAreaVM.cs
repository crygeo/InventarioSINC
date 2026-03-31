using Cliente.Obj.Model;
using Cliente.Services.Model;
using Cliente.ViewModel.Model.Sub;
using Utilidades.Interfaces;
using Utilidades.Mvvm;

namespace Cliente.ViewModel.Model;

/// <summary>
/// Orquestador principal de la jerarquía Area → Turno → Seccion → Grupo.
///
/// Responsabilidades:
/// - Lista de áreas (panel izquierdo) vía herencia de ViewModelServiceBase&lt;Area&gt;.
/// - NavigationStack para el panel derecho.
/// - Ciclo de vida de los VMs hijos (singletons) — activa/desactiva según navegación.
/// - Coordinación ante eliminación remota (SignalR) del ítem activo en el stack.
///
/// Los VMs hijos (PageTurnoVM, PageSeccionVM, PageGrupoVM) son singletons
/// instanciados aquí — un único objeto que se reconfigura al cambiar el contexto padre.
/// Esto evita el costo de inicialización repetida y mantiene la caché caliente.
/// </summary>
public partial class PageAreaVM : ViewModelServiceBase<Area>
{
    // ==============================
    // VMs HIJOS (singletons)
    // ==============================

    public PageTurnoVM    PageTurnoVm   { get; }
    public PageSeccionVM  PageSeccionVm { get; }
    public PageGrupoVM    PageGrupoVm   { get; }
    public PageEmpleadoAsignacionVM PageEmpleadoAsignacionVM { get; }


    // ==============================
    // NAVEGACIÓN
    // ==============================

    public NavigationStack DetailNavigation { get; } = new();

    // ==============================
    // CONSTRUCTOR
    // ==============================

    public PageAreaVM()
    {
        PageTurnoVm   = new PageTurnoVM();
        PageSeccionVm = new PageSeccionVM();
        PageGrupoVm   = new PageGrupoVM();
        
        PageEmpleadoAsignacionVM = new PageEmpleadoAsignacionVM(ServiceFactory.GetService<Empleado>());
    }

    // ==============================
    // CICLO DE VIDA
    // ==============================

    protected override async Task OnActivateAsync()
    {
        // 1. Pre-inicializar caché de servicios hijos (una sola vez).
        //    Cuando el usuario seleccione un área, los datos ya están localmente
        //    y el filtrado es instantáneo sin llamadas adicionales al servidor.
        await PageTurnoVm.ServicioBase.InitializeAsync();
        await PageSeccionVm.ServicioBase.InitializeAsync();
        await PageGrupoVm.ServicioBase.InitializeAsync();

        // 2. Inyectar servicios hijos en PageTurnoVM para que pueda
        //    calcular las estadísticas de secciones y grupos en tiempo real.
        PageTurnoVm.InjectChildServices(
            PageSeccionVm.ServicioBase,
            PageGrupoVm.ServicioBase);

        PageSeccionVm.InjectChildServices(
            PageGrupoVm.ServicioBase);

        // 3. Activar base → carga áreas, suscribe CollectionChanged de Area
        await base.OnActivateAsync();

        // 4. Suscribir eventos de hijos para detectar eliminación del padre activo
        PageTurnoVm.ServicioBase.CollectionChanged   += OnTurnoChangedExternal;
        PageSeccionVm.ServicioBase.CollectionChanged += OnSeccionChangedExternal;

        // 5. Panel derecho vacío al iniciar
        DetailNavigation.Reset(new EmptyDetailVM());
    }

    protected override async Task OnDeactivateAsync()
    {
        PageTurnoVm.ServicioBase.CollectionChanged   -= OnTurnoChangedExternal;
        PageSeccionVm.ServicioBase.CollectionChanged -= OnSeccionChangedExternal;

        // Liberar referencias a servicios hijos antes de desactivar
        PageTurnoVm.DetachChildServices();

        // Desactivar hijos: limpia Entities y desuscribe CollectionChanged de la vista,
        // pero NO cierra SignalR (DeactivateAsync ya no llama ShutdownAsync).
        await TryDeactivateAsync(PageGrupoVm);
        await TryDeactivateAsync(PageSeccionVm);
        await TryDeactivateAsync(PageTurnoVm);
        await TryDeactivateAsync(PageEmpleadoAsignacionVM);

        // Cerrar SignalR de los hijos — solo al destruir el orquestador completamente
        await PageGrupoVm.ShutdownSignalRAsync();
        await PageSeccionVm.ShutdownSignalRAsync();
        await PageTurnoVm.ShutdownSignalRAsync();
        await PageEmpleadoAsignacionVM.ShutdownAsync();

        DetailNavigation.Reset(new EmptyDetailVM());

        await base.OnDeactivateAsync();
    }

    // ==============================
    // SELECCIÓN — NIVEL 1: ÁREA
    // ==============================

    private Area? _areaActiva;

    /// <summary>
    /// Llamado desde la vista cuando el usuario selecciona un área en la lista izquierda.
    /// Reconfigura PageTurnoVm con el nuevo contexto y lo coloca como raíz del stack.
    /// </summary>
    public async Task OnAreaSelectedAsync(Area area)
    {
        if (_areaActiva?.Id == area.Id) return;

        // Limpiar niveles inferiores antes de cambiar el contexto raíz
        await TryDeactivateAsync(PageGrupoVm);
        await TryDeactivateAsync(PageSeccionVm);
        await TryDeactivateAsync(PageTurnoVm);

        _areaActiva  = area;
        EntitySelect = area;

        PageTurnoVm.AreaPadre = area;
        
        await PageTurnoVm.ActivateAsync();
        await PageGrupoVm.ActivateAsync();
        await PageSeccionVm.ActivateAsync();

        DetailNavigation.Reset(PageTurnoVm);
    }

    // ==============================
    // SELECCIÓN — NIVEL 2: TURNO
    // ==============================

    /// <summary>
    /// Llamado desde la vista de PageTurnoVM cuando el usuario selecciona un turno
    /// para ver sus secciones. Empuja PageSeccionVM al stack.
    /// </summary>
    public async Task OnTurnoSelectedAsync(Turno turno)
    {
        await TryDeactivateAsync(PageGrupoVm);
        await TryDeactivateAsync(PageSeccionVm);

        PageSeccionVm.TurnoPadre = turno;
        await PageSeccionVm.ActivateAsync();

        DetailNavigation.Push(PageSeccionVm);
    }

    // ==============================
    // SELECCIÓN — NIVEL 3: SECCIÓN
    // ==============================

    /// <summary>
    /// Llamado desde la vista de PageSeccionVM cuando el usuario selecciona
    /// una sección para ver sus grupos. Empuja PageGrupoVM al stack.
    /// </summary>
    public async Task OnSeccionSelectedAsync(Seccion seccion)
    {
        await TryDeactivateAsync(PageGrupoVm);
        await TryDeactivateAsync(PageEmpleadoAsignacionVM);

        if (seccion.EsGrupo)
        {
            // Ruta existente → Grupos
            PageGrupoVm.SeccionPadre = seccion;
            await PageGrupoVm.ActivateAsync();
            DetailNavigation.Push(PageGrupoVm);
        }
        else
        {
            // Ruta nueva → Empleados directos de la Sección
            PageEmpleadoAsignacionVM.SetContexto(
                nombreContexto: $"Empleados — {seccion.Nombre}",
                cupo:           seccion.Cupo,
                getEmpleadoIds: () => seccion.EmpleadoIds
                                      ?? Enumerable.Empty<string>(),
                addToList:      itemId => PageSeccionVm.ServicioBase
                    .ItemAddedToListAsync(
                        seccion.Id, nameof(Seccion.EmpleadoIds), itemId),
                removeFromList: itemId => PageSeccionVm.ServicioBase
                    .ItemRemovedToListAsync(
                        seccion.Id, nameof(Seccion.EmpleadoIds), itemId));

            await PageEmpleadoAsignacionVM.ActivateAsync();
            DetailNavigation.Push(PageEmpleadoAsignacionVM);
        }
    }
    
    public async Task OnGrupoSelectedAsync(Grupo grupo)
    {
        await TryDeactivateAsync(PageEmpleadoAsignacionVM);

        PageEmpleadoAsignacionVM.SetContexto(
            nombreContexto: $"Empleados — {grupo.Nombre}",
            cupo:           grupo.Cupo,
            getEmpleadoIds: () => grupo.EmpleadoIds
                                  ?? Enumerable.Empty<string>(),
            addToList:      itemId => PageGrupoVm.ServicioBase
                .ItemAddedToListAsync(
                    grupo.Id, nameof(Grupo.EmpleadoIds), itemId),
            removeFromList: itemId => PageGrupoVm.ServicioBase
                .ItemRemovedToListAsync(
                    grupo.Id, nameof(Grupo.EmpleadoIds), itemId));

        await PageEmpleadoAsignacionVM.ActivateAsync();
        DetailNavigation.Push(PageEmpleadoAsignacionVM);
    }

    // ==============================
    // ELIMINACIÓN REMOTA — ÁREA ACTIVA
    // ==============================

    protected override void OnServiceCollectionChanged(EntityChangeType type, string id, Area? entity)
    {
        base.OnServiceCollectionChanged(type, id, entity);

        if (type == EntityChangeType.Deleted && _areaActiva?.Id == id)
        {
            _areaActiva = null;
            _ = HandleRootDeletedAsync("El área fue eliminada por otro usuario.");
        }
    }

    // ==============================
    // ELIMINACIÓN REMOTA — HIJOS
    // ==============================

    private void OnTurnoChangedExternal(EntityChangeType type, string id, Turno? entity)
    {
        if (type != EntityChangeType.Deleted) return;

        if (PageSeccionVm.TurnoPadre?.Id == id)
            _ = HandleIntermediateParentDeletedAsync(
                vmAfectado:         PageSeccionVm,
                vmHijoDelAfectado:  PageGrupoVm,
                vmPadreSeguro:      PageTurnoVm,
                mensaje:            "El turno fue eliminado por otro usuario.");
    }

    private void OnSeccionChangedExternal(EntityChangeType type, string id, Seccion? entity)
    {
        if (type != EntityChangeType.Deleted) return;

        if (PageGrupoVm.SeccionPadre?.Id == id)
            _ = HandleIntermediateParentDeletedAsync(
                vmAfectado:         PageGrupoVm,
                vmHijoDelAfectado:  null,
                vmPadreSeguro:      PageSeccionVm,
                mensaje:            "La sección fue eliminada por otro usuario.");
    }

    // ==============================
    // HELPERS DE NAVEGACIÓN
    // ==============================

    /// <summary>
    /// Maneja la eliminación del área raíz: limpia todo el stack.
    /// </summary>
    private async Task HandleRootDeletedAsync(string mensaje)
    {
        await TryDeactivateAsync(PageGrupoVm);
        await TryDeactivateAsync(PageSeccionVm);
        await TryDeactivateAsync(PageTurnoVm);

        DetailNavigation.Reset(new EmptyDetailVM());
        EntitySelect  = null;
        _areaActiva   = null;

        DialogServiceI.MensajeQueue.Enqueue(mensaje);
    }

    /// <summary>
    /// Maneja la eliminación de un padre intermedio (Turno o Sección).
    /// Desactiva el VM afectado y sus hijos, navega al nivel seguro y notifica.
    /// </summary>
    /// <param name="vmAfectado">VM cuyo padre fue eliminado — sale del stack.</param>
    /// <param name="vmHijoDelAfectado">VM hijo del afectado (puede ser null si no hay más niveles).</param>
    /// <param name="vmPadreSeguro">VM al que el stack regresa como nueva raíz.</param>
    /// <param name="mensaje">Texto para el snackbar.</param>
    private async Task HandleIntermediateParentDeletedAsync(
        IDeactivable vmAfectado,
        IDeactivable? vmHijoDelAfectado,
        ViewModelBase vmPadreSeguro,
        string mensaje)
    {
        // Desactivar desde el nivel más profundo hacia arriba
        if (vmHijoDelAfectado is not null)
            await TryDeactivateAsync(vmHijoDelAfectado);

        await TryDeactivateAsync(vmAfectado);

        // Resetear el stack al nivel seguro — NavigationStack.Reset limpia
        // la pila completa y establece el VM indicado como única entrada
        DetailNavigation.Reset(vmPadreSeguro);

        DialogServiceI.MensajeQueue.Enqueue(mensaje);
    }

    /// <summary>
    /// Desactiva de forma segura sin propagar excepciones si el VM no estaba activo.
    /// </summary>
    private static async Task TryDeactivateAsync(IDeactivable vm)
    {
        try { await vm.DeactivateAsync(); }
        catch { /* ya estaba inactivo o sin inicializar */ }
    }

    protected override void UpdateChanged()
    {
        base.UpdateChanged();
    }
}

/// <summary>
/// Placeholder para el panel derecho cuando no hay área seleccionada.
/// La vista usa un DataTemplate vacío para este tipo.
/// </summary>
public class EmptyDetailVM : ViewModelBase
{
    protected override void UpdateChanged() { }
}