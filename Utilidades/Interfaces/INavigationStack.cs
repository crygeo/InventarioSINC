using Utilidades.Mvvm;

namespace Utilidades.Interfaces;

public interface INavigationStack
{
    bool CanGoBack { get; }
    void Push(ViewModelBase vm);
    void Pop();
    void Reset(ViewModelBase vm);
}