using System.Collections.ObjectModel;
using Cliente.Default;
using Cliente.Extencions;
using Cliente.Helpers;
using Cliente.Services;
using Cliente.Services.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.Extensions;
using Shared.Interfaces;
using Shared.Interfaces.Model;
using Utilidades.Dialogs;
using Utilidades.Interfaces;
using Utilidades.Mvvm;

namespace Cliente.ViewModel.Model;

public class ViewModelServiceBase<TEntity> : ViewModelBase, IViewModelServiceBase<TEntity>
    where TEntity : class, IModelObj, ISelectable, new()
{
    // ==============================
    // CONSTANTES
    private const int TiempoEsperaSegundos = 5;

    // ==============================
    // ESTADO PRIVADO
    private bool _canRefresh = true;
    private bool _autoCarga = true;
    private EntityWrapper<TEntity>? _entitySelect;
    private int _progressValue;
    private bool _progressVisible;
    private int _pageIndex = 0;
    private int _pageSize = 50;
    private int _totalItems = 0;

    // ==============================
    // SERVICIOS
    protected DialogService DialogServiceI { get; } = DialogService.Instance;
    public ServiceBase<TEntity> ServicioBase { get; } = ServiceFactory.GetService<TEntity>();

    // ==============================
    // PROPIEDADES PÚBLICAS
    public ObservableCollection<EntityWrapper<TEntity>> Entities { get; } = new();
    
    public EntityWrapper<TEntity>? EntitySelect
    {
        get => _entitySelect;
        set
        {
            SetProperty(ref _entitySelect, value);
            EditarEntityCommand.NotifyCanExecuteChanged();
            EditarEntityCommand.NotifyCanExecuteChanged();
            EditarEntityFromItemCommand.NotifyCanExecuteChanged();
            EliminarEntityFromItemCommand.NotifyCanExecuteChanged();
        }
    }

    public bool CanRefresh
    {
        get => _canRefresh;
        set
        {
            if (SetProperty(ref _canRefresh, value))
                CargarPageCommand.NotifyCanExecuteChanged();
        }
    }

    public bool ProgressVisible
    {
        get => _progressVisible;
        set => SetProperty(ref _progressVisible, value);
    }

    public int ProgressValue
    {
        get => _progressValue;
        set => SetProperty(ref _progressValue, value);
    }

    public bool AutoCarga
    {
        get => _autoCarga;
        set => SetProperty(ref _autoCarga, value);
    }

    public int PageSize
    {
        get => _pageSize;
        set => SetProperty(ref _pageSize, value);
    }

    public int TotalItems
    {
        get => _totalItems;
        set => SetProperty(ref _totalItems, value);
    }

    public int PageIndex
    {
        get => _pageIndex;
        set => SetProperty(ref _pageIndex, value);
    }

    // ==============================
    // PROPIEDADES CALCULADAS
    protected bool CanNextPage => (PageIndex + 1) * PageSize < TotalItems;
    protected bool CanPreviousPage => PageIndex > 0;
    public Type EntityType => typeof(TEntity);

    // ==============================
    // CAN EXECUTE VIRTUALES
    protected virtual bool CanCrearEntity => true;
    protected virtual bool CanEditarEntity => true;
    protected virtual bool CanEliminarEntity => true;

    // ==============================
    // COMANDOS
    public IAsyncRelayCommand CargarPageCommand { get; protected set; }
    public IAsyncRelayCommand CrearEntityCommand { get; protected set; }
    public IAsyncRelayCommand EditarEntityCommand { get; protected set; }
    public IAsyncRelayCommand EliminarEntityCommand { get; protected set; }
    public IAsyncRelayCommand<TEntity> EditarEntityFromItemCommand { get; protected set; }
    public IAsyncRelayCommand<TEntity> EliminarEntityFromItemCommand { get; protected set; }
    public IAsyncRelayCommand<TEntity> CrearEntityFromItemCommand { get; protected set; }
    public IAsyncRelayCommand NextPageCommand { get; protected set; }
    public IAsyncRelayCommand PrevPageCommand { get; protected set; }

    protected ReferenceEnricher _referenceEnricher = new ReferenceEnricher(new ReferenceResolver());
    
    protected ViewModelServiceBase()
    {
        CargarPageCommand = new AsyncRelayCommand(
            CargarPageAsync,
            () => CanRefresh);

        CrearEntityCommand = new AsyncRelayCommand(
            CreateAsync,
            () => CanCrearEntity);

        EditarEntityCommand = new AsyncRelayCommand(
            UpdateAsync,
            () => CanEditarEntity);

        EliminarEntityCommand = new AsyncRelayCommand(
            DeleteAsync,
            () => CanEliminarEntity);

        EditarEntityFromItemCommand = new AsyncRelayCommand<TEntity>(
            UpdateFromItemAsync,
            entity => entity != null && CanEditarEntity);

        EliminarEntityFromItemCommand = new AsyncRelayCommand<TEntity>(
            DeleteFromItemAsync,
            entity => entity != null && CanEliminarEntity);

        CrearEntityFromItemCommand = new AsyncRelayCommand<TEntity>(
            CreateFromItemAsync,
            _ => CanCrearEntity);

        NextPageCommand = new AsyncRelayCommand(
            NextPageAsync,
            () => CanNextPage);

        PrevPageCommand = new AsyncRelayCommand(
            PrevPageAsync,
            () => CanPreviousPage);
    }

    // ==============================
    // CICLO DE VIDA
    // ==============================

    private bool _initialized;
    private bool _disposed;
    private bool _active;

    public virtual async Task ActivateAsync()
    {
        if (!_initialized)
        {
            _initialized = true;
            await InitInternalAsync();
        }

        if (_active) return;

        _active = true;
        await OnActivateAsync();
    }

    public virtual async Task DeactivateAsync()
    {
        if (!_active) return;

        _active = false;
        await OnDeactivateAsync();
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        OnDispose();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Se ejecuta una única vez: conecta SignalR y carga la caché global.
    /// Separado de OnActivateAsync para que los VMs singletons (PageTurnoVM, etc.)
    /// mantengan SignalR activo aunque salgan del NavigationStack temporalmente.
    /// </summary>
    protected virtual async Task InitInternalAsync()
    {
        await ServicioBase.InitializeAsync();
    }

    /// <summary>
    /// Se ejecuta cada vez que el VM entra al NavigationStack o se selecciona.
    /// Suscribe CollectionChanged para actualizar Entities y carga los datos filtrados.
    /// NO gestiona SignalR — eso lo hace InitInternalAsync/ShutdownSignalRAsync.
    /// </summary>
    protected virtual async Task OnActivateAsync()
    {
        // Evitar suscripcion duplicada si se activa mas de una vez sin desactivar
        ServicioBase.CollectionChanged -= OnServiceCollectionChanged;
        ServicioBase.CollectionChanged += OnServiceCollectionChanged;

        await RefreshCacheAsync();
        await LoadEntitiesAsync();
    }

    /// <summary>
    /// Se ejecuta cuando el VM sale del NavigationStack.
    /// Solo limpia la vista — NO desconecta SignalR ni destruye la caché.
    /// Los VMs singletons deben mantener la caché caliente para el filtrado rapido.
    /// SignalR se cierra unicamente en ShutdownSignalRAsync (llamado al hacer Dispose).
    /// </summary>
    protected virtual Task OnDeactivateAsync()
    {
        ServicioBase.CollectionChanged -= OnServiceCollectionChanged;
        Entities.Clear();
        EntitySelect = null;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Cierra la conexion SignalR. Llamado solo cuando el VM se destruye completamente
    /// (PageAreaVM.OnDeactivateAsync o Dispose), no al salir del stack.
    /// </summary>
    public virtual async Task ShutdownSignalRAsync()
    {
        await ServicioBase.ShutdownAsync();
    }

    protected virtual void OnDispose()
    {
        ServicioBase.CollectionChanged -= OnServiceCollectionChanged;
    }

    // ==============================
    // PUNTO DE EXTENSIÓN PRINCIPAL
    // ==============================

    /// <summary>
    /// Carga las entidades en <see cref="Entities"/>.
    /// Por defecto puebla desde la caché (ya actualizada por RefreshCacheAsync).
    /// Los VMs hijos con contexto padre sobreescriben este método para filtrar
    /// por su padre activo sin llamadas adicionales al servidor.
    /// </summary>
    protected virtual async Task LoadEntitiesAsync()
    {
        Entities.Clear();

        foreach (var entity in ServicioBase.CacheById.Values)
        {
            var wrapper = new EntityWrapper<TEntity>(entity);

            await _referenceEnricher.EnrichAsync(wrapper);

            Entities.Add(wrapper);
        }
    }

    // ==============================
    // REACCIÓN A SIGNALR
    // ==============================

    protected virtual async void OnServiceCollectionChanged(EntityChangeType type, string id, TEntity? entity)
    {
        switch (type)
        {
            case EntityChangeType.Created when entity != null:
            {
                if (!ShouldIncludeEntity(entity)) return;

                var wrapper = new EntityWrapper<TEntity>(entity);
                await _referenceEnricher.EnrichAsync(wrapper);

                Entities.Add(wrapper);
                break;
            }

            case EntityChangeType.Updated when entity != null:
            {
                var existing = Entities.FirstOrDefault(x => ((IIdentifiable)x.Model).Id == id);

                if (existing != null)
                {
                    var index = Entities.IndexOf(existing);

                    var wrapper = new EntityWrapper<TEntity>(entity);
                    await _referenceEnricher.EnrichAsync(wrapper);

                    Entities[index] = wrapper;
                }
                break;
            }

            case EntityChangeType.Deleted:
            {
                var toRemove = Entities.FirstOrDefault(x => ((IIdentifiable)x.Model).Id == id);
                if (toRemove != null)
                    Entities.Remove(toRemove);

                break;
            }
        }
    }

    /// <summary>
    /// Filtro de pertenencia al contexto padre actual.
    /// Por defecto acepta todas las entidades (comportamiento global).
    /// Los VMs hijos sobreescriben esto para filtrar por padre.
    /// Se usa en <see cref="OnServiceCollectionChanged"/> para que
    /// las actualizaciones SignalR respeten el contexto activo.
    /// </summary>
    protected virtual bool ShouldIncludeEntity(TEntity entity) => true;

    // ==============================
    // PAGINACIÓN
    // ==============================

    public async Task LoadPageAsync(int pageNumber)
    {
        PageIndex = pageNumber;
        await CargarPageAsync();
        OnPropertyChanged(nameof(CanNextPage));
        OnPropertyChanged(nameof(CanPreviousPage));
    }

    public async Task NextPageAsync()
    {
        if (!CanNextPage) return;
        await LoadPageAsync(PageIndex + 1);
    }

    public async Task PrevPageAsync()
    {
        if (!CanPreviousPage) return;
        await LoadPageAsync(PageIndex - 1);
    }

    // ==============================
    // CRUD PÚBLICO
    // ==============================

    public virtual async Task CreateAsync()
    {
        await DialogServiceI.BuscarMostrarDialogAsync(
            new TEntity(),
            $"Crear {ComponetesHelp.GetNombreEntidad<TEntity>(Pluralidad.Singular)}",
            ConfirmarCrearEntityAsync);
    }

    public virtual async Task UpdateAsync()
    {
        if (EntitySelect == null) return;

        await DialogServiceI.BuscarMostrarDialogAsync(
            EntitySelect.Clone(),
            $"Editar {ComponetesHelp.GetNombreEntidad<TEntity>(Pluralidad.Singular)}",
            ConfirmarEditarEntityAsync);
    }

    public virtual async Task DeleteAsync()
    {
        if (EntitySelect == null) return;

        var confirmDialog = new ConfirmDialog
        {
            TextHeader = $"Eliminar {ComponetesHelp.GetNombreEntidad<TEntity>(Pluralidad.Singular)}",
            Message = $"¿Estás seguro de que quieres eliminar el {ComponetesHelp.GetNombreEntidad<TEntity>(Pluralidad.Singular)} seleccionado?",
            AceptarCommand = new AsyncRelayCommand(ConfirmarEliminarEntityAsync),
            DialogNameIdentifier = DialogDefaults.Sub01,
            DialogOpenIdentifier = DialogDefaults.Main
        };

        await DialogServiceI.MostrarDialogo(confirmDialog);
    }

    // ==============================
    // CRUD DESDE ITEM (con parámetro explícito — evita bug de EntitySelect)
    // ==============================

    private async Task CreateFromItemAsync(TEntity? _)
    {
        await CreateAsync();
    }

    private async Task UpdateFromItemAsync(TEntity? entity)
    {
        if (entity == null) return;

        await DialogServiceI.BuscarMostrarDialogAsync(
            entity.Clone(),
            $"Editar {ComponetesHelp.GetNombreEntidad<TEntity>(Pluralidad.Singular)}",
            ConfirmarEditarEntityAsync);
    }

    private async Task DeleteFromItemAsync(TEntity? entity)
    {
        if (entity == null) return;

        var confirmDialog = new ConfirmDialog
        {
            TextHeader = $"Eliminar {ComponetesHelp.GetNombreEntidad<TEntity>(Pluralidad.Singular)}",
            Message = $"¿Eliminar elemento seleccionado?",
            AceptarCommand = new AsyncRelayCommand(async () =>
            {
                await DialogServiceI.MostrarDialogoProgreso(async () =>
                {
                    var result = await ServicioBase.DeleteAsync(entity.Id);
                    result.ObjInteration = typeof(TEntity);
                    await DialogServiceI.ValidarRespuesta(result);
                    return result;
                }, DialogDefaults.Progress);
            }),
            DialogNameIdentifier = DialogDefaults.Sub01,
            DialogOpenIdentifier = DialogDefaults.Main
        };

        await DialogServiceI.MostrarDialogo(confirmDialog);
    }

    // ==============================
    // CRUD CONFIRMACIONES
    // ==============================

    public virtual async Task ConfirmarCrearEntityAsync(TEntity? entity)
    {
        if (entity == null) return;

        await DialogServiceI.MostrarDialogoProgreso(async () =>
        {
            var result = await ServicioBase.CreateAsync(entity);
            result.ObjInteration = typeof(TEntity);
            await DialogServiceI.ValidarRespuesta(result);
            return result;
        }, DialogDefaults.Progress);
    }

    private async Task ConfirmarEditarEntityAsync(EntityWrapper<TEntity>? entity)
    {
        if (entity == null) return;

        var entityOriginal = ServicioBase.GetFromCache(entity.Model.Id);
        if (entityOriginal == null) return;

        var changes = entityOriginal.GetChanges(entity);
        if (changes.Count == 0) return;

        foreach (var property in changes)
        {
            if (property.NewValue == null) continue;

            await DialogServiceI.MostrarDialogoProgreso(async () =>
            {
                var result = await ServicioBase.PropertyUpdateAsync(entity.Model.Id, property.Name, property.NewValue);
                result.ObjInteration = typeof(TEntity);
                await DialogServiceI.ValidarRespuesta(result);
                return result;
            }, DialogDefaults.Progress);
        }
    }

    private async Task ConfirmarEliminarEntityAsync()
    {
        if (EntitySelect == null) return;

        await DialogServiceI.MostrarDialogoProgreso(async () =>
        {
            var result = await ServicioBase.DeleteAsync(EntitySelect.Id);
            result.ObjInteration = typeof(TEntity);
            await DialogServiceI.ValidarRespuesta(result);
            return result;
        }, DialogDefaults.Progress);
    }

    // ==============================
    // CARGA INTERNA
    // ==============================

    /// <summary>
    /// Punto de entrada del comando CargarPageCommand.
    /// Separa dos responsabilidades:
    /// 1. RefreshCacheAsync → va al servidor y actualiza la caché global.
    /// 2. LoadEntitiesAsync → puebla Entities (puede ser filtrado desde caché).
    ///
    /// Los VMs con contexto padre (PageTurnoVM, etc.) sobreescriben solo
    /// LoadEntitiesAsync — el refresh del servidor sigue siendo global
    /// para mantener la caché caliente para todos los contextos.
    /// </summary>
    private async Task CargarPageAsync()
    {
        if (!CanRefresh) return;

        CanRefresh = false;
        ProgressVisible = true;
        ProgressValue = 0;

        _ = StartCooldownAsync(TiempoEsperaSegundos);

        await RefreshCacheAsync();
        await LoadEntitiesAsync();
    }

    /// <summary>
    /// Actualiza la caché global desde el servidor.
    /// No toca Entities directamente — eso es responsabilidad de LoadEntitiesAsync.
    /// Los VMs base (PageAreaVM) usan esto para poblar también su colección.
    /// Los VMs hijos lo heredan sin cambios — solo refrescan su caché global.
    /// </summary>
    protected virtual async Task RefreshCacheAsync()
    {
        var result = await ServicioBase.GetPagedAsync(PageIndex, PageSize);
        await DialogServiceI.ValidarRespuesta(result);

        if (result.Success)
        {
            foreach (var entity in result.EntityGet.Items)
                ServicioBase.CacheById[entity.Id] = entity;

            TotalItems = result.EntityGet.TotalCount;
        }
    }

    private async Task StartCooldownAsync(int seconds)
    {
        const int steps = 100;
        int delay = (seconds * 1000) / steps;

        for (int i = 0; i <= steps; i++)
        {
            ProgressValue = i;
            await Task.Delay(delay);
        }

        ProgressVisible = false;
        CanRefresh = true;
    }

    protected override void UpdateChanged() { }
}