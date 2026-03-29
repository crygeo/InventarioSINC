using Cliente.Obj.Model;
using Utilidades.Mvvm;

namespace Cliente.ViewModel.Model.Detail;

public class SeccionDetailVM : ViewModelBase
{
    public Seccion Seccion { get; }
    public Turno TurnoPadre { get; }
    public IReadOnlyList<GrupoDetailVM> Grupos { get; }

    public SeccionDetailVM(Seccion seccion, Turno turnoPadre, IEnumerable<GrupoDetailVM> grupos)
    {
        Seccion = seccion;
        TurnoPadre = turnoPadre;
        Grupos = grupos.ToList();
    }

    protected override void UpdateChanged() { }
}