using System.Collections.ObjectModel;
using System.Windows.Controls;
using Cliente.Attributes;
using Cliente.View.Items;
using MaterialDesignThemes.Wpf;
using Shared.Interfaces.Model.Obj;
using Shared.Interfaces.ModelsBase;
using Utilidades.Attributes;
using Utilidades.Controls;

namespace Cliente.Obj.Model;

[AutoViewModel]
[Navegacion("PanelConfig", TituloS = "Recepcion", SelectedIcon = PackIconKind.AlphaEBox,
    UnselectedIcon = PackIconKind.AlphaEBoxOutline)]
public class RecepcionCarga : ModelBase<IRecepcionCarga>, IRecepcionCarga
{
    private List<ICarga> _camiones = [];
    private DateTime _fechaIngreso = DateTime.Now;
    private byte[]? _guiaGlobal;
    private IEnumerable<string> _identificadores = new ObservableCollection<string>();

    private string _idProveedor = string.Empty;
    private string _nota = string.Empty;
    private float _pesoTotal;

    [Solicitar("Identificadores", InputBoxType.None, typeof(IdentificadoresSelect), Requerido = true)]
    [Vista(LookupType = typeof(ElementoJerarquico))]
    public IEnumerable<string> IdIdentificadores
    {
        get => _identificadores;
        set => SetProperty(ref _identificadores, value);
    }


    [Solicitar("Proveedor", InputBoxType.None, typeof(EntitySelector), Requerido = true, EntityType = typeof(Proveedor))]
    [Vista(LookupType = typeof(Proveedor))]
    public string IdProveedor
    {
        get => _idProveedor;
        set => SetProperty(ref _idProveedor, value);
    }


    [Solicitar("Fecha de ingreso", InputBoxType.None, typeof(DatePicker), Requerido = true)]
    [Vista]
    public DateTime FechaIngreso
    {
        get => _fechaIngreso;
        set => SetProperty(ref _fechaIngreso, value);
    }


    //[Solicitar("Camiones", ItemType = typeof(DataGrid), Requerido = true, InputBoxConvert = InputBoxType.None)]
    public IEnumerable<ICarga> Camiones
    {
        get => _camiones;
        set => SetProperty(ref _camiones, value?.ToList() ?? []);
    }

    [Vista]
    public float PesoTotal
    {
        get => _pesoTotal;
        set => SetProperty(ref _pesoTotal, value);
    }

    public byte[]? GuiaGlobal
    {
        get => _guiaGlobal;
        set => SetProperty(ref _guiaGlobal, value);
    }

    public string Nota
    {
        get => _nota;
        set => SetProperty(ref _nota, value);
    }

    protected override void UpdateChanged()
    {
        // Opcional: lógica específica luego de una actualización
    }
}