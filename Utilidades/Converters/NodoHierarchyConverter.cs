using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using Utilidades.Extencions;
using Utilidades.Factory;

namespace Utilidades.Converters;

public class NodoHierarchyConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not List<string> list || list.Count == 0)
            return new List<Nodos>();

        return list.ConstruirArbol();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}