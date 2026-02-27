using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Shared.ClassModel;
using Shared.Interfaces;
using Shared.Interfaces.Model;
using Shared.Request;

namespace Servidor.Repositorios;

public class RepositorioBase<TEntity> : IRepository<TEntity> where TEntity : IModelObj
{
    public RepositorioBase()
    {
        _ = new MongoDBConnection();
        Collection = MongoDBConnection._database?.GetCollection<TEntity>(NameCollection)
                     ?? throw new InvalidOperationException($"No se pudo encontrar la colección: {NameCollection}");
    }

    public IMongoCollection<TEntity> Collection { get; }

    public virtual string NameCollection => $"Rep{typeof(TEntity).Name}";

    // =========================
    // SEARCH
    // =========================

    public async Task<IEnumerable<TEntity>> SearchAsync(SearchRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Query) || request.Propiedades.Count == 0)
                return Enumerable.Empty<TEntity>();

            // Cada palabra del query debe aparecer en al menos una propiedad (AND entre palabras)
            var palabras = request.Query
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Distinct()
                .ToList();

            var filtrosPorPalabra = palabras.Select(palabra =>
            {
                // OR entre propiedades para esta palabra
                var regex = new BsonRegularExpression(palabra, "i"); // case insensitive
                var filtrosPorPropiedad = request.Propiedades
                    .Select(prop => Builders<TEntity>.Filter.Regex(prop, regex))
                    .ToList();

                return Builders<TEntity>.Filter.Or(filtrosPorPropiedad);
            });

            // AND entre todas las palabras
            var filtroFinal = Builders<TEntity>.Filter.And(filtrosPorPalabra);

            return await Collection.Find(filtroFinal)
                .Limit(request.PageSize)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en búsqueda: {ex.Message}");
            return Enumerable.Empty<TEntity>();
        }
    }

    // =========================
    // CRUD
    // =========================

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        try
        {
            return await Collection.Find(_ => true).ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener todos los documentos: {ex.Message}");
            return Enumerable.Empty<TEntity>();
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
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al insertar el documento: {ex.Message}");
            return false;
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
            if (obj == null || !obj.Deleteable) return false;

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
        return result.ModifiedCount > 0;
    }

    public async Task<bool> AddItemIdToListAsync(string entityId, string selector, object itemId)
    {
        var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entityId);
        var update = Builders<TEntity>.Update.Push(selector, itemId);

        var result = await Collection.UpdateOneAsync(filter, update);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> RemoveItemIdToListAsync(string entityId, string selector, object itemId)
    {
        var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entityId);
        var update = Builders<TEntity>.Update.Pull(selector, itemId);

        var result = await Collection.UpdateOneAsync(filter, update);
        return result.ModifiedCount > 0;
    }

    public async Task<long> RemoveItemFromAllListsAsync(string selector, object itemId)
    {
        var filter = Builders<TEntity>.Filter.AnyEq(selector, itemId);
        var update = Builders<TEntity>.Update.Pull(selector, itemId);

        var result = await Collection.UpdateManyAsync(filter, update);
        return result.ModifiedCount;
    }

    public async Task<long> RemoveItemsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var result = await Collection.DeleteManyAsync(predicate);
        return result.DeletedCount;
    }

    public async Task<List<TEntity>> GetItemsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Collection.Find(predicate).ToListAsync();
    }
}