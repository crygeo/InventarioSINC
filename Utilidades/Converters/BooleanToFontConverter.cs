using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Utilidades.Converters;

public class BooleanToFontConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var isVisible = (bool)value;
        return isVisible ? new FontFamily("Segoe UI") : new FontFamily("Segoe UI Symbol");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return DependencyProperty.UnsetValue;
    }
}