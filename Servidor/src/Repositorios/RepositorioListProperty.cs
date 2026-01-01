using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using Shared.Interfaces;
using Shared.Interfaces.Model;

namespace Servidor.Repositorios;

public class RepositorioListProperty<TEntity> :RepositorioBase<TEntity>, IUpdateListProperty<TEntity>
    where TEntity : IModelObj
{

    public async Task<bool> AddItemIdToListAsync(
        string entityId,
        string selector,
        string itemId)
    {
        var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entityId);
        var update = Builders<TEntity>.Update.AddToSet(selector, itemId);

        var result = await Collection.UpdateOneAsync(filter, update);
        return result.IsAcknowledged && result.MatchedCount > 0;
    }

    public async Task<bool> RemoveItemToListAsync(
        string entityId,
        string selector,
        string itemId)
    {
        var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entityId);
        var update = Builders<TEntity>.Update.Pull(selector, itemId);

        var result = await Collection.UpdateOneAsync(filter, update);
        return result.IsAcknowledged && result.MatchedCount > 0;
    }
}
