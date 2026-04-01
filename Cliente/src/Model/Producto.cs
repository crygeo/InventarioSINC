using System.Windows.Controls;
using Cliente.Attributes;
using MaterialDesignThemes.Wpf;
using Shared.Interfaces.ModelsBase;
using Utilidades.Attributes;
using Utilidades.Controls;

namespace Cliente.Obj.Model;


[AutoViewModel]
[Navegacion("PanelConfig", TituloS = "Producto", SelectedIcon = PackIconKind.AlphaPBox,
    UnselectedIcon = PackIconKind.AlphaPBoxOutline)]
public class Producto : ModelBase<IProducto>, IProducto
{
    private string _description = string.Empty;
    private string _name = string.Empty;

    [Solicitar("Nombre", InputBoxType.Text, typeof(TextBox), Requerido = true, MinLength = 2)]
    [Vista]
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    [Solicitar("Descripcion", InputBoxType.Text, typeof(TextBox))]
    [Vista]
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }


    protected override void UpdateChanged()
    {
    }
}