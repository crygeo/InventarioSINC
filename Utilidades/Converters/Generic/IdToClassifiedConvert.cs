using System;
using System.Globalization;
using System.Windows.Data;
using Utilidades.Factory;
using Utilidades.Interfaces;

namespace Utilidades.Converters.Generic;

public class IdToClassifiedConvert<T> : IValueConverter where T : class, IClassified, new()
{
    private readonly Func<string, T?> _obtenerPorId;

    public IdToClassifiedConvert(Func<string, T?> obtenerPorId)
    {
        _obtenerPorId = obtenerPorId;
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string id)
        {
            var result = _obtenerPorId(id);
            return result ?? ClassifiedFactory.CrearClasificado<T>();
        }

        return ClassifiedFactory.CrearClasificado<T>("Id inválido");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}