using Cliente.Helpers;
using Cliente.Obj;
using Utilidades.Mvvm;

namespace Cliente.ViewModel;

public class PageProcesosVM : ViewModelBase
{
    private const string Key = "PanelConfig";

    private ViewModelBase _pageSelectViewModel = null!;

    private ItemNavigationM _selectedItemNav = null!;

    public ViewModelBase PageSelectViewModel
    {
        get => _pageSelectViewModel;
        set => SetProperty(ref _pageSelectViewModel, value);
    }

    public ItemNavigationM SelectedItemNav
    {
        get => _selectedItemNav;
        set
        {
            if (SetProperty(ref _selectedItemNav, value)) PageSelectViewModel = value.Page;
        }
    }

    public List<ItemNavigationM> ListItemsNav { get; } = NavegacionFactory.ObtenerTodosPorIndicador(Key);


    protected override void UpdateChanged()
    {
    }
}