using System.Collections;
using System.Reflection;
using System.Windows;
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
    // Cliente/src/Helpers/DataGridColumnBuilder.cs
    public static void BuildColumns(DataGrid dataGrid, Type itemType)
    {
        if (dataGrid == null) throw new ArgumentNullException(nameof(dataGrid));
        if (itemType == null) throw new ArgumentNullException(nameof(itemType));

        var columns = itemType.GetProperties()
            .Select(p => new
            {
                Prop = p,
                Vista = p.GetCustomAttribute<VistaAttribute>(),
                Solicitar = p.GetCustomAttribute<SolicitarAttribute>()
            })
            .Where(x => x.Vista != null && x.Vista.Visible)
            .OrderBy(x => x.Vista!.Orden)
            .ToList();

        dataGrid.Columns.Clear();

        foreach (var item in columns)
        {
            var prop = item.Prop;
            var vista = item.Vista!;
            var solicitar = item.Solicitar;

            var header =
                vista?.Nombre ??
                solicitar?.Nombre ??
                prop.Name;

            var propType = prop.PropertyType;
            var isEnumerable = typeof(IEnumerable).IsAssignableFrom(propType)
                               && propType != typeof(string);
            var hasLookup = vista?.LookupType != null;
            var displayMember = vista?.DisplayMember ?? "Nombre";

            DataGridColumn column;

            // Dentro del bloque hasLookup && isEnumerable, tras crear la columna:
            if (hasLookup && isEnumerable)
            {
                var converter = new ResolvedCollectionConverter { MemberName = displayMember };

                var elementStyle = new Style(typeof(TextBlock));
                elementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));

                column = new DataGridTextColumn
                {
                    Header = header,
                    ElementStyle = elementStyle,
                    Binding = new Binding($"Ref[{prop.Name}]")
                    {
                        Converter = converter,
                        TargetNullValue = string.Empty
                    }
                };
            }
            else if (hasLookup)
            {
                // ID simple → objeto resuelto → propiedad DisplayMember
                column = new DataGridTextColumn
                {
                    Header = header,
                    Binding = new Binding($"Ref[{prop.Name}].{displayMember}")
                    {
                        TargetNullValue = string.Empty
                    }
                };
            }
            else if (isEnumerable)
            {
                // Colección sin lookup → mostrar Count
                column = new DataGridTextColumn
                {
                    Header = header,
                    Binding = new Binding($"Model.{prop.Name}.Count")
                };
            }
            else
            {
                // Valor simple
                column = new DataGridTextColumn
                {
                    Header = header,
                    Binding = new Binding($"Model.{prop.Name}")
                };
            }

            dataGrid.Columns.Add(column);
        }
    }
    
    public static void BuildColumnsFromModel(DataGrid dataGrid, Type itemType)
{
    if (dataGrid == null) throw new ArgumentNullException(nameof(dataGrid));
    if (itemType == null) throw new ArgumentNullException(nameof(itemType));

    var columns = itemType.GetProperties()
        .Select(p => new
        {
            Prop = p,
            Vista = p.GetCustomAttribute<VistaAttribute>(),
            Solicitar = p.GetCustomAttribute<SolicitarAttribute>()
        })
        .Where(x => x.Vista != null && x.Vista.Visible)
        .OrderBy(x => x.Vista!.Orden)
        .ToList();

    dataGrid.Columns.Clear();

    foreach (var item in columns)
    {
        var prop = item.Prop;
        var vista = item.Vista!;
        var solicitar = item.Solicitar;

        var header =
            vista?.Nombre ??
            solicitar?.Nombre ??
            prop.Name;

        var propType = prop.PropertyType;

        bool isCollection =
            propType != typeof(string) &&
            typeof(IEnumerable).IsAssignableFrom(propType);

        bool hasLookup = vista.LookupType != null;
        var displayMember = vista.DisplayMember ?? "Nombre";

        DataGridColumn column;

        // 🔥 CASO 1: colección de objetos (ya resueltos)
        if (hasLookup && isCollection)
        {
            var converter = new ResolvedCollectionConverter
            {
                MemberName = displayMember
            };

            var style = new Style(typeof(TextBlock));
            style.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));

            column = new DataGridTextColumn
            {
                Header = header,
                ElementStyle = style,
                Binding = new Binding(prop.Name)
                {
                    Converter = converter,
                    TargetNullValue = string.Empty
                }
            };
        }
        // 🟡 CASO 2: objeto simple (lookup ya resuelto)
        else if (hasLookup)
        {
            column = new DataGridTextColumn
            {
                Header = header,
                Binding = new Binding($"{prop.Name}.{displayMember}")
                {
                    TargetNullValue = string.Empty
                }
            };
        }
        // 🟠 CASO 3: colección sin lookup
        else if (isCollection)
        {
            column = new DataGridTextColumn
            {
                Header = header,
                Binding = new Binding($"{prop.Name}.Count")
            };
        }
        // 🟢 CASO 4: valor simple
        else
        {
            column = new DataGridTextColumn
            {
                Header = header,
                Binding = new Binding(prop.Name)
            };
        }

        dataGrid.Columns.Add(column);
    }
}
}