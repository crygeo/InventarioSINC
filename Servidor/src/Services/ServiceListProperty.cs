using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Servidor.HubsService;
using Servidor.Repositorios;
using Shared.Interfaces;
using Shared.Interfaces.Model;

namespace Servidor.Services;

public class ServiceListProperty<TEntity> : ServiceBase<TEntity>, IUpdateListProperty<TEntity> where TEntity : class, IModelObj
{
    public RepositorioListProperty<TEntity> RepostListProperty => (RepositorioListProperty<TEntity>)Repository;
    public HubListNotifier<TEntity> HubListNotifier => (HubListNotifier<TEntity>)HubService;
    
    public async Task<bool> AddItemIdToListAsync(string entityId, string selector, string itemId)
    {
        var update = await RepostListProperty.AddItemIdToListAsync(entityId, selector, itemId);
        if (update)
        {
            await HubListNotifier.ItemAddedAsync(entityId, selector, itemId);
        }
        return  update;
    }

    public async Task<bool> RemoveItemToListAsync(string entityId, string selector, string itemId)
    {
        var update = await RepostListProperty.RemoveItemToListAsync(entityId, selector, itemId);
        if (update)
        {
            await HubListNotifier.ItemRemovedAsync(entityId, selector, itemId);
        }
        return  update;
    }
    


    
}


