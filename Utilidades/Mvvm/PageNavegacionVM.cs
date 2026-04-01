// Utilidades/Mvvm/PageNavegacionVM.cs

using System.Collections.Generic;
using System.Threading.Tasks;
using Utilidades.Interfaces;
using Utilidades.Mvvm;
using Utilidades.Objs;

namespace Utilidades.Mvvm;

public abstract class PageNavegacionVM : ViewModelBase, IBarNavegacion
{
    private ViewModelBase _pageSelectViewModel = null!;
    private IItemNav _selectedItemNav = null!;
    private ViewModelBase? _currentViewModel;

    public ViewModelBase PageSelectViewModel
    {
        get => _pageSelectViewModel;
        set => SetProperty(ref _pageSelectViewModel, value);
    }

    public IItemNav SelectedItemNav
    {
        get => _selectedItemNav;
        set
        {
            if (SetProperty(ref _selectedItemNav, value))
                _ = OnNavigationSelectedAsync(value);
        }
    }

    // Expuesto para que la vista pueda bindear el ItemsSource
    public List<IItemNav> ListItemsNav { get; }

    protected PageNavegacionVM(List<IItemNav> items)
    {
        ListItemsNav = items;
        if (items.Count > 0)
            SelectedItemNav = items[0];
    }

    private async Task OnNavigationSelectedAsync(IItemNav item)
    {
        if (_currentViewModel is IDeactivable deactivable)
            await deactivable.DeactivateAsync();

        if (item.Page is IActivable activable)
            await activable.ActivateAsync();

        _currentViewModel = item.Page;
        PageSelectViewModel = item.Page;
    }

    protected override void UpdateChanged() { }
}