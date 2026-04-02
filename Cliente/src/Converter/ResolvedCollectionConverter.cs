

using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace Cliente.Converter;

/// <summary>
/// Convierte una List&lt;object&gt; resuelta por ReferenceEnricher en un string
/// con los valores de la propiedad indicada, separados por saltos de línea.
/// </summary>
public class ResolvedCollectionConverter : IValueConverter
{
    public string MemberName { get; set; } = "Nombre";

    // Cache de PropertyInfo por tipo para no usar reflection en cada render
    private static readonly Dictionary<(Type, string), PropertyInfo?> _propCache = new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not IEnumerable<object> items)
            return string.Empty;

        var lines = new List<string>();

        foreach (var item in items)
        {
            if (item is null) continue;

            var type = item.GetType();
            var key  = (type, MemberName);

            if (!_propCache.TryGetValue(key, out var prop))
            {
                prop = type.GetProperty(
                    MemberName,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                _propCache[key] = prop;
            }

            var display = prop?.GetValue(item)?.ToString() ?? item.ToString() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(display))
                lines.Add(display);
        }

        return string.Join("\n", lines);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}