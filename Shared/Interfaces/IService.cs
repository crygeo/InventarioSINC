using Shared.Interfaces.Model;
using Shared.Request;

namespace Shared.Interfaces;

public interface IService<TEntity> : ICrud<TEntity>, IUpdateProperty<TEntity> where TEntity : IModelObj
{
    IRepository<TEntity> Repository { get; }
    IHubService<TEntity> HubService { get; }

    Task<IEnumerable<TEntity>> SearchAsync(SearchRequest request);
}