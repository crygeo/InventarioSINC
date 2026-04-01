using System.Windows.Controls;
using Cliente.Attributes;
using MaterialDesignThemes.Wpf;
using Shared.Interfaces.Model.Obj;
using Utilidades.Attributes;
using Utilidades.Controls;
using Utilidades.Interfaces;

namespace Cliente.Obj.Model;

[AutoViewModel]
[Navegacion("PageGestionEmployer",
    TituloS = "Turno Trabajo",
    SelectedIcon = PackIconKind.ClockOutline,
    UnselectedIcon = PackIconKind.ClockOutline)]
public class TurnoTrabajo : ModelBase<ITurnoTrabajo>, ITurnoTrabajo, IDisplayable
{
    private string _nombre = string.Empty;
    private TimeSpan _horaInicio;
    private TimeSpan _horaSalidaMinima;
    private TimeSpan _horasMinimas;

    [Solicitar("Nombre", InputBoxType.Text, typeof(TextBox), Requerido = true)]
    [Vista]
    [Buscable]
    public string Nombre
    {
        get => _nombre;
        set => SetProperty(ref _nombre, value);
    }

    [Solicitar("Hora Inicio", InputBoxType.Time, typeof(TimePicker), Requerido = true)]
    [Vista]
    public TimeSpan HoraInicio
    {
        get => _horaInicio;
        set => SetProperty(ref _horaInicio, value);
    }

    [Solicitar("Hora Salida Mínima", InputBoxType.Time, typeof(TimePicker), Requerido = true)]
    [Vista]
    public TimeSpan HoraSalidaMinima
    {
        get => _horaSalidaMinima;
        set => SetProperty(ref _horaSalidaMinima, value);
    }

    [Solicitar("Horas Mínimas", InputBoxType.Time, typeof(TimePicker), Requerido = true)]
    [Vista]
    public TimeSpan HorasMinimas
    {
        get => _horasMinimas;
        set => SetProperty(ref _horasMinimas, value);
    }

    public override string ToString()
    {
        return $"{Nombre} ({HoraInicio:hh\\:mm} - {HoraSalidaMinima:hh\\:mm}, Min. {HorasMinimas:hh\\:mm})";
    }

    protected override void UpdateChanged() { }

    public string DescripcionVisual => Nombre;
}