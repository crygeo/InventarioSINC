using System;
using System.Globalization;
using System.Windows.Data;

namespace Utilidades.Converters;

public class InverseBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is bool b && !b; // Si IsPasswordVisible es true, devuelve false (no editable)
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value; // No necesitamos implementar la conversión inversa.
    }
}