using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace Utilidades.Converters;

public class ArrayToEnumerableConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is IEnumerable enumerable && !(value is string)) return enumerable;

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}