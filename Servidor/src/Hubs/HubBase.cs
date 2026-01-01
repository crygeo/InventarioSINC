using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Shared.Interfaces;
using Shared.Interfaces.Model;

namespace Servidor.Hubs;

public class HubBase<T> : Hub, IHubNotification<T> where T : IModelObj
{
    public virtual async Task NewItem(T obj)
    {
        await Clients.All.SendAsync($"New{typeof(T).Name}", obj);
    }

    public virtual async Task UpdateItem(T obj)
    {
        await Clients.All.SendAsync($"Update{typeof(T).Name}", obj);
    }

    public virtual async Task DeleteItem(T obj)
    {
        await Clients.All.SendAsync($"Delete{typeof(T).Name}", obj);
    }

    public async Task UpdateProperty(string entityId, string propertyName, object newPropertyValue)
    {
        await Clients.All.SendAsync($"UpdateProperty{typeof(T).Name}", entityId, propertyName, newPropertyValue);
    }
}