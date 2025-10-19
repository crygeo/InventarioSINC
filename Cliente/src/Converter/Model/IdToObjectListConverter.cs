using System.Globalization;
using System.Windows.Data;
using Cliente.Services.Model;
using Shared.Interfaces.Model;

namespace Cliente.Converter.Model;

public class IdToObjectListConverter<T> : IValueConverter where T : class, IModelObj, new()
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is IEnumerable<string> ids)
        {
            var service = ServiceFactory.GetService<T>();
            return service.Collection.Where(x => ids.Contains(x.Id)).ToList();
        }
        return Binding.DoNothing;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is IEnumerable<T> objs)
        {
            return objs.Select(x => x.Id).ToList();
        }
        return Binding.DoNothing;
    }
}