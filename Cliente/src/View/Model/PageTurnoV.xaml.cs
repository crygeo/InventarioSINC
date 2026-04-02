using System.Windows;
using System.Windows.Controls;
using Cliente.Obj.Model;
using Cliente.ViewModel.Model;
using Utilidades.Mvvm;

namespace Cliente.View.Model;

/// <summary>
/// Code-behind de PageTurnoV.
///
/// Conecta dos eventos WPF que no pueden bindearse directamente a métodos async:
/// - SelectionChanged → establece EntitySelect en PageTurnoVM
/// - Click del botón ChevronRight → navega al nivel de secciones vía PageAreaVM
///
/// PageAreaVM es el orquestador: se obtiene subiendo por el árbol visual.
/// PageTurnoVM (DataContext) no conoce a PageAreaVM — bajo acoplamiento mantenido.
/// </summary>
public partial class PageTurnoV : UserControl
{
    public PageTurnoV()
    {
        InitializeComponent();
    }

    private void OnTurnoSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Solo actualiza EntitySelect — la navegación se dispara
        // únicamente al hacer clic en ChevronRight, no al seleccionar.
        if (DataContext is PageTurnoVM vm && e.AddedItems.Count > 0)
            vm.EntitySelect = e.AddedItems[0] as EntityWrapper<Turno>;
    }

    /// <summary>
    /// El botón ChevronRight usa Tag="{Binding}" para pasar el Turno.
    /// Se escala por el árbol visual para encontrar PageAreaVM
    /// sin crear acoplamiento directo entre PageTurnoV y PageAreaVM.
    /// </summary>
    private void OnVerSeccionesClick(object sender, RoutedEventArgs e)
    {
        if (sender is not FrameworkElement fe) return;
        if (fe.Tag is not Turno turno) return;

        var areaVm = FindPageAreaVM();
        if (areaVm is null) return;

        _ = areaVm.OnTurnoSelectedAsync(turno);
    }

    private PageAreaVM? FindPageAreaVM()
    {
        // Subir por el árbol de DataContext buscando PageAreaVM.
        // PageTurnoV está dentro del ContentControl del NavigationStack,
        // cuyo DataContext es PageTurnoVM, pero su padre tiene PageAreaVM.
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