using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Servidor.src.Factory
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

            // Buscar tipo personalizado
            var customType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t =>
                    !t.IsAbstract &&
                    t.IsClass &&
                    t.BaseType?.IsGenericType == true &&
                    t.BaseType.GetGenericTypeDefinition() == typeof(T).GetGenericTypeDefinition() &&
                    t.BaseType.GetGenericArguments().First() == targetType);


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
