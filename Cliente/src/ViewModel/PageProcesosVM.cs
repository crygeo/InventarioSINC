using Cliente.Helpers;
using Cliente.Obj.Model;
using MaterialDesignThemes.Wpf;
using Utilidades.Mvvm;

namespace Cliente.ViewModel;

public class PageProcesosVM : ViewModelBase
{

    private ViewModelBase _pageSelectViewModel = null!;
    public ViewModelBase PageSelectViewModel
    {
        get => _pageSelectViewModel;
        set => SetProperty(ref _pageSelectViewModel, value);
    }

    private ItemNavigationM _selectedItemNav = null!;
    public ItemNavigationM SelectedItemNav
    {
        get => _selectedItemNav;
        set
        {
            if (SetProperty(ref _selectedItemNav, value))
            {
                PageSelectViewModel = value.Page;
            }
        }
    }

    public List<ItemNavigationM> ListItemsNav { get; } = NavegacionFactory.CrearTodos();


    protected override void UpdateChanged()
    {
    }
}