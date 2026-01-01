using System.Windows.Controls;
using Cliente.Attributes;
using MaterialDesignThemes.Wpf;
using Shared.Interfaces.Model.Obj;
using Utilidades.Attributes;
using Utilidades.Controls;

namespace Cliente.Obj.Model;

[AutoViewModel]
[Navegacion("PanelConfig", TituloS = "Identificadores", SelectedIcon = PackIconKind.AlphaIBox,
    UnselectedIcon = PackIconKind.AlphaIBoxOutline)]
public class Identificador : ModelBase<IIdentificador>, IIdentificador
{
    private string _descripcion = string.Empty;
    private DateTime _fechaCreacion;
    private string _name = string.Empty;

    [Solicitar("Nombre", InputBoxType.Text,typeof(TextBox))]
    [Vista]
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    [Solicitar("Descripcion", InputBoxType.Text, typeof(TextBox))]
    [Vista]
    public string Descripcion
    {
        get => _descripcion;
        set => SetProperty(ref _descripcion, value);
    }

    public DateTime FechaCreacion
    {
        get => _fechaCreacion;
        set => SetProperty(ref _fechaCreacion, value);
    }


    protected override void UpdateChanged()
    {
    }
}