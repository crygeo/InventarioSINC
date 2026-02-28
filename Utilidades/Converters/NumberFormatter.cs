using System;
using System.Globalization;
using System.Windows.Data;

namespace Utilidades.Converters;

public class NumberToShortStringConverter : IValueConverter
{
    private const decimal K = 1_000m;
    private const decimal M = 1_000_000m;
    private const decimal B = 1_000_000_000m;
    private const decimal RadioBase = 0.9m;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not decimal decValue)
        {
            if (value is IConvertible c)
                decValue = c.ToDecimal(culture);
            else
                return value?.ToString() ?? string.Empty;
        }

        return ToShortString(decValue);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public static string ToShortString(decimal value)
    {
        if (value < K)
            return value.ToString("0");

        if (value < M)
        {
            var ratio = value / M;
            if (ratio >= RadioBase)
                return Format(value / M, "M");
            return Format(value / K, "K");
        }

        if (value < B)
        {
            var ratio = value / B;
            if (ratio >= RadioBase)
                return Format(value / B, "B");
            return Format(value / M, "M");
        }

        return Format(value / B, "B");
    }

    private static string Format(decimal number, string suffix)
    {
        // redondear a 2 decimales, quitar ceros innecesarios
        var str = Math.Round(number, 1, MidpointRounding.AwayFromZero).ToString("0.##");
        return str + suffix;
    }
}