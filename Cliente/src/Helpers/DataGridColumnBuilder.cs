using System.Collections;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;
using Cliente.Converter;
using Utilidades.Attributes;

namespace Cliente.Helpers;

public static class DataGridColumnBuilder
{
    /// <summary>
    /// Genera columnas en el DataGrid basándose en propiedades 
    /// decoradas con [Vista] del tipo especificado.
    /// </summary>
    public static void BuildColumns(DataGrid dataGrid, Type itemType)
    {
        if (dataGrid == null) throw new ArgumentNullException(nameof(dataGrid));
        if (itemType == null) throw new ArgumentNullException(nameof(itemType));

        var columns = itemType.GetProperties()
            .Select(p => new
            {
                Prop = p,
                Vista = p.GetCustomAttribute<VistaAttribute>()
            })
            .Where(x => x.Vista != null && x.Vista.Visible)
            .OrderBy(x => x.Vista!.Orden)
            .ToList();

        dataGrid.Columns.Clear();

        foreach (var item in columns)
        {
            var prop = item.Prop;
            var vista = item.Vista!;

            var header = vista.Nombre ?? prop.Name;

            var isEnumerable =
                typeof(IEnumerable).IsAssignableFrom(prop.PropertyType)
                && prop.PropertyType != typeof(string);

            Binding binding;

            if (vista.LookupType != null)
            {
                binding = new Binding($"Ref[{prop.Name}].{vista.DisplayMember ?? "Nombre"}");
            }
            else
            {
                binding = isEnumerable
                    ? new Binding($"Model.{prop.Name}.Count")
                    : new Binding($"Model.{prop.Name}");
            }

            DataGridColumn column = vista.LookupType != null
                ? new DataGridTextColumn
                {
                    Header = header,
                    Binding = new Binding(prop.Name)
                    {
                        Converter = new LookupValueConverter(vista.LookupType)
                    }
                }
                : new DataGridTextColumn
                {
                    Header = header,
                    Binding = binding
                };

            dataGrid.Columns.Add(column);
        }
    }
}