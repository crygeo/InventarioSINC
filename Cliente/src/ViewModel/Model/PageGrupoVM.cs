using Cliente.Default;
using Cliente.Obj.Model;
using Cliente.Services.Model;
using CommunityToolkit.Mvvm.Input;

namespace Cliente.ViewModel.Model;

public partial class PageGrupoVM : ViewModelServiceBase<Grupo>
{
    public ServiceGrupo ServiceGrupo => (ServiceGrupo)ServicioBase;

    public int TotalEmpleados
    {
        get
        {
            if(SeccionPadre is null) return 0;
            
            // Empleados en grupos
            var empleadosGrupos = ServiceGrupo.CacheById.Values
                .Where(g => SeccionPadre.Id == g.SeccionId)
                .SelectMany(g => g.EmpleadoIds ?? Enumerable.Empty<string>());

            // Unir y contar únicos
            return empleadosGrupos
                .Distinct()
                .Count();
        }
    }

    // ==============================
    // CONTEXTO PADRE
    // ==============================

    private Seccion? _seccionPadre;

    public Seccion? SeccionPadre
    {
        get => _seccionPadre;
        set
        {
            SetProperty(ref _seccionPadre, value);
            CrearEntityCommand.NotifyCanExecuteChanged();
        }
    }

    // ==============================
    // CAN EXECUTE
    // ==============================

    protected override bool CanCrearEntity => SeccionPadre is not null;
    protected override bool CanEditarEntity => EntitySelect is not null;
    protected override bool CanEliminarEntity => EntitySelect is not null;

    // ==============================
    // CICLO DE VIDA
    // ==============================

    /// <summary>
    /// Carga grupos de la sección activa desde la caché local.
    /// No realiza llamada al servidor.
    /// </summary>
    protected override async Task LoadEntitiesAsync()
    {
        Entities.Clear();
        EntitySelect = null;

        if (SeccionPadre is null) return;

        var gruposFiltrados = ServicioBase.CacheById.Values
            .Where(g => g.SeccionId == SeccionPadre.Id)
            .OrderBy(g => g.Nombre);

        foreach (var grupo in gruposFiltrados)
            Entities.Add(grupo);

        await Task.CompletedTask;
    }

    /// <summary>
    /// Solo acepta eventos SignalR de grupos que pertenecen a la sección activa.
    /// </summary>
    protected override bool ShouldIncludeEntity(Grupo entity)
        => SeccionPadre is not null && entity.SeccionId == SeccionPadre.Id;

    // ==============================
    // CREAR CON CONTEXTO PADRE
    // ==============================

    public override async Task CreateAsync()
    {
        if (SeccionPadre is not null && !SeccionPadre.EsGrupo)
        {
            DialogServiceI.MensajeQueue.Enqueue("Esta sección no permite grupos.");
            return;
        }

        await base.CreateAsync();
    }

    public override async Task ConfirmarCrearEntityAsync(Grupo? entity)
    {
        if (entity == null || SeccionPadre == null) return;

        await DialogServiceI.MostrarDialogoProgreso(async () =>
        {
            var result = await ServiceGrupo.CreateInSeccionAsync(entity, SeccionPadre.Id);
            result.ObjInteration = typeof(Grupo);
            await DialogServiceI.ValidarRespuesta(result);
            return result;
        }, DialogDefaults.Progress);
    }

    protected override void UpdateChanged()
    {
        base.UpdateChanged();
    }
}