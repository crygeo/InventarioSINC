using System.Windows.Controls;
using Cliente.Obj.Model;
using Cliente.ViewModel.Model;
using Utilidades.Mvvm;

namespace Cliente.View.Model;

/// <summary>
/// Code-behind mínimo para PageAreasV.
///
/// La única lógica aquí es el puente entre el evento SelectionChanged de WPF
/// (que no se puede bindear directamente a un Task) y el método async del VM.
/// Todo lo demás está en PageAreaVM.
/// </summary>
public partial class PageAreasV : UserControl
{
    public PageAreasV()
    {
        InitializeComponent();
    }

    private void OnAreaSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DataContext is not PageAreaVM vm) return;
        if (e.AddedItems.Count == 0) return;
        if (e.AddedItems[0] is not EntityWrapper<Area> area) return;

        // Lanzar la tarea async sin bloquear el hilo UI.
        // PageAreaVM garantiza que no se lanza si el área ya está activa.
        _ = vm.OnAreaSelectedAsync(area.Model);
    }
}