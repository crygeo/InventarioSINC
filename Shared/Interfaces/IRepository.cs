using MongoDB.Driver;
using Shared.ClassModel;
using Shared.Interfaces.Model;
using Shared.Request;

namespace Shared.Interfaces;

public interface IRepository<TEntity> : ICrud<TEntity>, IUpdateProperty<TEntity> where TEntity : IModelObj
{
    IMongoCollection<TEntity> Collection { get; }
    string NameCollection { get; }

    Task<IEnumerable<TEntity>> SearchAsync(SearchRequest request);
}