using Cliente.Attributes;
using Cliente.View.Dialog;
using Cliente.View.Model;
using MaterialDesignThemes.Wpf;
using Shared.Interfaces.Model.Obj;
using Utilidades.Attributes;
using Utilidades.Interfaces;

namespace Cliente.Obj.Model;

[AutoViewModel(typeof(PageRolesV))]
[Navegacion("PanelConfig", TituloS = "Roles", SelectedIcon = PackIconKind.AlphaRBox,
    UnselectedIcon = PackIconKind.AlphaRBoxOutline, DialogoPersonalizado = typeof(RolDialog))]
public class Rol : ModelBase<IRol>, IRol, IClassified
{
    public bool _isAdmin;
    private bool _isClassified;
    private string _message = string.Empty;
    private string _nombre = "";
    private List<string> _permisos = [];

    public bool IsClassified
    {
        get => _isClassified;
        set => SetProperty(ref _isClassified, value);
    }

    public string Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }


    public string Nombre
    {
        get => _nombre;
        set => SetProperty(ref _nombre, value);
    }

    public List<string> Permisos
    {
        get => _permisos;
        set => SetProperty(ref _permisos, value);
    }

    public bool IsAdmin
    {
        get => _isAdmin;
        set => SetProperty(ref _isAdmin, value);
    }


    protected override void UpdateChanged()
    {
    }
}