using System;
using System.Globalization;
using System.Windows.Data;

namespace Utilidades.Converters;

public class DateToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime dateTime) return dateTime.ToString("dd/MM/yyyy");

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string str &&
            DateTime.TryParseExact(str, "dd/MM/yy", culture, DateTimeStyles.None, out var date))
            return date;

        return null; // Devuelve null si la conversión no es posible
    }
}