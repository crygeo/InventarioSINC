namespace Shared.Factory;

public static class FactoryResolver
{
    private static readonly Dictionary<Type, object> _cache = new();

    // 🔥 Nuevo helper interno: centraliza la lógica
    private static object CreateInstance(Type targetType)
    {
        // Buscar implementación concreta asignable al tipo base
        var customType = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t =>
                !t.IsAbstract &&
                t.IsClass &&
                targetType.IsAssignableFrom(t));

        var instanceType = customType ?? targetType;
        return Activator.CreateInstance(instanceType)!;
    }


    // ✅ Versión genérica (usa helper)
    public static T Resolve<T>() where T : class
    {
        var targetType = typeof(T);

        if (_cache.TryGetValue(targetType, out var instance))
            return (T)instance;

        var result = (T)CreateInstance(targetType);
        _cache[targetType] = result;

        return result;
    }


    // ✅ Versión por Type (usa helper)
    public static object Resolve(Type type)
    {
        if (type == null)
            throw new ArgumentNullException(nameof(type));

        if (_cache.TryGetValue(type, out var instance))
            return instance;

        var result = CreateInstance(type);
        _cache[type] = result;

        return result;
    }
}