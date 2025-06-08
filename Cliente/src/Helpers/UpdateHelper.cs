using System.Collections.Concurrent;
using Shared.Interfaces.Model;
using System.Reflection;
using Cliente.Attributes;

namespace Cliente.Helpers;

public static class UpdateHelper
{
    private static readonly ConcurrentDictionary<Type, PropertyInfo[]> _propertiesCache = new();

    public static void UpdateFrom<T>(this T target, T source)
    {
        if (target == null || source == null)
            return;

        var type = typeof(T);

        var properties = _propertiesCache.GetOrAdd(type, t =>
            t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p =>
                    p.CanRead &&
                    p.CanWrite &&
                    !p.IsDefined(typeof(NoActualizarAttribute), inherit: true)) // excluye marcados
                .ToArray());

        foreach (var prop in properties)
        {
            try
            {
                var value = prop.GetValue(source);
                prop.SetValue(target, value);
            }
            catch
            {
                // Ignorar errores de asignación
            }
        }
    }
}
