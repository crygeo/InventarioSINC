using System.Windows.Controls;
using Cliente.View.Items;
using Shared.Interfaces.Model.Obj;
using Utilidades.Attributes;
using Utilidades.Controls;

namespace Cliente.Obj.Model;

[AutoViewModel]
public class Grupo : ModelBase<IGrupo>, IGrupo
{
    private string _jefeId = string.Empty;
    private int _cupo = 0;
    private IList<string> _empleadoIds = new List<string>();
    private string _nombre = string.Empty;
    private string _seccionId = string.Empty;

    [Solicitar("Nombre", InputBoxType.Text, typeof(TextBox), MaxLength = 20, Requerido =  true)]
    public string Nombre
    {
        get => _nombre;
        set => SetProperty(ref _nombre, value);
    }

    public string SeccionId
    {
        get => _seccionId;
        set => SetProperty(ref _seccionId, value);
    }

    [Solicitar("Encargado", InputBoxType.None, typeof(EntitySelector), Requerido = true, EntityType = typeof(Empleado))]
    public string JefeId
    {
        get => _jefeId;
        set => SetProperty(ref _jefeId, value);
    }

    [Solicitar("Empleados maximos", InputBoxType.Number, typeof(TextBox), Requerido = true)]
    public int Cupo
    {
        get => _cupo;
        set =>  SetProperty(ref _cupo, value);
    }

    public IList<string> EmpleadoIds
    {
        get => _empleadoIds;
        set => SetProperty(ref _empleadoIds, value);
    }

}