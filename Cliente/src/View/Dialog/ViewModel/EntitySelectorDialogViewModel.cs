
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Shared.Interfaces;
using Utilidades.Command;
using Utilidades.Mvvm;

namespace Cliente.View.Dialog.ViewModel;

public class EntitySelectorDialogViewModel : INotifyPropertyChanged
{
    // =========================================================
    // Configuración externa (se inyecta al abrir)
    // =========================================================

    public required Func<string, Task<IEnumerable<object>>> SearchFunc { get; init; }

    // =========================================================
    // Estado
    // =========================================================

    private string _searchText = string.Empty;
    public string SearchText
    {
        get => _searchText;
        set
        {
            if (_searchText == value) return;
            _searchText = value;
            OnPropertyChanged();
        }
    }

    private object? _selectedItem;
    public object? SelectedItem
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanConfirm));
        }
    }

    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set { _isLoading = value; OnPropertyChanged(); }
    }

    public bool CanConfirm => SelectedItem != null;

    public ObservableCollection<object> Results { get; } = [];

    // =========================================================
    // Commands
    // =========================================================

    public ICommand SearchCommand { get; }

    public EntitySelectorDialogViewModel()
    {
        SearchCommand = new RelayCommand(async _ => await ExecuteSearchAsync());
    }

    // =========================================================
    // Lógica
    // =========================================================

    private CancellationTokenSource? _cts;

    private async Task ExecuteSearchAsync()
    {
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
            Console.WriteLine($"Error en búsqueda del diálogo: {ex}");
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