using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Utilidades.Converters;

public class CountToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type t, object p, CultureInfo c) =>
        value is int n && n > 0 ? Visibility.Visible : Visibility.Collapsed;

    public object ConvertBack(object v, Type t, object p, CultureInfo c) =>
        throw new NotImplementedException();
}