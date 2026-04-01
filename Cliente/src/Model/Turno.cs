using System.Windows.Controls;
using Cliente.View.Items;
using MaterialDesignThemes.Wpf;
using Shared.Interfaces.Model;
using Shared.Interfaces.Model.Obj;
using Utilidades.Attributes;
using Utilidades.Controls;

namespace Cliente.Obj.Model;

[AutoViewModel]
public class Turno : ModelBase<ITurno>, ITurno
{
    private string _id = string.Empty;
    private bool _deleteable = false;
    private bool _updatable = false;

    private string _nombre = string.Empty;
    private TimeSpan _horaInicio = TimeSpan.Zero;
    private TimeSpan _horaFinal = TimeSpan.Zero;
    private bool _esRotativo = false;
    private string _areaId = string.Empty;
    private string _idTurnoTrabajo;

    public string Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }


    public bool Deleteable
    {
        get => _deleteable;
        set => SetProperty(ref _deleteable, value);
    }

    public bool Updatable
    {
        get => _updatable;
        set => SetProperty(ref _updatable, value);
    }

    [Solicitar("Nombre", InputBoxType.Text, typeof(TextBox), Requerido = true, MaxLength = 20)]
    public string Nombre
    {
        get => _nombre;
        set => SetProperty(ref _nombre, value);
    }

    [Solicitar("Turno de Trabajo", InputBoxType.None, typeof(EntitySelector), Requerido = true, EntityType = typeof(TurnoTrabajo), VisibleWhen = nameof(EsRotativo), VisibleWhenValue = false)]
    [Vista(LookupType = typeof(TurnoTrabajo))]
    public string IdTurnoTrabajo
    {
        get => _idTurnoTrabajo;
        set => SetProperty(ref _idTurnoTrabajo, value);
    }
   

    [Solicitar("Turno Rotativo.", InputBoxType.None, typeof(CheckBox))]
    public bool EsRotativo
    {
        get => _esRotativo;
        set => SetProperty(ref _esRotativo, value);
    }

    public string AreaId
    {
        get => _areaId;
        set => SetProperty(ref  _areaId, value);
    }

    
}