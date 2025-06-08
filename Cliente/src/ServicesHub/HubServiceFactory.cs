using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cliente.ServicesHub;
using Shared.Factory;
using Shared.Interfaces.Model;

namespace Cliente.src.ServicesHub
{
    public static class HubServiceFactory
    {
        public static HubServiceBase<TEntity> GetHubService<TEntity>() where TEntity : class, IModelObj, new()
        {
            return FactoryResolver.Resolve<HubServiceBase<TEntity>>();
        }
    }
}
