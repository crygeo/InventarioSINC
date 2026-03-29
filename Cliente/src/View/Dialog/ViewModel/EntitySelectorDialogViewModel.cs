using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Utilidades.Mvvm;

namespace Cliente.View.Dialog.ViewModel;

public class EntitySelectorDialogViewModel : INotifyPropertyChanged
{
    // =========================================================
    // Configuración externa (inyectada al construir)
    // =========================================================

    public required Func<string, Task<IEnumerable<object>>> SearchFunc { get; init; }

    // Opcional: si se provee, carga items al abrir el diálogo
    public Func<Task<IEnumerable<object>>>? DefaultLoadFunc { get; init; }
    // =========================================================
    // Estado
    // =========================================================

    private string _searchText = string.Empty;
    public string SearchText
    {
        get => _searchText;
        set { _searchText = value; OnPropertyChanged(); }
    }

    private object? _selectedItem;
    public object? SelectedItem
    {
        get => _selectedItem;
        set { _selectedItem = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanConfirm)); }
    }

    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set { _isLoading = value; OnPropertyChanged(); }
    }

    public bool CanConfirm => SelectedItem != null;

    public ObservableCollection<object> Results { get; } = [];

    // Tipo de entidad — usado para generar columnas [Vista]
    public required Type EntityType { get; init; }

    // =========================================================
    // Commands
    // =========================================================

    public ICommand SearchCommand { get; }

    public EntitySelectorDialogViewModel()
    {
        SearchCommand = new RelayCommand(async _ => await ExecuteSearchAsync());
    }

    // =========================================================
    // Carga inicial
    // =========================================================

    public async Task LoadDataDefaultAsync()
    {
        if (DefaultLoadFunc == null) return;

        try
        {
            IsLoading = true;
            Results.Clear();

            var items = await DefaultLoadFunc();

            foreach (var item in items)
                Results.Add(item);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[EntitySelectorDialog] Error en carga inicial: {ex}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    // =========================================================
    // Lógica de búsqueda
    // =========================================================

    private CancellationTokenSource? _cts;

    private async Task ExecuteSearchAsync()
    {
        // Si no hay texto, restaurar los resultados iniciales sin ir a red
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            await LoadDataDefaultAsync();
            return;
        }

        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        var token = _cts.Token;

        try
        {
            IsLoading = true;
            Results.Clear();
            SelectedItem = null;

            var items = await SearchFunc(SearchText);

            if (token.IsCancellationRequested) return;

            foreach (var item in items)
                Results.Add(item);
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            Console.WriteLine($"[EntitySelectorDialog] Error en búsqueda: {ex}");
        }
        finally
        {
            if (!token.IsCancellationRequested)
                IsLoading = false;
        }
    }

    // =========================================================
    // INotifyPropertyChanged
    // =========================================================

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}