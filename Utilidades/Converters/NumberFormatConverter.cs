using System;
using System.Globalization;
using System.Windows.Data;

namespace Utilidades.Converters;

public class NumberFormatConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is decimal decimalValue)
            return decimalValue.ToString("#,##0",
                new CultureInfo("es-EC")); // Ecuador usa puntos como separadores de miles

        if (value is double doubleValue) return doubleValue.ToString("#,##0", new CultureInfo("es-EC"));

        if (value is int intValue) return intValue.ToString("#,##0", new CultureInfo("es-EC"));

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var stringValue = value as string;
        if (decimal.TryParse(stringValue, NumberStyles.AllowThousands, new CultureInfo("es-EC"),
                out var decimalValue))
            return decimalValue;

        if (double.TryParse(stringValue, NumberStyles.AllowThousands, new CultureInfo("es-EC"), out var doubleValue))
            return doubleValue;

        if (int.TryParse(stringValue, NumberStyles.AllowThousands, new CultureInfo("es-EC"), out var intValue))
            return intValue;

        return 0;
    }
}