using Shared.Factory;
using Shared.Interfaces.Model;

namespace Cliente.ServicesHub
{
    public static class HubServiceFactory
    {
        public static HubServiceBase<TEntity> GetHubService<TEntity>() where TEntity : class, IModelObj, new()
        {
            return FactoryResolver.Resolve<HubServiceBase<TEntity>>();
        }
    }
}
