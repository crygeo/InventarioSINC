// Cliente/src/ViewModel/Model/PageEmpleadoVM.cs
using System.Collections.ObjectModel;
using Cliente.Default;
using Cliente.Obj.Model;
using Cliente.Services.Model;
using CommunityToolkit.Mvvm.Input;
using Shared.Interfaces.Model;
using Utilidades.Dialogs;
using Utilidades.Interfaces;
using Utilidades.Mvvm;

namespace Cliente.ViewModel.Model.Sub;

/// <summary>
/// VM para la página de asignación de empleados a un contenedor
/// (Seccion.EsGrupo=false o Grupo).
///
/// Responsabilidades:
/// - Mostrar empleados ya asignados al contenedor.
/// - Buscar empleados existentes en el sistema.
/// - Agregar/quitar IDs de la lista del contenedor.
///
/// Este VM NO crea empleados — solo gestiona la asignación.
/// El tipo del contenedor se abstrae mediante delegates.
/// </summary>
public class PageEmpleadoAsignacionVM : ViewModelBase, IActivable, IDeactivable
{
    private readonly ServiceBase<Empleado> _serviceEmpleado;
    private readonly DialogService   _dialogService = DialogService.Instance;

    // Delegates — el VM no conoce Seccion ni Grupo
    private Func<object, Task<IResultResponse<bool>>>? _addToList;
    private Func<object, Task<IResultResponse<bool>>>? _removeFromList;
    private Func<IEnumerable<string>>?                                  _getEmpleadoIds;

    private string    _nombreContexto = string.Empty;
    private int       _cupo;
    private string    _textoBusqueda  = string.Empty;
    private Empleado? _empleadoSelect;
    private bool      _progressVisible;
    private bool      _canRefresh = true;

    // ==============================
    // PROPIEDADES PÚBLICAS
    // ==============================

    /// <summary>Nombre del contenedor — encabezado de la página.</summary>
    public string NombreContexto
    {
        get => _nombreContexto;
        private set => SetProperty(ref _nombreContexto, value);
    }

    /// <summary>Cupo máximo del contenedor. 0 = sin límite.</summary>
    public int Cupo
    {
        get => _cupo;
        private set => SetProperty(ref _cupo, value);
    }

    /// <summary>Empleados actualmente asignados al contenedor.</summary>
    public ObservableCollection<Empleado> EmpleadosAsignados { get; } = [];

    /// <summary>Resultados de la búsqueda — empleados disponibles para asignar.</summary>
    public ObservableCollection<Empleado> ResultadosBusqueda { get; } = [];

    public Empleado? EmpleadoSelect
    {
        get => _empleadoSelect;
        set => SetProperty(ref _empleadoSelect, value);
    }

    public string TextoBusqueda
    {
        get => _textoBusqueda;
        set
        {
            if (SetProperty(ref _textoBusqueda, value))
                _ = BuscarAsync();
        }
    }

    public bool ProgressVisible
    {
        get => _progressVisible;
        set => SetProperty(ref _progressVisible, value);
    }

    /// <summary>Cantidad de empleados asignados vs cupo.</summary>
    public string ResumenCupo =>
        Cupo > 0
            ? $"{EmpleadosAsignados.Count} / {Cupo}"
            : $"{EmpleadosAsignados.Count}";

    /// <summary>True cuando se alcanzó el cupo máximo.</summary>
    public bool CupoAlcanzado =>
        Cupo > 0 && EmpleadosAsignados.Count >= Cupo;

    // ==============================
    // COMANDOS
    // ==============================

    public IAsyncRelayCommand           CargarCommand  { get; }
    public IAsyncRelayCommand<Empleado> AsignarCommand { get; }
    public IAsyncRelayCommand<Empleado> QuitarCommand  { get; }

    // ==============================
    // CONSTRUCTOR
    // ==============================

    public PageEmpleadoAsignacionVM(ServiceBase<Empleado> serviceEmpleado)
    {
        _serviceEmpleado = serviceEmpleado;

        CargarCommand  = new AsyncRelayCommand(CargarAsync, () => _canRefresh);

        AsignarCommand = new AsyncRelayCommand<Empleado>(
            AsignarAsync,
            e => e is not null && !CupoAlcanzado && !EstaAsignado(e));

        QuitarCommand = new AsyncRelayCommand<Empleado>(
            QuitarAsync,
            e => e is not null);
    }

    // ==============================
    // CONFIGURACIÓN DE CONTEXTO
    // ==============================

    /// <summary>
    /// Configura el contexto del VM para un contenedor específico.
    /// Llamado por PageAreaVM antes de hacer Push al NavigationStack.
    /// </summary>
    /// <param name="nombreContexto">Texto del encabezado.</param>
    /// <param name="cupo">Cupo máximo. 0 = sin límite.</param>
    /// <param name="getEmpleadoIds">Función que retorna los IDs actuales del contenedor.</param>
    /// <param name="addToList">Delegate para agregar un ID al contenedor.</param>
    /// <param name="removeFromList">Delegate para quitar un ID del contenedor.</param>
    public void SetContexto(
        string nombreContexto,
        int    cupo,
        Func<IEnumerable<string>>                    getEmpleadoIds,
        Func<object, Task<IResultResponse<bool>>>    addToList,
        Func<object, Task<IResultResponse<bool>>>    removeFromList)
    {
        NombreContexto  = nombreContexto;
        Cupo            = cupo;
        _getEmpleadoIds = getEmpleadoIds;
        _addToList      = addToList;
        _removeFromList = removeFromList;

        EmpleadoSelect = null;
        EmpleadosAsignados.Clear();
        ResultadosBusqueda.Clear();
        TextoBusqueda = string.Empty;
    }

    // ==============================
    // CICLO DE VIDA
    // ==============================

    private bool _initialized;

    public async Task ActivateAsync()
    {
        if (!_initialized)
        {
            _initialized = true;
            await _serviceEmpleado.InitializeAsync();
            _serviceEmpleado.CollectionChanged += OnEmpleadoCollectionChanged;
        }

        await CargarAsync();
    }

    public Task DeactivateAsync()
    {
        EmpleadosAsignados.Clear();
        ResultadosBusqueda.Clear();
        EmpleadoSelect = null;
        return Task.CompletedTask;
    }

    public async Task ShutdownAsync()
    {
        _serviceEmpleado.CollectionChanged -= OnEmpleadoCollectionChanged;
        await _serviceEmpleado.ShutdownAsync();
    }

    // ==============================
    // CARGA
    // ==============================

    private async Task CargarAsync()
    {
        if (_getEmpleadoIds is null) return;

        ProgressVisible = true;
        _canRefresh     = false;
        CargarCommand.NotifyCanExecuteChanged();

        // Asegurar que la caché de empleados esté actualizada
        await _serviceEmpleado.GetPagedAsync(0, 500);

        PopularAsignados();
        NotificarCupo();

        ProgressVisible = false;
        _canRefresh     = true;
        CargarCommand.NotifyCanExecuteChanged();
    }

    /// <summary>
    /// Reconstruye EmpleadosAsignados desde la caché local.
    /// O(n) sobre los IDs del contenedor — sin llamada al servidor.
    /// </summary>
    private void PopularAsignados()
    {
        EmpleadosAsignados.Clear();

        if (_getEmpleadoIds is null) return;

        foreach (var id in _getEmpleadoIds())
        {
            var empleado = _serviceEmpleado.GetFromCache(id);
            if (empleado is not null)
                EmpleadosAsignados.Add(empleado);
        }
    }

    // ==============================
    // BÚSQUEDA
    // ==============================

    private CancellationTokenSource? _searchCts;

    /// <summary>
    /// Búsqueda con debounce de 300ms sobre la caché local.
    /// No realiza llamadas al servidor — los empleados ya están en caché.
    /// </summary>
    private async Task BuscarAsync()
    {
        _searchCts?.Cancel();
        _searchCts = new CancellationTokenSource();
        var token  = _searchCts.Token;

        try
        {
            await Task.Delay(300, token);
        }
        catch (OperationCanceledException)
        {
            return;
        }

        ResultadosBusqueda.Clear();

        var texto = TextoBusqueda.Trim().ToLowerInvariant();

        if (string.IsNullOrEmpty(texto))
            return;

        var resultados = _serviceEmpleado.CacheById.Values
            .Where(e => !EstaAsignado(e))
            .Where(e =>
                e.NombreCompleto.ToLowerInvariant().Contains(texto) ||
                e.Cedula.Contains(texto))
            .Take(20);

        foreach (var emp in resultados)
            ResultadosBusqueda.Add(emp);
    }

    // ==============================
    // ASIGNACIÓN
    // ==============================

    private async Task AsignarAsync(Empleado? empleado)
    {
        if (empleado is null || _addToList is null || _getEmpleadoIds is null) return;
        if (CupoAlcanzado) return;
        if (EstaAsignado(empleado)) return;

        await _dialogService.MostrarDialogoProgreso(async () =>
        {
            var result = await _addToList(empleado.Id);

            result.ObjInteration = typeof(Empleado);
            await _dialogService.ValidarRespuesta(result);

            if (result.Success)
            {
                EmpleadosAsignados.Add(empleado);
                ResultadosBusqueda.Remove(empleado);
                NotificarCupo();
            }

            return result;
        }, DialogDefaults.Progress);
    }

    private async Task QuitarAsync(Empleado? empleado)
    {
        if (empleado is null || _removeFromList is null) return;

        await _dialogService.MostrarDialogoProgreso(async () =>
        {
            var result = await _removeFromList(empleado.Id);

            result.ObjInteration = typeof(Empleado);
            await _dialogService.ValidarRespuesta(result);

            if (result.Success)
            {
                EmpleadosAsignados.Remove(empleado);
                NotificarCupo();
            }

            return result;
        }, DialogDefaults.Progress);
    }

    // ==============================
    // HELPERS
    // ==============================

    private bool EstaAsignado(Empleado empleado) =>
        EmpleadosAsignados.Any(e => e.Id == empleado.Id);

    private void NotificarCupo()
    {
        OnPropertyChanged(nameof(ResumenCupo));
        OnPropertyChanged(nameof(CupoAlcanzado));
        AsignarCommand.NotifyCanExecuteChanged();
    }

    // ==============================
    // SIGNALR — sincronización reactiva
    // ==============================

    private void OnEmpleadoCollectionChanged(
        EntityChangeType type, string id, Empleado? entity)
    {
        switch (type)
        {
            case EntityChangeType.Updated when entity is not null:
                // Actualizar en asignados si estaba
                var idx = EmpleadosAsignados
                    .Select((e, i) => (e, i))
                    .FirstOrDefault(x => x.e.Id == id).i;
                if (idx >= 0)
                    EmpleadosAsignados[idx] = entity;

                // Actualizar en resultados de búsqueda si aparece
                var idxR = ResultadosBusqueda
                    .Select((e, i) => (e, i))
                    .FirstOrDefault(x => x.e.Id == id).i;
                if (idxR >= 0)
                    ResultadosBusqueda[idxR] = entity;
                break;

            case EntityChangeType.Deleted:
                var asignado  = EmpleadosAsignados.FirstOrDefault(e => e.Id == id);
                var enBusqueda = ResultadosBusqueda.FirstOrDefault(e => e.Id == id);

                if (asignado is not null)
                {
                    EmpleadosAsignados.Remove(asignado);
                    NotificarCupo();
                }
                if (enBusqueda is not null)
                    ResultadosBusqueda.Remove(enBusqueda);
                break;
        }
    }

    protected override void UpdateChanged() { }
}