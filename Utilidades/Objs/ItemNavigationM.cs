using MaterialDesignThemes.Wpf;
using Utilidades.Interfaces;
using Utilidades.Mvvm;

namespace Utilidades.Objs;

public class ItemNavigationM : NotifyProperty, IItemNav
{
    private object? _notification;
    public required string Title { get; set; }
    public PackIconKind SelectedIcon { get; set; }
    public PackIconKind UnselectedIcon { get; set; }

    public required ViewModelBase Page { get; set; }


    public object? Notification
    {
        get => _notification;
        set => SetProperty(ref _notification, value);
    }

    public override string ToString()
    {
        return Title;
    }

    protected override void UpdateChanged()
    {
    }
}