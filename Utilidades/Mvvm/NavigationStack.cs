// Cliente/src/ViewModel/NavigationStack.cs

using System.Collections.Generic;
using cv = CommunityToolkit.Mvvm.Input;
using Utilidades.Interfaces;

namespace Utilidades.Mvvm;

/// <summary>
/// Pila de navegación intra-panel. Thread-safe vía Dispatcher no requerido
/// porque solo se usa desde el hilo UI (comandos WPF).
/// </summary>
public class NavigationStack : NotifyProperty, INavigationStack
{
    private readonly Stack<ViewModelBase> _stack = new();

    public ViewModelBase? Current => _stack.Count > 0 ? _stack.Peek() : null;

    public bool CanGoBack => _stack.Count > 1;

    public cv.IRelayCommand GoBackCommand { get; }

    public NavigationStack()
    {
        GoBackCommand = new cv.RelayCommand(Pop, () => CanGoBack);
    }

    public void Push(ViewModelBase vm)
    {
        _stack.Push(vm);
        NotifyAll();
    }

    public void Pop()
    {
        if (_stack.Count <= 1) return;
        _stack.Pop();
        NotifyAll();
    }

    public void Reset(ViewModelBase vm)
    {
        _stack.Clear();
        _stack.Push(vm);
        NotifyAll();
    }

    private void NotifyAll()
    {
        OnPropertyChanged(nameof(Current));
        OnPropertyChanged(nameof(CanGoBack));
        GoBackCommand.NotifyCanExecuteChanged();
    }

    protected override void UpdateChanged() { }
}