using Cliente.Obj.Model;
using Utilidades.Mvvm;

namespace Cliente.ViewModel.Model.Detail;

public class TurnoDetailVM : ViewModelBase
{
    public Turno Turno { get; }
    public Area AreaPadre { get; }
    public IReadOnlyList<SeccionDetailVM> Secciones { get; }

    public TurnoDetailVM(Turno turno, Area areaPadre, IEnumerable<SeccionDetailVM> secciones)
    {
        Turno = turno;
        AreaPadre = areaPadre;
        Secciones = secciones.ToList();
    }

    protected override void UpdateChanged() { }
}