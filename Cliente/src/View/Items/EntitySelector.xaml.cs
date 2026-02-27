using System.Collections;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Cliente.Obj.Model;
using Shared.Interfaces;
using Utilidades.Attributes;

namespace Cliente.View.Items;

public partial class EntitySelector : UserControl
{
    
    public Func<Task<IEnumerable<Empleado>>>? InitialLoadFunc { get; set; }
    public EntitySelector()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (InitialLoadFunc == null) return;

        var resultados = await InitialLoadFunc();
        if (resultados == null) return;

        var lista = resultados.ToList();
        _listaCompleta = lista;
        View = new ObservableCollection<Empleado>(lista);
    }
    // ================================
    // HitText
    // ================================

    public static readonly DependencyProperty HitTextProperty =
        DependencyProperty.Register(
            nameof(HitText),
            typeof(string),
            typeof(EntitySelector),
            new PropertyMetadata(string.Empty)
        );

    public string? HitText
    {
        get => (string?)GetValue(HitTextProperty);
        set => SetValue(HitTextProperty, value);
    }

    // ================================
    // DisplayMemberPath (opcional, cae a ToString() si no se asigna)
    // ================================

    public static readonly DependencyProperty DisplayMemberPathProperty =
        DependencyProperty.Register(
            nameof(DisplayMemberPath),
            typeof(string),
            typeof(EntitySelector),
            new PropertyMetadata(null)
        );

    public string? DisplayMemberPath
    {
        get => (string?)GetValue(DisplayMemberPathProperty);
        set => SetValue(DisplayMemberPathProperty, value);
    }

    // ================================
    // RemoteSearchFunc
    // ================================

    public static readonly DependencyProperty RemoteSearchFuncProperty =
        DependencyProperty.Register(
            nameof(RemoteSearchFunc),
            typeof(Func<string, Task<IEnumerable<Empleado>>>),
            typeof(EntitySelector),
            new PropertyMetadata(null)
        );

    public Func<string, Task<IEnumerable<Empleado>>>? RemoteSearchFunc
    {
        get => (Func<string, Task<IEnumerable<Empleado>>>?)GetValue(RemoteSearchFuncProperty);
        set => SetValue(RemoteSearchFuncProperty, value);
    }

    // ================================
    // ItemsSource
    // ================================

    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(
            nameof(ItemsSource),
            typeof(IEnumerable),
            typeof(EntitySelector),
            new PropertyMetadata(null, OnItemsSourceChanged)
        );

    public IEnumerable? ItemsSource
    {
        get => (IEnumerable?)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    private List<Empleado> _listaCompleta = [];

    private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (EntitySelector)d;
        if (e.NewValue == null) return;

        var lista = ((IEnumerable<Empleado>)e.NewValue);
        control._listaCompleta = lista.ToList();
        control.View = new ObservableCollection<Empleado>(lista);
    }

    // ================================
    // View (lo que ve el ComboBox)
    // ================================

    public static readonly DependencyProperty ViewProperty =
        DependencyProperty.Register(
            nameof(View),
            typeof(IEnumerable<Empleado>),
            typeof(EntitySelector)
        );

    public IEnumerable<Empleado>? View
    {
        get => (IEnumerable<Empleado>?)GetValue(ViewProperty);
        private set => SetValue(ViewProperty, value);
    }

    // ================================
    // SelectedItem
    // ================================

    public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.Register(
            nameof(SelectedItem),
            typeof(object),
            typeof(EntitySelector),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnSelectedItemChanged
            )
        );

    private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (EntitySelector)d;
        if (e.NewValue is not IIdentifiable identifiable) return;

        // Sincronizar SelectedId sin disparar OnSelectedIdChanged en loop
        if (control.SelectedId != identifiable.Id)
            control.SelectedId = identifiable.Id;
    }

    public object? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    // ================================
    // SelectedId
    // ================================

    public static readonly DependencyProperty SelectedIdProperty =
        DependencyProperty.Register(
            nameof(SelectedId),
            typeof(string),
            typeof(EntitySelector),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnSelectedIdChanged
            )
        );

    public string? SelectedId
    {
        get => (string?)GetValue(SelectedIdProperty);
        set => SetValue(SelectedIdProperty, value);
    }

    private static void OnSelectedIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (EntitySelector)d;
        if (control.View == null || e.NewValue is not string id) return;

        control.SelectedItem = control.View
            .OfType<IIdentifiable>()
            .FirstOrDefault(x => x.Id == id);
    }

    // ================================
    // OnFilterTextChanged — llamado por el Behavior
    // ================================

    public Action<string> OnFilterTextChanged => texto =>
    {
        if (RemoteSearchFunc != null)
            EjecutarBusquedaRemota(texto);
        else
            FiltrarLocal(texto);
    };

    // ================================
    // Filtro local
    // ================================

    private void FiltrarLocal(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
        {
            View = new ObservableCollection<Empleado>(_listaCompleta);
            return;
        }

        var filtrados = _listaCompleta.Where(obj =>
        {
            var propsBuscables = obj.GetType()
                .GetProperties()
                .Where(p => p.GetCustomAttribute<BuscableAttribute>() != null)
                .ToList();

            var textoObj = propsBuscables.Any()
                ? string.Join(" ", propsBuscables.Select(p => p.GetValue(obj)?.ToString() ?? string.Empty))
                : obj.ToString() ?? string.Empty;

            return textoObj.Contains(texto, StringComparison.OrdinalIgnoreCase);
        });

        View = new ObservableCollection<Empleado>(filtrados);
    }

    // ================================
    // Búsqueda remota
    // ================================

    private CancellationTokenSource? _debounceToken;

    private void EjecutarBusquedaRemota(string texto)
    {
        _debounceToken?.Cancel();
        _debounceToken = new CancellationTokenSource();
        var token = _debounceToken.Token;

        Task.Delay(350, token).ContinueWith(async t =>
        {
            if (t.IsCanceled) return;

            await Dispatcher.InvokeAsync(async () =>
            {
                try
                {
                    var resultados = string.IsNullOrWhiteSpace(texto)
                        ? _listaCompleta.AsEnumerable()
                        : await RemoteSearchFunc!(texto);

                    if (resultados == null) return;

                    View = new ObservableCollection<Empleado>(resultados);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en búsqueda remota: {ex}");
                }
            });
        }, TaskScheduler.Default);
    }
    
    
}