using Cliente.src.Services.Model;
using Cliente.src.ServicesHub;
using Shared.Interfaces;
using Shared.Interfaces.Model;
using Shared.Interfaces.ModelsBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilidades.Interfaces;

namespace Cliente.src.Services
{
    public static class ServiceFactory
    {
        private static readonly Dictionary<Type, object> _services = new();

        public static ServiceBase<T> GetService<T>() where T : IModelObj
        {
            var type = typeof(T);
            if (_services.TryGetValue(type, out var service))
                return (ServiceBase<T>)service;

            // Verificar si hay un servicio personalizado
            var customServiceType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => typeof(ServiceBase<T>).IsAssignableFrom(t) && typeof(ICustomObjs).IsAssignableFrom(t));

            if (customServiceType != null)
            {
                var customInstance = Activator.CreateInstance(customServiceType);
                _services[type] = customInstance!;
                return (ServiceBase<T>)customInstance!;
            }

            // Si no hay personalizado, crea uno genérico
            var hubServiceType = typeof(HubServiceBase<>).MakeGenericType(type);
            var hubService = Activator.CreateInstance(hubServiceType);

            var serviceBaseType = typeof(ServiceBase<>).MakeGenericType(type);
            var genericService = Activator.CreateInstance(serviceBaseType, hubService);

            _services[type] = genericService!;
            return (ServiceBase<T>)genericService!;
        }
    }

}

