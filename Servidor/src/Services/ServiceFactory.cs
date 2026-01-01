using Shared.Factory;
using Shared.Interfaces.Model;

namespace Servidor.Services;

public static class ServiceFactory
{
    public static ServiceBase<T> GetService<T>() where T : class, IModelObj
    {
        return FactoryResolver.Resolve<ServiceBase<T>>();
    }
}