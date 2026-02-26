using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Shared.Interfaces;
using Shared.Interfaces.Model;
using Shared.Request;

namespace Servidor.Hubs;

public class HubBase<TEntity> : Hub, IHubNotification<TEntity> where TEntity : IModelObj
{
    public virtual async Task NewItem(TEntity obj)
    {
        await Clients.All.SendAsync($"New{typeof(TEntity).Name}", obj);
    }

    public virtual async Task UpdateItem(TEntity obj)
    {
        await Clients.All.SendAsync($"Update{typeof(TEntity).Name}", obj);
    }

    public virtual async Task DeleteItem(TEntity obj)
    {
        await Clients.All.SendAsync($"Delete{typeof(TEntity).Name}", obj);
    }

    public async Task UpdateProperty(string entityId, string propertyName, object newPropertyValue)
    {
        var request = GetRequest(entityId, propertyName, newPropertyValue);
        await Clients.All.SendAsync($"UpdateProperty{typeof(TEntity).Name}", request);
    }
    
    public async Task AddItemToListAsync(string entityId, string propertyName, object itemId)
    {
        var request = GetRequest(entityId, propertyName, itemId);
        await Clients.All.SendAsync($"AddItemToListAsync{typeof(TEntity).Name}", request);
    }

    public async Task RemoveItemToListAsync(string entityId, string propertyName, object itemId)
    {
        var request = GetRequest(entityId, propertyName, itemId);
        await Clients.All.SendAsync($"RemoveItemToListAsync{typeof(TEntity).Name}", request);
    }
    
    private object GetRequest(string entityId, string propertyName, object itemId)
    {
        return new PropertyChangedEventRequest
        {
            EntityId = entityId,
            Selector = propertyName,
            NewValue = itemId
        };
    }
    
}