using System.Windows.Controls;
using Cliente.Attributes;
using MaterialDesignThemes.Wpf;
using Shared.Interfaces.Model.Obj;
using Utilidades.Attributes;
using Utilidades.Controls;

namespace Cliente.Obj.Model;

[AutoViewModel]
[Navegacion("PanelConfig", TituloS = "Clasificacion", SelectedIcon = PackIconKind.AlphaCBox,
    UnselectedIcon = PackIconKind.AlphaCBoxOutline)]
public class Clasificacion : ModelBase<IClasificacion>, IClasificacion
{
    private string _idRecepcionCarga;
    private float _pesoDesecho;
    private float _pesoNeto;

    public float PesoBruto => PesoNeto - PesoDesecho;

    //public float TotalClasificado = ServiceFactory.GetService<ElementoJerarquico>().
    public float Rendimiento => PesoNeto - PesoDesecho;

    public string IdRecepcionCarga
    {
        get => _idRecepcionCarga;
        set => SetProperty(ref _idRecepcionCarga, value);
    }

    [Solicitar("Peso Desecho", InputBoxType.Decimal, typeof(TextBox))]
    [Vista]
    public float PesoDesecho
    {
        get => _pesoDesecho;
        set => SetProperty(ref _pesoDesecho, value);
    }

    [Solicitar("Peso Neto",  InputBoxType.Decimal,typeof(TextBox))]
    [Vista]
    public float PesoNeto
    {
        get => _pesoNeto;
        set => SetProperty(ref _pesoNeto, value);
    }

    protected override void UpdateChanged()
    {
        OnPropertyChanged(nameof(PesoBruto));
        OnPropertyChanged(nameof(Rendimiento));
    }
}