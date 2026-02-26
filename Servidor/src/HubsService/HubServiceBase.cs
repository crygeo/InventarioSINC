using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Servidor.Hubs;
using Shared.Interfaces;
using Shared.Interfaces.Model;
using Shared.Request;

namespace Servidor.HubsService;

public class HubServiceBase<TEntity> : IHubService<TEntity> where TEntity : class, IModelObj
{
    private IHubContext<Hub>? _hubContext;
    public IHubContext<Hub> HubContext => _hubContext ??= (IHubContext<Hub>)HubFactory.GetHubContext<TEntity>();


    public async Task NewItem(TEntity obj)
    {
        await HubContext.Clients.All.SendAsync($"New{typeof(TEntity).Name}", obj);
    }

    public async Task UpdateItem(TEntity obj)
    {
        await HubContext.Clients.All.SendAsync($"Update{typeof(TEntity).Name}", obj);
    }

    public async Task DeleteItem(TEntity obj)
    {
        await HubContext.Clients.All.SendAsync($"Delete{typeof(TEntity).Name}", obj);
    }
    
    public async Task UpdateProperty(string entityId, string propertyName, object newPropertyValue)
    {
        var request = GetRequest(entityId, propertyName, newPropertyValue);
        await HubContext.Clients.All.SendAsync($"UpdateProperty{typeof(TEntity).Name}", request);
    }
    
    public async Task AddItemToListAsync(string entityId, string propertyName, object itemId)
    {
        var request = GetRequest(entityId, propertyName, itemId);
        await HubContext.Clients.All.SendAsync($"ItemAdded{typeof(TEntity).Name}", request);

    }

    public async Task RemoveItemToListAsync(string entityId, string propertyName, object itemId)
    {
        var request = GetRequest(entityId, propertyName, itemId);
        await HubContext.Clients.All.SendAsync($"ItemRemoved{typeof(TEntity).Name}", request);
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