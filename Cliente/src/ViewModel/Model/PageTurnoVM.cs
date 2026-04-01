using System.Collections.Specialized;
using Cliente.Default;
using Cliente.Obj.Model;
using Cliente.Services.Model;
using Utilidades.Mvvm;

namespace Cliente.ViewModel.Model;

public partial class PageTurnoVM : ViewModelServiceBase<Turno>
{
    public ServiceTurno ServiceTurno => (ServiceTurno)ServicioBase;

    // ==============================
    // SERVICIOS DE HIJOS (para estadísticas)
    // Inyectados por PageAreaVM tras la inicialización — no se instancian aquí
    // para no duplicar servicios ni conexiones SignalR.
    // ==============================

    private ServiceBase<Seccion>? _serviceSeccion;
    private ServiceBase<Grupo>? _serviceGrupo;

    /// <summary>
    /// Llamado por PageAreaVM después de inicializar los servicios hijos.
    /// Permite a PageTurnoVM calcular estadísticas de secciones y grupos
    /// sin crear nuevas instancias de servicio.
    /// </summary>
    public void InjectChildServices(
        ServiceBase<Seccion> serviceSeccion,
        ServiceBase<Grupo> serviceGrupo)
    {
        _serviceSeccion = serviceSeccion;
        _serviceGrupo   = serviceGrupo;

        // Reaccionar a cambios en secciones/grupos para actualizar los contadores
        _serviceSeccion.CollectionChanged += OnSeccionCollectionChanged;
        _serviceGrupo.CollectionChanged   += OnGrupoCollectionChanged;
    }

    public void DetachChildServices()
    {
        if (_serviceSeccion is not null)
            _serviceSeccion.CollectionChanged -= OnSeccionCollectionChanged;
        if (_serviceGrupo is not null)
            _serviceGrupo.CollectionChanged -= OnGrupoCollectionChanged;
    }

    private void OnSeccionCollectionChanged(EntityChangeType type, string id, Seccion? entity)
    {
        OnPropertyChanged(nameof(TotalSecciones));
        OnPropertyChanged(nameof(TotalGrupos));
        OnPropertyChanged(nameof(TotalEmpleados));
    }

    private void OnGrupoCollectionChanged(EntityChangeType type, string id, Grupo? entity)
    {
        OnPropertyChanged(nameof(TotalGrupos));
        OnPropertyChanged(nameof(TotalEmpleados));
    }

    // ==============================
    // ESTADÍSTICAS CALCULADAS
    // Se calculan en tiempo real desde la caché — sin llamadas al servidor.
    // ==============================

    /// <summary>Total de secciones que pertenecen a los turnos del área activa.</summary>
    public int TotalSecciones
    {
        get
        {
            if (_serviceSeccion is null || AreaPadre is null) return 0;

            var turnoIds = ServicioBase.CacheById.Values
                .Where(t => t.AreaId == AreaPadre.Id)
                .Select(t => t.Id)
                .ToHashSet();

            return _serviceSeccion.CacheById.Values
                .Count(s => turnoIds.Contains(s.TurnoId));
        }
    }

    /// <summary>Total de grupos que pertenecen a las secciones de los turnos del área activa.</summary>
    public int TotalGrupos
    {
        get
        {
            if (_serviceSeccion is null || _serviceGrupo is null || AreaPadre is null) return 0;

            var turnoIds = ServicioBase.CacheById.Values
                .Where(t => t.AreaId == AreaPadre.Id)
                .Select(t => t.Id)
                .ToHashSet();

            var seccionIds = _serviceSeccion.CacheById.Values
                .Where(s => turnoIds.Contains(s.TurnoId))
                .Select(s => s.Id)
                .ToHashSet();

            return _serviceGrupo.CacheById.Values
                .Count(g => seccionIds.Contains(g.SeccionId));
        }
    }

    public int TotalEmpleados
    {
        get
        {
            if (_serviceSeccion is null || _serviceGrupo is null || AreaPadre is null) return 0;

            var turnoIds = ServicioBase.CacheById.Values
                .Where(t => t.AreaId == AreaPadre.Id)
                .Select(t => t.Id)
                .ToHashSet();

            var seccionIds = _serviceSeccion.CacheById.Values
                .Where(s => turnoIds.Contains(s.TurnoId))
                .Select(s => s.Id)
                .ToHashSet();

            // Empleados en secciones
            var empleadosSecciones = _serviceSeccion.CacheById.Values
                .Where(s => seccionIds.Contains(s.Id))
                .SelectMany(s => s.EmpleadoIds ?? Enumerable.Empty<string>());

            // Empleados en grupos
            var empleadosGrupos = _serviceGrupo.CacheById.Values
                .Where(g => seccionIds.Contains(g.SeccionId))
                .SelectMany(g => g.EmpleadoIds ?? Enumerable.Empty<string>());

            // Unir y contar únicos
            return empleadosSecciones
                .Concat(empleadosGrupos)
                .Distinct()
                .Count();
        }
    }

    // ==============================
    // CONTEXTO PADRE
    // ==============================

    private Area? _areaPadre;

    public Area? AreaPadre
    {
        get => _areaPadre;
        set
        {
            SetProperty(ref _areaPadre, value);
            CrearEntityCommand.NotifyCanExecuteChanged();

            // Invalidar estadísticas al cambiar el área
            OnPropertyChanged(nameof(TotalSecciones));
            OnPropertyChanged(nameof(TotalGrupos));
            OnPropertyChanged(nameof(TotalEmpleados));

        }
    }

    // ==============================
    // CAN EXECUTE
    // ==============================

    protected override bool CanCrearEntity   => AreaPadre is not null;
    protected override bool CanEditarEntity  => EntitySelect is not null;
    protected override bool CanEliminarEntity => EntitySelect is not null;

    // ==============================
    // CICLO DE VIDA
    // ==============================


    protected override async Task OnDeactivateAsync()
    {
        DetachChildServices();
        await base.OnDeactivateAsync();
    }

    // ==============================
    // REFRESH — va al servidor pero solo actualiza caché, no añade
    // entidades de otras áreas a Entities
    // ==============================

    protected override async Task RefreshCacheAsync()
    {
        // Refrescamos la caché global de turnos (todos los turnos, no solo del área)
        // para mantenerla al día para SignalR y estadísticas cross-área.
        await base.RefreshCacheAsync();

        // Invalidar estadísticas tras el refresh
        OnPropertyChanged(nameof(TotalSecciones));
        OnPropertyChanged(nameof(TotalGrupos));
        OnPropertyChanged(nameof(TotalEmpleados));

    }

    // ==============================
    // CARGA FILTRADA
    // ==============================

    /// <summary>
    /// Puebla Entities solo con los turnos del área activa.
    /// Se llama en ActivateAsync y en CargarPageCommand (que llama RefreshCacheAsync + LoadEntitiesAsync).
    /// </summary>
    protected async override Task LoadEntitiesAsync()
    {
        Entities.Clear();
        EntitySelect = null;

        if (AreaPadre is null) return;

        var turnosFiltrados = ServicioBase.CacheById.Values
            .Where(t => t.AreaId == AreaPadre.Id)
            .OrderBy(t => t.Nombre);

        foreach (var turno in turnosFiltrados)
        {
            var wrapper = new EntityWrapper<Turno>(turno);

            await _referenceEnricher.EnrichAsync(wrapper);
            Entities.Add(wrapper);
        }

        return;
    }

    /// <summary>
    /// Filtra eventos SignalR para que solo afecten a turnos del área activa.
    /// </summary>
    protected override bool ShouldIncludeEntity(Turno entity)
        => AreaPadre is not null && entity.AreaId == AreaPadre.Id;

    // ==============================
    // CREAR CON CONTEXTO PADRE
    // ==============================

    public override async Task ConfirmarCrearEntityAsync(Turno? entity)
    {
        if (entity == null || AreaPadre == null) return;

        await DialogServiceI.MostrarDialogoProgreso(async () =>
        {
            var result = await ServiceTurno.CreateInAreaAsync(entity, AreaPadre.Id);
            result.ObjInteration = typeof(Turno);
            await DialogServiceI.ValidarRespuesta(result);
            return result;
        }, DialogDefaults.Progress);
    }

    protected override void UpdateChanged()
    {
        base.UpdateChanged();
    }
}