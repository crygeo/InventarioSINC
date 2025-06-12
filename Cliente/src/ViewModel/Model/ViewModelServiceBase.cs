using System.Collections.ObjectModel;
using Cliente.Helpers;
using Cliente.Services;
using Cliente.Services.Model;
using Cliente.View.Dialog;
using CommunityToolkit.Mvvm.Input;
using Shared.Extensions;
using Shared.Interfaces.Model;
using Utilidades.Interfaces;
using Utilidades.Mvvm;

namespace Cliente.ViewModel.Model;

public class ViewModelServiceBase<TEntity> : ViewModelBase where TEntity : class, IModelObj, ISelectable, new()
{
    protected DialogService DialogServiceI { get; set; } = DialogService.Instance;

    public ServiceBase<TEntity> ServicioBase { get; } = ServiceFactory.GetService<TEntity>();
    public ObservableCollection<TEntity> Entitys { get => ServicioBase.Collection; }

    private TEntity? _entitySelect;
    public TEntity? EntitySelect
    {
        get { return _entitySelect; }
        set { SetProperty(ref _entitySelect, value); }
    }

    private bool _progressVisible;
    public bool ProgressVisible
    {
        get => _progressVisible;
        set => SetProperty(ref _progressVisible, value);
    }
    private int _progressValue;
    public int ProgressValue
    {
        get => _progressValue;
        set => SetProperty(ref _progressValue, value);
    }

    private bool _autoCarga = true;
    public bool AutoCarga
    {
        get => _autoCarga;
        set => SetProperty(ref _autoCarga, value);
    }

    public Type TypeEntity { get => typeof(TEntity); }

    private bool _isLoading = false; // Variable para controlar si ya estamos en el proceso de carga
    private int _tiempoEsperaS = 5; // Variable para controlar si ya estamos en el proceso de carga


    public IAsyncRelayCommand CargarEntityCommand { get; }
    public IAsyncRelayCommand CrearEntityCommand { get; }
    public IAsyncRelayCommand EditarEntityCommand { get; }
    public IAsyncRelayCommand EliminarEntityCommand { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="typeService">Tipo del servicio para crear la instacia del servicio, debe ser de tipo <see cref="ServiceBase"/> para que funcione</param>
    /// <exception cref="Exception">Si el tipo no es correcto lanzara un error</exception>
    public ViewModelServiceBase()
    {
        CargarEntityCommand = new AsyncRelayCommand((a) => CargarEntitysAsync());
        CrearEntityCommand = new AsyncRelayCommand((a) => CrearEntityAsync());
        EditarEntityCommand = new AsyncRelayCommand((a) => EditarEntityAsync());
        EliminarEntityCommand = new AsyncRelayCommand((a) => DeleteEntityAsync());

        InitAsync();
    }

    public virtual async void InitAsync()
    {
        var result = await ServicioBase.InitAsync();
        await DialogServiceI.ValidarRespuesta(result);
    }


    public virtual async void StopAsync() => await ServicioBase.StopAsync();


    public virtual async Task CargarEntitysAsync()
    {
        if (_isLoading)
            return;

        _isLoading = true;
        ProgressVisible = true;
        ProgressValue = 0;

        var progressTask = Task.Run(async () =>
        {
            for (int i = 0; i <= 100; i++)
            {
                ProgressValue = i;
                await Task.Delay(_tiempoEsperaS * 10);
            }

            ProgressVisible = false;
            _isLoading = false;
        });

        var result = await ServicioBase.UpdateCollection();
        await DialogServiceI.ValidarRespuesta(result);
        if (result.Success)
            Entitys.FirstOrDefault()?.Select();

        await progressTask;

    }


    public async virtual Task CrearEntityAsync()
    {

        await DialogService.BuscarYMostrarFormularioAsyncMain(
            new TEntity(),
            $"Crear {ComponetesHelp.GetNombreEntidad<TEntity>(Pluralidad.Singular)}",
            ConfirmarCrearEntityAsync
        );
    }
    public async virtual Task EditarEntityAsync()
    {
        if (EntitySelect == null)
            return;


        await DialogService.BuscarYMostrarFormularioAsyncMain(
            EntitySelect.Clone(),
            $"Editar {ComponetesHelp.GetNombreEntidad<TEntity>(Pluralidad.Singular)}",
            ConfirmarEditarEntityAsync
        );

    }
    public async virtual Task DeleteEntityAsync()
    {
        if (EntitySelect == null)
            return;

        var confirmDialog = new ConfirmDialog
        {
            TextHeader = $"Eliminar {ComponetesHelp.GetNombreEntidad<TEntity>(Pluralidad.Singular)}",
            Message = $"¿Estás seguro de que quieres eliminar el {ComponetesHelp.GetNombreEntidad<TEntity>(Pluralidad.Singular)} seleccionado?",
            AceptarCommand = new AsyncRelayCommand(ConfirmarEliminarEntityAsync),
            DialogNameIdentifier = DialogService.DialogSub01,
            DialogOpenIdentifier = DialogService.DialogIdentifierMain
        };

        await DialogServiceI.MostrarDialogo(confirmDialog);
    }


    private async Task ConfirmarCrearEntityAsync(TEntity? entity)
    {
        if (entity == null)
            return;

        await DialogServiceI.MostrarDialogoProgreso(async () =>
        {
            var result = await ServicioBase.CreateAsync(entity);
            result.ObjInteration = typeof(TEntity);
            await DialogServiceI.ValidarRespuesta(result);
            return result;
        });
    }
    private async Task ConfirmarEditarEntityAsync(TEntity? entity)
    {
        if (EntitySelect == null) return;

        await DialogServiceI.MostrarDialogoProgreso(async () =>
        {
            var result = await ServicioBase.UpdateAsync(entity.Id, entity);
            result.ObjInteration = typeof(TEntity);
            await DialogServiceI.ValidarRespuesta(result);
            return result;
        });
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
        });
    }


    protected override void UpdateChanged()
    {
    }
}