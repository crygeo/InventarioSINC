using System.Diagnostics;
using Cliente.Helpers;
using Cliente.Obj;
using Shared.Interfaces.Model;
using Utilidades.Interfaces;
using Utilidades.Mvvm;

namespace Cliente.ViewModel;

public class PageProcesosVm : ViewModelBase
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
            if (SetProperty(ref _selectedItemNav, value))
            {
                _ = OnNavigationSelectedAsync(value);
            }
        }
    }

    private ViewModelBase? _currentViewModel;

    private async Task OnNavigationSelectedAsync(ItemNavigationM item)
    {
        // 1️⃣ Desactivar el actual
        if (_currentViewModel is IDeactivable deactivable)
        {
            await deactivable.DeactivateAsync();
        }
        

        // 2️⃣ Activar el nuevo
        if (item.Page is IActivable activable)
        {
            await activable.ActivateAsync();
        }
       

        // 3️⃣ Asignar
        _currentViewModel = item.Page;
        PageSelectViewModel = item.Page;
       

    }


    public List<ItemNavigationM> ListItemsNav { get; } = NavegacionFactory.ObtenerTodosPorIndicador(Key);


    protected override void UpdateChanged()
    {
    }
}