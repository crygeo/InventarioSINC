using System.Windows.Controls;
using Cliente.Attributes;
using Cliente.View.Dialog;
using MaterialDesignThemes.Wpf;
using Shared.Interfaces.Model.Obj;
using Utilidades.Attributes;
using Utilidades.Controls;

namespace Cliente.Obj.Model;

[AutoViewModel]
[Navegacion("PanelConfig", TituloS = "Proveedor", SelectedIcon = PackIconKind.Domain,
    UnselectedIcon = PackIconKind.DomainOff, DialogoPersonalizado = typeof(ProveedorDialog))]
public class Proveedor : ModelBase<IProveedor>, IProveedor
{
    private string _cedula = string.Empty;
    private string _celular = string.Empty;

    private string _direccion = string.Empty;

    private DateTime _fechaNacimiento = DateTime.Now;

    // Tipo de proveedor
    private bool _isPersona;

    private string _primerApellido = string.Empty;

    // Persona
    private string _primerNombre = string.Empty;

    // Empresa
    private string _razonSocial = string.Empty;
    private string _representanteLegal = string.Empty;

    //Proveedor
    private string _ruc = string.Empty;
    private string _segundoApellido = string.Empty;
    private string _segundoNombre = string.Empty;

    [Vista("Nombre")] public string DisplayName => EsEmpresa ? RazonSocial : $"{PrimerNombre} {PrimerApellido}";

    public bool EsEmpresa
    {
        get => _isPersona;
        set => SetProperty(ref _isPersona, value);
    }

    [Solicitar("Primer Nombre", InputBoxType.Name, typeof(TextBox), Requerido = true)]
    public string PrimerNombre
    {
        get => _primerNombre;
        set => SetProperty(ref _primerNombre, value);
    }

    [Solicitar("Segundo Nombre", InputBoxType.Name, typeof(TextBox))]
    public string SegundoNombre
    {
        get => _segundoNombre;
        set => SetProperty(ref _segundoNombre, value);
    }

    [Solicitar("Primer Apellido", InputBoxType.Name, typeof(TextBox), Requerido = true)]
    public string PrimerApellido
    {
        get => _primerApellido;
        set => SetProperty(ref _primerApellido, value);
    }

    [Solicitar("Segundo Apellido", InputBoxType.Name, typeof(TextBox))]
    public string SegundoApellido
    {
        get => _segundoApellido;
        set => SetProperty(ref _segundoApellido, value);
    }

    [Solicitar("Cédula", InputBoxType.Dni, typeof(TextBox), Requerido = true)]
    public string Cedula
    {
        get => _cedula;
        set => SetProperty(ref _cedula, value);
    }

    [Solicitar("RUC", InputBoxType.Ruc, typeof(TextBox), Requerido = true)]
    [Vista(Orden = 1)]
    public string RUC
    {
        get => _ruc;
        set => SetProperty(ref _ruc, value);
    }

    [Solicitar("Dirección", InputBoxType.Direccion, typeof(TextBox))]
    public string Direccion
    {
        get => _direccion;
        set => SetProperty(ref _direccion, value);
    }

    [Solicitar("Razón Social", InputBoxType.Text, typeof(TextBox), Requerido = true)]
    public string RazonSocial
    {
        get => _razonSocial;
        set => SetProperty(ref _razonSocial, value);
    }

    [Solicitar("Representante Legal", InputBoxType.Text,typeof(TextBox), Requerido = true)]
    public string RepresentanteLegal
    {
        get => _representanteLegal;
        set => SetProperty(ref _representanteLegal, value);
    }

    [Solicitar("Celular", InputBoxType.Phone,typeof(TextBox), Requerido = true)]
    public string Celular
    {
        get => _celular;
        set => SetProperty(ref _celular, value);
    }

    [Solicitar("Fecha de Nacimiento", InputBoxType.Date, typeof(DatePicker), Requerido = true)]
    public DateTime FechaNacimiento
    {
        get => _fechaNacimiento;
        set => SetProperty(ref _fechaNacimiento, value);
    }

    public string NombreCompleto =>
        EsEmpresa ? RazonSocial : $"{PrimerNombre} {SegundoNombre} {PrimerApellido} {SegundoApellido}".Trim();


    protected override void UpdateChanged()
    {
        // Acá podrías activar lógica condicional si cambia el tipo
    }

    public override string ToString()
    {
        return DisplayName;
    }
}