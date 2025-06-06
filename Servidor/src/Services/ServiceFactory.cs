using Servidor.Services;
using Servidor.src.Factory;
using Shared.Interfaces.Model;

namespace Servidor.src.Services
{
    public static class ServiceFactory
    {
        public static ServiceBase<T> GetService<T>() where T : class, IModelObj
        {
            return FactoryResolver.Resolve<ServiceBase<T>>();
        }
    }
}
