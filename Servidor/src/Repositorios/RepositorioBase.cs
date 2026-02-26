using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using Shared.ClassModel;
using Shared.Interfaces;
using Shared.Interfaces.Model;

namespace Servidor.Repositorios;

public class RepositorioBase<TEntity> : IRepository<TEntity> where TEntity : IModelObj
{
    
    public RepositorioBase()
    {
        _ = new MongoDBConnection(); // Inicializa la conexión a la base de datos
        Collection = MongoDBConnection._database?.GetCollection<TEntity>(NameCollection)
                     ?? throw new InvalidOperationException($"No se pudo encontrar la colección: {NameCollection}");
    }

    public IMongoCollection<TEntity> Collection { get; }


    public virtual string NameCollection => $"Rep{typeof(TEntity).Name}";

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        try
        {
            return await Collection.Find(_ => true).ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener todos los documentos: {ex.Message}");
            return Enumerable.Empty<TEntity>(); // Retorna una lista vacía en caso de error
        }
    }

    public async Task<PagedResult<TEntity>> GetPagedAsync(int page, int pageSize)
    {
        try
        {
            var total = await Collection.CountDocumentsAsync(_ => true);

            var items = await Collection.Find(_ => true)
                .Skip(page * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return new PagedResult<TEntity>
            {
                Items = items,
                TotalCount = (int)total,
                Page = page,
                PageSize = pageSize
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en paginación: {ex.Message}");

            return new PagedResult<TEntity>
            {
                Items = [],
                TotalCount = 0,
                Page = page,
                PageSize = pageSize
            };
        }
    }


    public virtual async Task<TEntity?> GetByIdAsync(string id)
    {
        try
        {
            return await Collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener el documento con ID {id}: {ex.Message}");
            return default;
        }
    }

    public virtual async Task<bool> CreateAsync(TEntity entity)
    {
        try
        {
            await Collection.InsertOneAsync(entity);
            return true; // Inserción exitosa
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al insertar el documento: {ex.Message}");
            return false; // Fallo en la inserción
        }
    }

    public virtual async Task<bool> UpdateAsync(string id, TEntity entity)
    {
        try
        {
            var result = await Collection.ReplaceOneAsync(x => x.Id == id, entity);
            return result.ModifiedCount > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al actualizar el documento con ID {id}: {ex.Message}");
            return false;
        }
    }

    public virtual async Task<bool> DeleteAsync(string id)
    {
        try
        {
            var obj = await GetByIdAsync(id);
            if (obj == null || !obj.Deleteable) return false; // Si el objeto no existe, no se puede eliminar

            var result = await Collection.DeleteOneAsync(x => x.Id == id);
            return result.DeletedCount > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar el documento con ID {id}: {ex.Message}");
            return false;
        }
    }
    
    public async Task<bool> UpdateProperty(string entityId, string selector, object newValues)
    {
        var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entityId);
        var update = Builders<TEntity>.Update.Set(selector, newValues);

        var result = await Collection.UpdateOneAsync(filter, update);
        return result.IsAcknowledged && result.MatchedCount > 0;
    }

    
    public async Task<bool> AddItemIdToListAsync(string entityId, string selector, object itemId)
    {
        var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entityId);
        var update = Builders<TEntity>.Update.AddToSet(selector, itemId);

        var result = await Collection.UpdateOneAsync(filter, update);
        return result.IsAcknowledged && result.MatchedCount > 0;
    }

    
    public async Task<bool> RemoveItemIdToListAsync(
        string entityId,
        string selector,
        object itemId)
    {
        var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entityId);
        var update = Builders<TEntity>.Update.Pull(selector, itemId);

        var result = await Collection.UpdateOneAsync(filter, update);

        return result.IsAcknowledged && result.ModifiedCount > 0;
    }


    
    public async Task<long> RemoveItemFromAllListsAsync(
        string selector,
        object itemId)
    {
        var filter = Builders<TEntity>.Filter.AnyEq(selector, itemId);
        var update = Builders<TEntity>.Update.Pull(selector, itemId);

        var result = await Collection.UpdateManyAsync(filter, update);

        return result.ModifiedCount;
    }

    public async Task<long> RemoveItemsAsync(
        Expression<Func<TEntity, bool>> predicate)
    {
        var result = await Collection.DeleteManyAsync(predicate);
        return result.DeletedCount;
    }
    
    public async Task<List<TEntity>> GetItemsAsync(
        Expression<Func<TEntity, bool>> predicate)
    {
        return await Collection
            .Find(predicate)
            .ToListAsync();
    }
}