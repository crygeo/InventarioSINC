using System.Windows;
using System.Windows.Controls;
using Cliente.Obj.Model;
using Cliente.ViewModel.Model;
using Utilidades.Mvvm;

namespace Cliente.View.Model;

public partial class PageSeccionV : UserControl
{
    public PageSeccionV()
    {
        InitializeComponent();
    }

    private void OnSeccionSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DataContext is PageSeccionVM vm && e.AddedItems.Count > 0)
            vm.EntitySelect = e.AddedItems[0] as EntityWrapper<Seccion>;
    }

    private void OnVerGruposClick(object sender, RoutedEventArgs e)
    {
        if (sender is not FrameworkElement fe) return;
        if (fe.Tag is not Seccion seccion) return;

        var areaVm = FindPageAreaVM();
        if (areaVm is null) return;

        _ = areaVm.OnSeccionSelectedAsync(seccion);
    }

    private PageAreaVM? FindPageAreaVM()
    {
        DependencyObject? current = this;
        while (current is not null)
        {
            current = System.Windows.Media.VisualTreeHelper.GetParent(current);
            if (current is FrameworkElement fe && fe.DataContext is PageAreaVM vm)
                return vm;
        }
        return null;
    }
}