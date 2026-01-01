using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using Cliente.Obj.Model;
using Cliente.View.Items;

namespace Cliente.Converter;

public class IdToSelectorJerarquicoConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        var ids = values[0] as ObservableCollection<string>;
        var identificadores = values[1] as ObservableCollection<Identificador>;
        var elementos = values[2] as ObservableCollection<ElementoJerarquico>;
        var result = values[3] as ObservableCollection<SelectorJerarquico>;

        if (identificadores == null || elementos == null)
            return null;

        if (result == null)
            result = new ObservableCollection<SelectorJerarquico>();


        foreach (var identificador in identificadores)
        {
            var valores = elementos
                .Where(e => e.IdPerteneciente == identificador.Id)
                .OrderByDescending(e => e.FechaCreacion)
                .ToList();

            var selector = new SelectorJerarquico(identificador, valores);

            // solo preselecciona si hay valores
            if (ids != null)
                selector.Seleccionado = valores.FirstOrDefault(e => ids.Contains(e.Id));

            result.Add(selector);
        }

        return result;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        var selectores = value as ObservableCollection<SelectorJerarquico>;
        if (selectores == null)
            return new object[] { null };

        var ids = new ObservableCollection<string>(
            selectores
                .Where(s => s.Seleccionado != null)
                .Select(s => s.Seleccionado!.Id)
        );

        return new object[] { ids };
    }
}