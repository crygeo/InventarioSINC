using Cliente.Messages;
using Cliente.Obj.Model;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Utilidades.Mvvm;

namespace Cliente.ViewModel.Model.Detail;

/// <summary>
/// ViewModel de detalle de un Área.
/// Solo expone datos — sin comandos de negocio propios (delegados al padre).
/// </summary>
public class AreaDetailVM : ViewModelBase
{
    public Area Area { get; }
    public IReadOnlyList<TurnoDetailVM> Turnos { get; }
    public IRelayCommand<TurnoDetailVM> NavigateToTurnoCommand { get; }

    public AreaDetailVM(Area area, IEnumerable<TurnoDetailVM> turnos)
    {
        Area   = area;
        Turnos = turnos.ToList();

        NavigateToTurnoCommand = new RelayCommand<TurnoDetailVM>(
            t => WeakReferenceMessenger.Default.Send(new  NavigateToDetailMessage(t!)),
            t => t != null);
    }
    
    

    protected override void UpdateChanged() { }
}