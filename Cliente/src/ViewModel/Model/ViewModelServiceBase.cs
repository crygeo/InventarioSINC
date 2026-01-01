using System.Collections.ObjectModel;
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
    private readonly int _tiempoEsperaS = 5; // Variable para controlar si ya estamos en el proceso de carga
    private bool _isLoading; // Variable para controlar si ya estamos en el proceso de carga


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
    public ObservableCollection<TEntity> Entitys { get; } = [];
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

    protected virtual bool CanCargarPage => true;
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
            () => CanCargarPage
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

        _ = InitAsync(); // 🔥 fire & forget CONTROLADO
    }

    #region Metodos de inicialización y cierre

    protected virtual async Task InitAsync()
    {
        await ServicioBase.InitializeAsync();
        await LoadPageAsync(0);
    }

    protected async Task StopAsync()   
    {
        await ServicioBase.ShutdownAsync();
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

        await DialogServiceI.MostrarDialogoProgreso(async () =>
        {
            var result = await ServicioBase.UpdateAsync(entity.Id, entity);
            result.ObjInteration = typeof(TEntity);
            await DialogServiceI.ValidarRespuesta(result);
            return result;
        }, DialogDefaults.Progress);
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
        if (_isLoading)
            return;

        _isLoading = true;
        ProgressVisible = true;
        ProgressValue = 0;

        var progressTask = Task.Run(async () =>
        {
            for (var i = 0; i <= 100; i++)
            {
                ProgressValue = i;
                await Task.Delay(_tiempoEsperaS * 10);
            }

            ProgressVisible = false;
            _isLoading = false;
        });

        var result = await ServicioBase.GetPagedAsync(PageIndex, PageSize);
        await DialogServiceI.ValidarRespuesta(result);
        if (result.Success)
        {
            Entitys.Clear();
            foreach (var entity in result.EntityGet!.Items)
            {
                Entitys.Add(entity);
                ServicioBase.CacheById[entity.Id] = entity;
            }

            TotalItems = result.EntityGet.TotalCount;
        }

        await progressTask;
    }

    protected override void UpdateChanged()
    {
    }
}