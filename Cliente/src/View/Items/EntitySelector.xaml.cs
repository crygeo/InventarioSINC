using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Cliente.View.Dialog;
using Cliente.View.Dialog.ViewModel;
using MaterialDesignThemes.Wpf;
using Shared.Interfaces;
using Utilidades.Dialogs;
using Utilidades.Mvvm;

namespace Cliente.View.Items;

public partial class EntitySelector : UserControl
{
    // =========================================================
    // Constructor
    // =========================================================

    public EntitySelector()
    {
        InitializeComponent();
        Loaded += OnLoaded;
        OpenDialogCommand = new RelayCommand(async _ => await OpenDialogAsync());
    }

    // =========================================================
    // Loader inicial (precarga caché para edición)
    // =========================================================

    public Func<Task<IEnumerable<object>>>? InitialLoadFunc { get; set; }
    private bool _isLoaded;

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (_isLoaded) return;
        _isLoaded = true;

        if (InitialLoadFunc == null) return;

        try
        {
            var items = await InitialLoadFunc();
            if (items != null) AddInitialItems(items);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[EntitySelector] Error cargando datos iniciales: {ex}");
        }
    }

    // =========================================================
    // Caché interna
    // =========================================================

    private readonly Dictionary<string, object> _cache = new();

    public void AddInitialItems(IEnumerable<object> items)
    {
        foreach (var item in items.OfType<IIdentifiable>())
            _cache.TryAdd(item.Id, item);
    }

    // =========================================================
    // Command
    // =========================================================

    public ICommand OpenDialogCommand { get; }

    // =========================================================
    // Dependency Properties
    // =========================================================

    #region HitText

    public static readonly DependencyProperty HitTextProperty =
        DependencyProperty.Register(
            nameof(HitText),
            typeof(string),
            typeof(EntitySelector));

    public string? HitText
    {
        get => (string?)GetValue(HitTextProperty);
        set => SetValue(HitTextProperty, value);
    }

    #endregion

    #region DisplayMemberPath

    public static readonly DependencyProperty DisplayMemberPathProperty =
        DependencyProperty.Register(
            nameof(DisplayMemberPath),
            typeof(string),
            typeof(EntitySelector));

    public string? DisplayMemberPath
    {
        get => (string?)GetValue(DisplayMemberPathProperty);
        set => SetValue(DisplayMemberPathProperty, value);
    }

    #endregion

    #region DisplayText (readonly — calculado internamente)

    private static readonly DependencyPropertyKey DisplayTextPropertyKey =
        DependencyProperty.RegisterReadOnly(
            nameof(DisplayText),
            typeof(string),
            typeof(EntitySelector),
            new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty DisplayTextProperty =
        DisplayTextPropertyKey.DependencyProperty;

    public string DisplayText
    {
        get => (string)GetValue(DisplayTextProperty);
        private set => SetValue(DisplayTextPropertyKey, value);
    }

    #endregion

    #region RemoteSearchFunc

    public static readonly DependencyProperty RemoteSearchFuncProperty =
        DependencyProperty.Register(
            nameof(RemoteSearchFunc),
            typeof(Func<string, Task<IEnumerable<object>>>),
            typeof(EntitySelector));

    public Func<string, Task<IEnumerable<object>>>? RemoteSearchFunc
    {
        get => (Func<string, Task<IEnumerable<object>>>?)GetValue(RemoteSearchFuncProperty);
        set => SetValue(RemoteSearchFuncProperty, value);
    }

    #endregion

    #region GetByIdFunc

    public static readonly DependencyProperty GetByIdFuncProperty =
        DependencyProperty.Register(
            nameof(GetByIdFunc),
            typeof(Func<string, Task<object?>>),
            typeof(EntitySelector));

    public Func<string, Task<object?>>? GetByIdFunc
    {
        get => (Func<string, Task<object?>>?)GetValue(GetByIdFuncProperty);
        set => SetValue(GetByIdFuncProperty, value);
    }

    #endregion

    #region SelectedItem

    public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.Register(
            nameof(SelectedItem),
            typeof(object),
            typeof(EntitySelector),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnSelectedItemChanged));

    public object? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    private static void OnSelectedItemChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e)
    {
        if (d is not EntitySelector control || control._isSynchronizing) return;
        if (e.NewValue is not IIdentifiable identifiable) return;
        if (control.SelectedId == identifiable.Id) return;

        try
        {
            control._isSynchronizing = true;
            control.SelectedId = identifiable.Id;
            control.DisplayText = control.ResolveDisplayText(e.NewValue);
        }
        finally
        {
            control._isSynchronizing = false;
        }
    }

    #endregion

    #region SelectedId

    public static readonly DependencyProperty SelectedIdProperty =
        DependencyProperty.Register(
            nameof(SelectedId),
            typeof(string),
            typeof(EntitySelector),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnSelectedIdChanged));

    public string? SelectedId
    {
        get => (string?)GetValue(SelectedIdProperty);
        set => SetValue(SelectedIdProperty, value);
    }

    private static void OnSelectedIdChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e)
    {
        if (d is not EntitySelector control || control._isSynchronizing) return;
        if (e.NewValue is not string id) return;

        // Buscar en caché local primero (O(1))
        if (control._cache.TryGetValue(id, out var cached))
        {
            control.SynchronizeSelectedItem(cached);
            return;
        }

        // Fallback: resolución remota (escenario edición sin caché)
        if (control.GetByIdFunc != null)
            _ = control.ResolveSelectedItemByIdAsync(id);
    }

    #endregion

    // =========================================================
    // Sincronización SelectedItem ↔ SelectedId
    // =========================================================

    private bool _isSynchronizing;

    private void SynchronizeSelectedItem(object item)
    {
        if (_isSynchronizing || SelectedItem == item) return;

        try
        {
            _isSynchronizing = true;
            SelectedItem = item;
            DisplayText = ResolveDisplayText(item);
        }
        finally
        {
            _isSynchronizing = false;
        }
    }

    private async Task ResolveSelectedItemByIdAsync(string id)
    {
        try
        {
            var item = await GetByIdFunc!(id);
            if (item is not IIdentifiable identifiable) return;

            _cache.TryAdd(identifiable.Id, item);

            // Validar que el Id sigue siendo el mismo tras el await
            if (SelectedId != id) return;

            SynchronizeSelectedItem(item);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[EntitySelector] Error resolviendo Id '{id}': {ex}");
        }
    }

    // =========================================================
    // Diálogo
    // =========================================================

    private async Task OpenDialogAsync()
    {
        if (RemoteSearchFunc == null) return;

        var vm = new EntitySelectorDialogViewModel
        {
            SearchFunc = RemoteSearchFunc
        };

        var dialog = new EntitySelectorDialog { DataContext = vm };

        await DialogService.Instance.MostrarDialogo(dialog);

        if (result is not IIdentifiable selected) return;

        _cache.TryAdd(selected.Id, selected);
        SynchronizeSelectedItem(selected);

        try
        {
            _isSynchronizing = true;
            SelectedId = selected.Id;
        }
        finally
        {
            _isSynchronizing = false;
        }
    }

    // =========================================================
    // Helpers
    // =========================================================

    private string ResolveDisplayText(object item)
    {
        if (string.IsNullOrWhiteSpace(DisplayMemberPath))
            return item.ToString() ?? string.Empty;

        var prop = item.GetType().GetProperty(DisplayMemberPath);
        return prop?.GetValue(item)?.ToString() ?? string.Empty;
    }
}