using MaterialDesignThemes.Wpf;
using Shared.Interfaces.Model;
using Utilidades.Mvvm;

namespace Utilidades.Interfaces;

public interface IItemNav
{
    string Title { get; set; }
    PackIconKind SelectedIcon { get; set; }
    PackIconKind UnselectedIcon { get; set; }
    ViewModelBase Page { get; set; }
    public object? Notification { get; set; }
}