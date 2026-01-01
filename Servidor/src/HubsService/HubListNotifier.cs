using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Shared.Interfaces;
using Shared.Interfaces.Model;

namespace Servidor.HubsService;

public class HubListNotifier<TEntity> : HubServiceBase<TEntity>, IListChangeNotifier where TEntity : class, IModelObj
{
    public async Task ItemAddedAsync(string entityId, string propertyName, string itemId)
    {
        await HubContext.Clients.All.SendAsync($"ItemAdded{typeof(TEntity).Name}", entityId, propertyName, itemId);

    }

    public async Task ItemRemovedAsync(string entityId, string propertyName, string itemId)
    {
        await HubContext.Clients.All.SendAsync($"ItemRemoved{typeof(TEntity).Name}", entityId, propertyName, itemId);
    }
}