using System.Windows.Controls;
using Cliente.Attributes;
using Cliente.View.Model;
using MaterialDesignThemes.Wpf;
using Shared.Interfaces.Model.Obj;
using Utilidades.Attributes;
using Utilidades.Controls;


// ReSharper disable once IdentifierTypo
namespace Cliente.Obj.Model;

[AutoViewModel(typeof(PageValoresV))]
[Navegacion("PanelConfig", TituloS = "Valores", SelectedIcon = PackIconKind.AlphaVBox,
    UnselectedIcon = PackIconKind.AlphaVBoxOutline)]
public class ElementoJerarquico : ModelBase<ElementoJerarquico>, IElementoJerarquico
{
    private string _descripcion = string.Empty;
    private DateTime _fechaCreacion = DateTime.Now;
    private string _idPerteneciente = string.Empty;
    private string _nombre = string.Empty;
    private string _valor = string.Empty;

    public string IdPerteneciente
    {
        get => _idPerteneciente;
        set => SetProperty(ref _idPerteneciente, value);
    }

    public string Nombre
    {
        get => _nombre;
        set => SetProperty(ref _nombre, value);
    }

    public string Descripcion
    {
        get => _descripcion;
        set => SetProperty(ref _descripcion, value);
    }

    [Solicitar("Valor", InputBoxType.Text, typeof(TextBox), Requerido = true)]
    public string Valor
    {
        get => _valor;
        set => SetProperty(ref _valor, value);
    }

    public DateTime FechaCreacion
    {
        get => _fechaCreacion;
        set => SetProperty(ref _fechaCreacion, value);
    }

    protected override void UpdateChanged()
    {
    }

    public override string ToString()
    {
        //var identificador = ServiceFactory.GetService<Identificador>().GetById(IdPerteneciente);

        //if (identificador != null)
        //{
        //    return $"{identificador.Name}: {Valor}";
        //}

        return Valor;
    }
}