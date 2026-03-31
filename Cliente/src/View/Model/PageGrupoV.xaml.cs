using System.Windows;
using System.Windows.Controls;
using Cliente.Obj.Model;
using Cliente.ViewModel.Model;

namespace Cliente.View.Model;

public partial class PageGrupoV : UserControl
{
    public PageGrupoV()
    {
        InitializeComponent();
    }

    private void OnVerEmpleadosClick(object sender, RoutedEventArgs e)
    {
        if (sender is not FrameworkElement fe) return;
        if (fe.Tag is not Grupo grupo) return;
        FindPageAreaVM()?.OnGrupoSelectedAsync(grupo);
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