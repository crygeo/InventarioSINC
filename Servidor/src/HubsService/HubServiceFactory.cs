using Servidor.src.Factory;
using Servidor.src.Repositorios;
using Shared.Interfaces;
using Shared.Interfaces.Model;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace Servidor.src.HubsService
{
    public static class HubServiceFactory
    {
        public static HubServiceBase<T> GetHubService<T>() where T : class, IModelObj
        {
            return FactoryResolver.Resolve<HubServiceBase<T>>();
        }
    }
}