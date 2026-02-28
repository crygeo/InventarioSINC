using System;
using System.Globalization;
using System.Windows.Data;
using Utilidades.Controls;

namespace Utilidades.Converters;

public class InputBoxConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (parameter is not string paramStr ||
            !Enum.TryParse(paramStr, out InputBoxType type))
            return value;

        // Si el modelo NO es string, conviértelo a string para el TextBox
        var text = value?.ToString() ?? string.Empty;

        return type switch
        {
            InputBoxType.Phone => text.ToPhone(),
            InputBoxType.Dni => text.ToDni(),
            InputBoxType.Ruc => text.ToRUC(),
            InputBoxType.Email => text.ToEmail(),
            InputBoxType.Name => text.ToName(),
            InputBoxType.NickName => text.ToNickName(),
            InputBoxType.Number => text.ToNumber(),
            _ => text
        };
    }


    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var text = value?.ToString() ?? string.Empty;

        if (parameter is not string paramStr ||
            !Enum.TryParse(paramStr, out InputBoxType type))
            return Binding.DoNothing;

        return type switch
        {
            InputBoxType.Phone => text.FromPhone(),
            InputBoxType.Dni => text.FromDni(),
            InputBoxType.Ruc => text.FromRUC(),
            InputBoxType.Email => text.FromEmail(),
            InputBoxType.Name => text.FromName(),
            InputBoxType.NickName => text.FromNickName(),
            InputBoxType.Number => text.FromNumber(),
            //InputBoxType.Date => text.FromDate(),
            _ => text
        };
    }
}