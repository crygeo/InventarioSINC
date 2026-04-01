using System.Windows.Controls;
using Shared.Interfaces.Model;
using Shared.Interfaces.Model.Obj;
using Utilidades.Attributes;
using Utilidades.Controls;

namespace Cliente.Obj.Model;

[AutoViewModel]
public class Seccion : ModelBase<ISeccion>, ISeccion
{
    private string _nombre = string.Empty;
    private int _cupo = 0; 
    private IList<string> _empleadoIds = new  List<string>();
    private bool _esGrupo = false;
    private string _turnoId = string.Empty;


    [Solicitar("Nombre", InputBoxType.Text, typeof(TextBox), MaxLength = 20, Requerido = true)]
    public string Nombre
    {
        get => _nombre;
        set => SetProperty(ref _nombre, value);
    }
    [Solicitar("Seccion de Grupos", InputBoxType.None, typeof(CheckBox), Requerido = true)]
    public bool EsGrupo
    {
        get => _esGrupo;
        set => SetProperty(ref _esGrupo, value);
    }

    public string TurnoId
    {
        get => _turnoId;
        set => SetProperty(ref  _turnoId, value);
    }

    [Solicitar(
        "Cantidad max empleados",
        InputBoxType.Number, 
        typeof(TextBox), 
        Requerido = true,
        VisibleWhen = nameof(EsGrupo),
        VisibleWhenValue = false)]
    public int Cupo
    {
        get => _cupo;
        set => SetProperty(ref _cupo, value);
    }

    public IList<string> EmpleadoIds
    {
        get => _empleadoIds;
        set => SetProperty(ref _empleadoIds, value);
    }
    
    public int EmpleadosCount => EmpleadoIds.Count;

    
}