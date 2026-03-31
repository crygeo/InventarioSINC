using Cliente.Default;
using Cliente.Obj.Model;
using Cliente.Services.Model;
using CommunityToolkit.Mvvm.Input;

namespace Cliente.ViewModel.Model;

public partial class PageSeccionVM : ViewModelServiceBase<Seccion>
{
    private ServiceBase<Grupo>? _serviceGrupo;
    
    public ServiceSeccion ServiceSeccion => (ServiceSeccion)ServicioBase;

    // ==============================
    // CONTEXTO PADRE
    // ==============================

    private Turno? _turnoPadre;

    public Turno? TurnoPadre
    {
        get => _turnoPadre;
        set
        {
            SetProperty(ref _turnoPadre, value);
            CrearEntityCommand.NotifyCanExecuteChanged();
        }
    }
    
    public int TotalGrupos
    {
        get
        {
            if (_serviceGrupo is null || TurnoPadre is null) return 0;

            var seccionIds = ServicioBase.CacheById.Values
                .Where(s => TurnoPadre.Id == s.TurnoId)
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
            if ( _serviceGrupo is null || TurnoPadre is null) return 0;

            var seccionIds = ServicioBase.CacheById.Values
                .Where(s => TurnoPadre.Id == s.TurnoId)
                .Select(s => s.Id)
                .ToHashSet();

            // Empleados en secciones
            var empleadosSecciones = ServicioBase.CacheById.Values
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
    // CAN EXECUTE
    // ==============================

    protected override bool CanCrearEntity => TurnoPadre is not null;
    protected override bool CanEditarEntity => EntitySelect is not null;
    protected override bool CanEliminarEntity => EntitySelect is not null;

    // ==============================
    // CICLO DE VIDA
    // ==============================

    /// <summary>
    /// Carga secciones del turno activo desde la caché local.
    /// No realiza llamada al servidor.
    /// </summary>
    protected override async Task LoadEntitiesAsync()
    {
        Entities.Clear();
        EntitySelect = null;

        if (TurnoPadre is null) return;

        var seccionesFiltradas = ServicioBase.CacheById.Values
            .Where(s => s.TurnoId == TurnoPadre.Id)
            .OrderBy(s => s.Nombre);

        foreach (var seccion in seccionesFiltradas)
            Entities.Add(seccion);

        await Task.CompletedTask;
    }

    /// <summary>
    /// Solo acepta eventos SignalR de secciones que pertenecen al turno activo.
    /// </summary>
    protected override bool ShouldIncludeEntity(Seccion entity)
        => TurnoPadre is not null && entity.TurnoId == TurnoPadre.Id;

    // ==============================
    // CREAR CON CONTEXTO PADRE
    // ==============================

    public override async Task ConfirmarCrearEntityAsync(Seccion? entity)
    {
        if (entity == null || TurnoPadre == null) return;

        await DialogServiceI.MostrarDialogoProgreso(async () =>
        {
            var result = await ServiceSeccion.CreateInTurnoAsync(entity, TurnoPadre.Id);
            result.ObjInteration = typeof(Seccion);
            await DialogServiceI.ValidarRespuesta(result);
            return result;
        }, DialogDefaults.Progress);
    }
    
    public void InjectChildServices(ServiceBase<Grupo> serviceGrupo)
    {
        _serviceGrupo   = serviceGrupo;

        // Reaccionar a cambios en secciones/grupos para actualizar los contadores
        _serviceGrupo.CollectionChanged   += OnGrupoCollectionChanged;
    }
    
    private void OnGrupoCollectionChanged(EntityChangeType type, string id, Grupo? entity)
    {
        OnPropertyChanged(nameof(TotalGrupos));
        OnPropertyChanged(nameof(TotalEmpleados));
    }

    protected override void UpdateChanged()
    {
        base.UpdateChanged();
    }
}