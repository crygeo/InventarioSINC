using System.Globalization;
using System.Windows.Data;
using Cliente.ViewModel.Model.Detail;

namespace Cliente.Converter;

/// <summary>
/// Cuenta elementos anidados dentro de una colección de TurnoDetailVM.
/// ConverterParameter: "Secciones" | "Grupos"
/// </summary>
public class CountNestedConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not IEnumerable<TurnoDetailVM> turnos)
            return 0;

        return parameter switch
        {
            "Secciones" => turnos.Sum(t => t.Secciones.Count),
            "Grupos"    => turnos.Sum(t => t.Secciones.Sum(s => s.Grupos.Count)),
            _           => 0
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException($"{nameof(CountNestedConverter)} no soporta ConvertBack.");
}