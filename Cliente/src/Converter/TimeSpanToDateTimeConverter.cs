using System.Globalization;
using System.Windows.Data;

namespace Cliente.Converter;

public class TimeSpanToDateTimeConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is TimeSpan ts)
            return DateTime.Today.Add(ts);
        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is DateTime dt)
            return dt.TimeOfDay;
        return null;
    }

    
}