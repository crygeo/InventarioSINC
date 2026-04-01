using System.Windows.Controls;
using Cliente.Attributes;
using MaterialDesignThemes.Wpf;
using Shared.Interfaces.Model.Obj;
using Utilidades.Attributes;
using Utilidades.Controls;

namespace Cliente.Obj.Model;

[AutoViewModel]
[Navegacion("PageGestionEmployer", TituloS = "Empleados", SelectedIcon = PackIconKind.AlphaIBox,
    UnselectedIcon = PackIconKind.AlphaIBoxOutline)]
public class Empleado : ModelBase<IEmpleado>, IEmpleado
{
    private string _cedula = string.Empty;
    private string _celular = string.Empty;
    private bool _estaActivo;

    private string _estadoActualNombre = string.Empty;

    // IEmpleado
    private string _estadoEmpleadoId = string.Empty;
    private DateTime _fechaIngreso = DateTime.Now;
    private DateTime _fechaNacimiento = DateTime.Now;

    // IPersona
    private string _primerNombre = string.Empty;
    private string? _segundoNombre = string.Empty;
    private string? _segundoApellido = string.Empty;
    private string _primerApellido = string.Empty;

    // Implementación de IEmpleado
    public string EstadoEmpleadoId
    {
        get => _estadoEmpleadoId;
        set => SetProperty(ref _estadoEmpleadoId, value);
    }

    public string EstadoActualNombre
    {
        get => _estadoActualNombre;
        set => SetProperty(ref _estadoActualNombre, value);
    }


    public bool EstaActivo
    {
        get => _estaActivo;
        set => SetProperty(ref _estaActivo, value);
    }

    // Implementación de IPersona
    [Solicitar("Primer Nombre", InputBoxType.Text, typeof(TextBox), Requerido = true)]
    [Buscable]
    public string PrimerNombre
    {
        get => _primerNombre;
        set => SetProperty(ref _primerNombre, value);

    }

    [Solicitar("Segundo Nombre", InputBoxType.Text, typeof(TextBox))]
    [Buscable]
    public string? SegundoNombre    
    {
        get => _segundoNombre;
        set => SetProperty(ref _segundoNombre, value);
    }

    [Solicitar("Primer Apellido", InputBoxType.Text, typeof(TextBox), Requerido = true)]
    [Buscable]
    public string PrimerApellido
    {
        get => _primerApellido;
        set => SetProperty(ref _primerApellido, value);
    }

    [Solicitar("Segundo Apellido", InputBoxType.Text, typeof(TextBox))]
    [Buscable]
    public string? SegundoApellido
    {
        get => _segundoApellido;
        set => SetProperty(ref _segundoApellido, value);
    }

    [Solicitar("Cedula", InputBoxType.Dni, typeof(TextBox), Requerido = true)]
    [Vista(Nombre = "Cedula", Orden = 1)]
    [Buscable]
    public string Cedula
    {
        get => _cedula;
        set => SetProperty(ref _cedula, value);
    }

    [Solicitar("Celular", InputBoxType.Phone, typeof(TextBox), Requerido = true)]
    public string Celular
    {
        get => _celular;
        set => SetProperty(ref _celular, value);
    }

    [Solicitar("Fecha de Nacimiento", InputBoxType.Date, typeof(DatePicker))]
    public DateTime FechaNacimiento
    {
        get => _fechaNacimiento;
        set => SetProperty(ref _fechaNacimiento, value);
    }

    [Solicitar("Fecha de Ingreso", InputBoxType.Date, typeof(DatePicker))]
    public DateTime FechaIngreso
    {
        get => _fechaIngreso;
        set => SetProperty(ref _fechaIngreso, value);
    }

    [IgnoreChangeTracking]
    [Vista(Nombre = "Nombre", Orden = 0)]
    [DependsOn(nameof(PrimerNombre))]
    [DependsOn(nameof(SegundoNombre))]
    [DependsOn(nameof(PrimerApellido))]
    [DependsOn(nameof(SegundoApellido))]
    public string NombreCompleto => $"{PrimerNombre} {SegundoNombre} {PrimerApellido} {SegundoApellido}";
    
    [IgnoreChangeTracking]
    [DependsOn(nameof(NombreCompleto))]
    [DependsOn(nameof(Cedula))]
    public string DescripcionVisual => $"{NombreCompleto} - {Cedula}";

    public override string ToString()
    {
        return $"{DescripcionVisual}";
    }

    protected override void UpdateChanged()
    {
    }
}