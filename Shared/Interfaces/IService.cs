using Shared.Interfaces.Model;

namespace Shared.Interfaces;

public interface IService<TEntity> : ICrud<TEntity>, IUpdateProperty<TEntity> where TEntity : IModelObj
{
    IRepository<TEntity> Repository { get; }
    IHubService<TEntity> HubService { get; }
}