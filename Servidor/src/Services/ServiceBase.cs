using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using Servidor.HubsService;
using Servidor.Model;
using Servidor.Repositorios;
using Shared.ClassModel;
using Shared.Interfaces;
using Shared.Interfaces.Model;
using Shared.Request;

namespace Servidor.Services;

public class ServiceBase<TEntity> : IService<TEntity> where TEntity : class, IModelObj
{
    private HubServiceBase<TEntity>? _hubService;
    private RepositorioBase<TEntity>? _repository;

    public virtual IRepository<TEntity> Repository => _repository ??= RepositorioFactory.GetRepositorio<TEntity>();
    public virtual IHubService<TEntity> HubService => _hubService ??= HubServiceFactory.GetHubService<TEntity>();

    // =========================
    // SEARCH
    // =========================

    public Task<IEnumerable<TEntity>> SearchAsync(SearchRequest request)
        => Repository.SearchAsync(request);

    // =========================
    // CRUD
    // =========================

    public async Task<bool> CreateAsync(TEntity entity)
    {
        if (await Repository.CreateAsync(entity))
        {
            await HubService.NewItem(entity);
            return true;
        }
        return false;
    }

    public async Task<bool> UpdateAsync(string id, TEntity entity)
    {
        if (await Repository.UpdateAsync(id, entity))
        {
            await HubService.UpdateItem(entity);
            return true;
        }
        return false;
    }

    public virtual async Task<bool> DeleteAsync(string id)
    {
        var entity = await Repository.GetByIdAsync(id);
        if (entity != null && await Repository.DeleteAsync(id))
        {
            await HubService.DeleteItem(entity);
            return true;
        }
        return false;
    }

    public virtual Task<PagedResult<TEntity>> GetPagedAsync(int page, int pageSize)
        => Repository.GetPagedAsync(page, pageSize);

    public Task<IEnumerable<TEntity>> GetAllAsync()
        => Repository.GetAllAsync();

    public Task<TEntity?> GetByIdAsync(string id)
        => Repository.GetByIdAsync(id);

    public async Task<bool> UpdateProperty(string entityId, string selector, object itemId)
    {
        var entity = await Repository.UpdateProperty(entityId, selector, itemId);
        if (entity) await HubService.UpdateProperty(entityId, selector, itemId);
        return entity;
    }

    public async Task<bool> AddItemIdToListAsync(string entityId, string selector, object itemId)
    {
        var update = await Repository.AddItemIdToListAsync(entityId, selector, itemId);
        if (update) await HubService.AddItemToListAsync(entityId, selector, itemId);
        return update;
    }

    public async Task<bool> RemoveItemIdToListAsync(string entityId, string selector, object itemId)
    {
        var update = await Repository.RemoveItemIdToListAsync(entityId, selector, itemId);
        if (update) await HubService.RemoveItemToListAsync(entityId, selector, itemId);
        return update;
    }

    public Task<long> RemoveItemFromAllListsAsync(string selector, object itemId)
        => Repository.RemoveItemFromAllListsAsync(selector, itemId);

    public Task<long> RemoveItemsAsync(Expression<Func<TEntity, bool>> predicate)
        => Repository.RemoveItemsAsync(predicate);

    public Task<List<TEntity>> GetItemsAsync(Expression<Func<TEntity, bool>> predicate)
        => Repository.GetItemsAsync(predicate);

    public virtual Task InitServiceAsync()
        => Task.CompletedTask;

    public async Task<bool> VerificarPermiso(string idUser, string NamePermiso)
    {
        var database = MongoDBConnection._database;
        if (database == null) throw new Exception("Base de datos null");

        var repUser = database.GetCollection<Usuario>("RepUsuario");
        var filterUsuario = Builders<Usuario>.Filter.Eq(u => u.Id, idUser);
        var usuario = (await repUser.FindAsync(filterUsuario)).FirstOrDefault();

        return await VerificarPermiso(usuario, NamePermiso);
    }

    public async Task<bool> VerificarPermiso(Usuario user, string NamePermiso)
    {
        var database = MongoDBConnection._database;
        if (database == null) return false;

        var repRol = database.GetCollection<Rol>("RepRol");

        if (user != null)
            foreach (var idRol in user.Roles)
            {
                var filtroRol = Builders<Rol>.Filter.Eq(r => r.Id, idRol);
                var rol = (await repRol.FindAsync(filtroRol)).FirstOrDefault();
                if (rol != null && rol.Permisos.Contains(NamePermiso)) return true;
            }

        return false;
    }
}