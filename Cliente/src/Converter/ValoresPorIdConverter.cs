using System.Globalization;
using System.Windows.Data;
using Cliente.Obj.Model;

namespace Cliente.src.Converter;

public class ValoresPorIdConverter : IMultiValueConverter
{
    public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        var id = values[0]?.ToString();
        var dict = values[1] as Dictionary<string, List<ElementoJerarquico>>;

        if (id != null && dict != null && dict.TryGetValue(id, out var lista))
            return lista;

        return Enumerable.Empty<ElementoJerarquico>();
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}