using Cliente.Services.Model;
using Shared.Factory;
using Shared.Interfaces.Model;

namespace Cliente.src.Services.Model;

public static class ServiceFactory
{
    public static ServiceBase<T> GetService<T>() where T : class, IModelObj, new()
    {
        return FactoryResolver.Resolve<ServiceBase<T>>();
    }

}