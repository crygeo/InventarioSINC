using System.Collections.ObjectModel;
using System.Diagnostics;
using Cliente.Default;
using Cliente.Extencions;
using Cliente.Helpers;
using Cliente.Services.Model;
using CommunityToolkit.Mvvm.Input;
using Shared.Extensions;
using Shared.Interfaces.Model;
using Utilidades.Dialogs;
using Utilidades.Interfaces;
using Utilidades.Mvvm;

namespace Cliente.ViewModel.Model;

public class ViewModelServiceBase<TEntity> : ViewModelBase, IViewModelServiceBase<TEntity>
    where TEntity : class, IModelObj, ISelectable, new()
{
    // ==============================
    // VARIABLES PRIVADAS
    private const int TiempoEsperaSegundos = 5; // Variable para controlar si ya estamos en el proceso de carga


    private bool _canRefresh = true; // Variable para controlar si ya estamos en el proceso de carga
    private bool _autoCarga = true;
    private TEntity? _entitySelect;
    private int _progressValue;
    private bool _progressVisible;

    private int _pageIndex = 0;
    private int _pageSize = 50;
    private int _totalItems = 0;

    // ==============================
    // SERVICIOS
    protected DialogService DialogServiceI { get; set; } = DialogService.Instance;
    public ServiceBase<TEntity> ServicioBase { get; } = ServiceFactory.GetService<TEntity>();


    // ==============================
    // PROPIEDADES PÚBLICAS
    public ObservableCollection<TEntity> Entities { get; } = [];

    public TEntity? EntitySelect
    {
        get => _entitySelect;
        set
        {
            SetProperty(ref _entitySelect, value);
            EditarEntityCommand.NotifyCanExecuteChanged();
            EliminarEntityCommand.NotifyCanExecuteChanged();
        }
    }

    public bool CanRefresh
    {
        get => _canRefresh;
        set
        {
            if(SetProperty(ref _canRefresh, value))
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

    protected bool CanNextPage => (PageIndex + 1) * PageSize < TotalItems;
    protected bool CanPreviousPage => PageIndex > 0;

    public Type EntityType => typeof(TEntity);

    /* ==============================
      MÉTODOS VIRTUALES CanExecute
      ============================== */

    protected virtual bool CanCrearEntity => true;
    protected virtual bool CanEditarEntity => true;
    protected virtual bool CanEliminarEntity => true;

    /* ==============================
        COMANDOS ASÍNCRONOS
      ============================== */
    public IAsyncRelayCommand CargarPageCommand { get; protected set; }
    public IAsyncRelayCommand CrearEntityCommand { get; protected set; }
    public IAsyncRelayCommand EditarEntityCommand { get; protected set; }
    public IAsyncRelayCommand EliminarEntityCommand { get; protected set; }

    public IAsyncRelayCommand NextPageCommand { get; protected set; }
    public IAsyncRelayCommand PrevPageCommand { get; protected set; }


    /// <summary>
    /// </summary>
    /// <param name="typeService">
    ///     Tipo del servicio para crear la instacia del servicio, debe ser de tipo
    ///     <see cref="ServiceBase" /> para que funcione
    /// </param>
    /// <exception cref="Exception">Si el tipo no es correcto lanzara un error</exception>
    protected ViewModelServiceBase()
    {
        CargarPageCommand = new AsyncRelayCommand(
            CargarPageAsync,
            () => CanRefresh
        );

        CrearEntityCommand = new AsyncRelayCommand(
            CreateAsync,
            () => CanCrearEntity
        );

        EditarEntityCommand = new AsyncRelayCommand(
            UpdateAsync,
            () => CanEditarEntity
        );

        EliminarEntityCommand = new AsyncRelayCommand(
            DeleteAsync,
            () => CanEliminarEntity
        );

        NextPageCommand = new AsyncRelayCommand(
            NextPageAsync,
            () => CanNextPage
        );
        PrevPageCommand = new AsyncRelayCommand(
            PrevPageAsync,
            () => CanPreviousPage
        );
    }

    #region Metodos de inicialización y cierre

    private bool _initialized;
    private bool _disposed;
    private bool _active;

    public virtual async Task ActivateAsync()
    {

        if (!_initialized)
        {
            _initialized = true;
            await InitInternalAsync(); // SOLO una vez
        }

        if (_active) return; // ya activo

        _active = true;
        await OnActivateAsync(); // se puede repetir
        
        


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

    
    protected virtual async Task InitInternalAsync()
    {
        await ServicioBase.InitializeAsync();
    }

    protected virtual async Task OnActivateAsync()
    {
        ServicioBase.CollectionChanged += OnServiceCollectionChanged;
        await LoadPageAsync(0);
    }

    protected virtual async Task OnDeactivateAsync()
    {
        ServicioBase.CollectionChanged -= OnServiceCollectionChanged;
        await ServicioBase.ShutdownAsync();
    }

    protected virtual void OnDispose()
    {
        ServicioBase.CollectionChanged -= OnServiceCollectionChanged;
        //ServicioBase.Dispose();
    }

    
    protected virtual void OnServiceCollectionChanged(EntityChangeType type, string id, TEntity? entity)
    {
        switch (type)
        {
            case EntityChangeType.Created:
                Entities.Add(entity!);
                break;

            case EntityChangeType.Updated:
                var existing = Entities.FirstOrDefault(x => x.Id == id);
                if (existing != null)
                {
                    var index = Entities.IndexOf(existing);
                    Entities[index] = entity!;
                }
                break;

            case EntityChangeType.Deleted:
                var toRemove = Entities.FirstOrDefault(x => x.Id == id);
                if (toRemove != null)
                    Entities.Remove(toRemove);
                break;
        }
    }

    #endregion

    #region Metodos de paginación

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

    #endregion

    #region MÉTODOS Crud Asíncronos

    public virtual async Task CreateAsync()
    {
        await DialogServiceI.BuscarMostrarDialogAsync(
            new TEntity(),
            $"Crear {ComponetesHelp.GetNombreEntidad<TEntity>(Pluralidad.Singular)}",
            ConfirmarCrearEntityAsync
        );
    }

    public virtual async Task UpdateAsync()
    {
        if (EntitySelect == null)
            return;


        await DialogServiceI.BuscarMostrarDialogAsync(
            EntitySelect.Clone(),
            $"Editar {ComponetesHelp.GetNombreEntidad<TEntity>(Pluralidad.Singular)}",
            ConfirmarEditarEntityAsync
        );
    }

    public virtual async Task DeleteAsync()
    {
        if (EntitySelect == null)
            return;

        var confirmDialog = new ConfirmDialog
        {
            TextHeader = $"Eliminar {ComponetesHelp.GetNombreEntidad<TEntity>(Pluralidad.Singular)}",
            Message =
                $"¿Estás seguro de que quieres eliminar el {ComponetesHelp.GetNombreEntidad<TEntity>(Pluralidad.Singular)} seleccionado?",
            AceptarCommand = new AsyncRelayCommand(ConfirmarEliminarEntityAsync),
            DialogNameIdentifier = DialogDefaults.Sub01,
            DialogOpenIdentifier = DialogDefaults.Main
        };

        await DialogServiceI.MostrarDialogo(confirmDialog);
    }

    #endregion

    #region MÉTODOS PRIVADOS CRUD

    public virtual async Task ConfirmarCrearEntityAsync(TEntity? entity)
    {
        if (entity == null)
            return;

        await DialogServiceI.MostrarDialogoProgreso(async () =>
        {
            var result = await ServicioBase.CreateAsync(entity);
            result.ObjInteration = typeof(TEntity);
            await DialogServiceI.ValidarRespuesta(result);
            return result;
        }, DialogDefaults.Progress);
    }

    private async Task ConfirmarEditarEntityAsync(TEntity? entity)
    {
        if (entity == null) return;
        var entityOriginal = ServicioBase.GetFromCache(entity.Id);
        if (entityOriginal == null) return;

        var changes = entityOriginal.GetChanges(entity);
        if (changes.Count < 0) return;

        foreach (var property in changes)
        {
            if (property.NewValue != null)
            {
                await DialogServiceI.MostrarDialogoProgreso(async () =>
                {
                    var result = await ServicioBase.PropertyUpdateAsync(entity.Id, property.Name, property.NewValue);
                    result.ObjInteration = typeof(TEntity);
                    await DialogServiceI.ValidarRespuesta(result);
                    return result;
                }, DialogDefaults.Progress);
            }
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

    #endregion

    // ==============================
    // MÉTODOS PRIVADOS
    // ==============================
    private async Task CargarPageAsync()
    {
        #region  CanRefresh
        if (!CanRefresh)
            return;

        CanRefresh = false;
        ProgressVisible = true;
        ProgressValue = 0;

        _ = StartCooldownAsync(TiempoEsperaSegundos); // 5 segundos

        #endregion

        var result = await ServicioBase.GetPagedAsync(PageIndex, PageSize);
        await DialogServiceI.ValidarRespuesta(result);
        if (result.Success)
        {
            Entities.Clear();
            foreach (var entity in result.EntityGet!.Items)
            {
                Entities.Add(entity);
                ServicioBase.CacheById[entity.Id] = entity;
            }

            TotalItems = result.EntityGet.TotalCount;
        }

    }
    
    private async Task StartCooldownAsync(int seconds)
    {
        int steps = 100;
        int delay = (seconds * 1000) / steps;

        for (int i = 0; i <= steps; i++)
        {
            ProgressValue = i;
            await Task.Delay(delay);
        }

        ProgressVisible = false;
        CanRefresh = true;
    }


    protected override void UpdateChanged()
    {
    }
}