using Shared.Interfaces.Model.Obj;
using System.Globalization;
using System.Windows.Data;

namespace Cliente.Converter;

public class IdToProveedorConverter : IValueConverter
{
    public IEnumerable<IProveedor>? ListaProveedores { get; set; }

    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string id && ListaProveedores != null)
        {
            return ListaProveedores.FirstOrDefault(p => p.Id == id);
        }
        return null;
    }

    public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is IProveedor proveedor ? proveedor.Id : null;
    }
}
