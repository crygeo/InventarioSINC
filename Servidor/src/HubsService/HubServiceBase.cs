using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Servidor.Hubs;
using Shared.Interfaces;
using Shared.Interfaces.Model;

namespace Servidor.HubsService;

public class HubServiceBase<T> : IHubService<T> where T : class, IModelObj
{
    private IHubContext<Hub>? _hubContext;
    public IHubContext<Hub> HubContext => _hubContext ??= (IHubContext<Hub>)HubFactory.GetHubContext<T>();


    public async Task NewItem(T obj)
    {
        await HubContext.Clients.All.SendAsync($"New{typeof(T).Name}", obj);
    }

    public async Task UpdateItem(T obj)
    {
        await HubContext.Clients.All.SendAsync($"Update{typeof(T).Name}", obj);
    }

    public async Task DeleteItem(T obj)
    {
        await HubContext.Clients.All.SendAsync($"Delete{typeof(T).Name}", obj);
    }
    
    public async Task UpdateProperty(string entityId, string propertyName, object newPropertyValue)
    {
        await HubContext.Clients.All.SendAsync($"UpdateProperty{typeof(T).Name}", entityId, propertyName, newPropertyValue);
    }
    
}