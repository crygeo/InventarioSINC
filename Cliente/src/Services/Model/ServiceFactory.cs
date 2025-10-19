using Shared.Factory;
using Shared.Interfaces.Model;

namespace Cliente.Services.Model;

public static class ServiceFactory
{
    public static ServiceBase<T> GetService<T>() where T : class, IModelObj, new()
    {
        return FactoryResolver.Resolve<ServiceBase<T>>();
    }

}