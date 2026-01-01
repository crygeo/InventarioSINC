using Shared.Factory;
using Shared.Interfaces.Model;

namespace Servidor.HubsService;

public static class HubServiceFactory
{
    public static HubServiceBase<T> GetHubService<T>() where T : class, IModelObj
    {
        return FactoryResolver.Resolve<HubServiceBase<T>>();
    }
}