using System.Collections.Concurrent;

namespace Shared.Factory
{
    public static class FactoryResolver
    {

        private static readonly ConcurrentDictionary<Type, object> _cache = new();

        public static T Resolve<T>()
            where T : class
        {
            var targetType = typeof(T);

            if (_cache.TryGetValue(targetType, out var instance))
                return (T)instance;

            var customType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t =>
                    !t.IsAbstract &&
                    t.IsClass &&
                    targetType.IsAssignableFrom(t)); // 👈 clave: incluye subclases concretas

            T result;

            if (customType != null)
            {
                result = (T)Activator.CreateInstance(customType)!;
            }
            else
            {
                result = (T)Activator.CreateInstance(targetType)!;
            }

            _cache[targetType] = result;
            return result;
        }


    }
}
