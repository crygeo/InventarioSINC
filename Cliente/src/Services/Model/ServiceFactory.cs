using Shared.Factory;
using Shared.Interfaces.Model;

namespace Cliente.Services.Model;

public static class ServiceFactory
{
    public static ServiceBase<T> GetService<T>() where T : class, IModelObj, new()
    {
        return FactoryResolver.Resolve<ServiceBase<T>>();
    }

    public static object GetService(Type modelType)
    {
        if (modelType == null)
            throw new ArgumentNullException(nameof(modelType));

        var serviceType = typeof(ServiceBase<>).MakeGenericType(modelType);

        return FactoryResolver.Resolve(serviceType);
    }
}