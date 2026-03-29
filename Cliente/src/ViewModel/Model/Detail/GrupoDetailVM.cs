using System.Collections.ObjectModel;
using Cliente.Obj.Model;
using Utilidades.Mvvm;

namespace Cliente.ViewModel.Model.Detail;

public class GrupoDetailVM : ViewModelBase
{
    public Grupo Grupo { get; }
    public Seccion SeccionPadre { get; }
    public ObservableCollection<Empleado> Empleados { get; } = [];

    public GrupoDetailVM(Grupo grupo, Seccion seccionPadre)
    {
        Grupo = grupo;
        SeccionPadre = seccionPadre;
    }

    protected override void UpdateChanged() { }
}