using MongoDB.Bson;
using MongoDB.Driver;
using Servidor.src.Objs;
using Servidor.src.Repositorios;
using Shared.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

public abstract class ServiceBase<TObj> : IService<TObj> where TObj : IIdentifiable
{
    public abstract IRepository<TObj> Repository { get; }
    public abstract IHubService<TObj> HubService { get; }

    public virtual Task InitServiceAsync()
    {
        return Task.CompletedTask;
    }

    public async Task<bool> CreateAsync(TObj entity)
    {
        if (await Repository.CreateAsync(entity))
        {
            await HubService.NewItem(entity);
            return true;
        }
        return false;
    }

    public async Task<bool> UpdateAsync(string id, TObj entity)
    {
        if (await Repository.UpdateAsync(id, entity))
        {
            await HubService.UpdateItem(entity);
            return true;
        }
        return false;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var entity = await Repository.GetByIdAsync(id);
        if (entity != null && await Repository.DeleteAsync(id))
        {
            await HubService.DeleteItem(entity);
            return true;
        }
        return false;
    }

    public Task<IEnumerable<TObj>> GetAllAsync()
    {
        return Repository.GetAllAsync();
    }

    public Task<TObj?> GetByIdAsync(string id)
    {
        return Repository.GetByIdAsync(id);
    }

    public async Task<bool> VerificarPermiso(string idUser, string NamePermiso)
    {
        var database = MongoDBConnection._database;
        if(database == null) return false;

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
        {
            foreach (var idRol in user.Roles)
            {
                var filtroRol = Builders<Rol>.Filter.Eq(r => r.Id, idRol);
                var rol = (await repRol.FindAsync(filtroRol)).FirstOrDefault();
                if (rol != null && rol.Permisos.Contains(NamePermiso))
                {
                    return true;
                }
            }
        }

        return false;
    }

}
