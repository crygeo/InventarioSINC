using System.Windows.Controls;
using Cliente.Attributes;
using MaterialDesignThemes.Wpf;
using Shared.Interfaces.Model.Obj;
using Utilidades.Attributes;
using Utilidades.Controls;

namespace Cliente.Obj.Model;

[AutoViewModel]
[Navegacion("PanelConfig", TituloS = "Usuarios", SelectedIcon = PackIconKind.AlphaUBox,
    UnselectedIcon = PackIconKind.AlphaUBoxOutline)]
public class Usuario : ModelBase<IUsuario>, IUsuario
{
    private string _cedula = "";
    private string _celular = "";
    private DateTime _fechaNacimiento = DateTime.Now;
    private string _password = "";
    private string _primerApellido = "";
    private string _primerNombre = "";
    private List<string> _roles = [];
    private string _segundoApellido = "";
    private string _segundoNombre = "";
    private string _user = "";

    [Vista]
    public string NombreAndApellido
    {
        get => $"{PrimerNombre} {PrimerApellido}";
        set { }
    }


    [Solicitar("Usuario", InputBoxType.NickName, typeof(TextBox), Requerido = true)]
    [Vista]
    public string User
    {
        get => _user;
        set => SetProperty(ref _user, value);
    }

    [Solicitar("Primer Nombre", InputBoxType.Name, typeof(TextBox), Requerido = true)]
    public string PrimerNombre
    {
        get => _primerNombre;
        set => SetProperty(ref _primerNombre, value);
    }

    [Solicitar("Segundo Nombre", InputBoxType.Name, typeof(TextBox), Requerido = false)]
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

    [Solicitar("Segundo Apellido", InputBoxType.Name, typeof(TextBox), Requerido = false)]
    public string SegundoApellido
    {
        get => _segundoApellido;
        set => SetProperty(ref _segundoApellido, value);
    }

    [Solicitar("Cedula", InputBoxType.Dni, typeof(TextBox), Requerido = true)]
    [Vista]
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



    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public List<string> Roles
    {
        get => _roles;
        set => SetProperty(ref _roles, value);
    }

    public string NombreCompleto => $"{PrimerNombre} {SegundoNombre} {PrimerApellido} {SegundoApellido}";


    protected override void UpdateChanged()
    {
    }
}