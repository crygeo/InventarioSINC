using System.Windows.Controls;
using Cliente.Attributes;
using Cliente.View.Model;
using MaterialDesignThemes.Wpf;
using Shared.Interfaces.Model.Obj;
using Utilidades.Attributes;
using Utilidades.Controls;

namespace Cliente.Obj.Model;

[AutoViewModel(typeof(PageAreasV))]
[Navegacion("PageGestionEmployer", TituloS = "Area", TituloP = "Areas", SelectedIcon = PackIconKind.AlphaTBox,
    UnselectedIcon = PackIconKind.AlphaTBoxOutline)]
public class Area : ModelBase<IArea>, IArea
{
    private string _nombre = string.Empty;

    [Solicitar("Nombre del area", InputBoxType.Text, typeof(TextBox),  Requerido = true)]
    [Vista]
    public string Nombre
    {
        get => _nombre;
        set => SetProperty(ref _nombre, value);
    }

    

}