using System;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Shared.Factory;
using Shared.Interfaces.Model;

namespace Servidor.Hubs;

public static class HubFactory
{
    public static IServiceProvider ServiceProvider { get; set; } = null!;


    public static HubBase<T> GetHub<T>() where T : class, IModelObj
    {
        return FactoryResolver.Resolve<HubBase<T>>();
    }

    public static IHubContext<THub> GetContext<THub>() where THub : Hub
    {
        return ServiceProvider.GetRequiredService<IHubContext<THub>>();
    }

    public static IHubContext GetContext(Type hubType)
    {
        var contextType = typeof(IHubContext<>).MakeGenericType(hubType);
        return (IHubContext)ServiceProvider.GetRequiredService(contextType);
    }

    public static IHubContext GetHubContext<TEntity>() where TEntity : class, IModelObj
    {
        var hub = GetHub<TEntity>();
        var hubType = hub.GetType();
        var contextType = GetContext(hubType);
        return contextType;
    }
}