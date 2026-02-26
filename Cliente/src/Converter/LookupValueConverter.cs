using System.Globalization;
using System.Windows.Data;
using Cliente.Services;
using Utilidades.Interfaces;

namespace Cliente.Converter;

public class LookupValueConverter : IValueConverter
{
    private readonly IEntityLookup _lookup = new EntityLookup();
    private readonly Type _lookupType;

    public LookupValueConverter(Type lookupType)
    {
        _lookupType = lookupType;
    }

    public object Convert(object value, Type t, object p, CultureInfo c)
    {
        return _lookup.GetDisplayName(_lookupType, value);
    }

    public object ConvertBack(object v, Type t, object p, CultureInfo c)
    {
        return v;
    }
}