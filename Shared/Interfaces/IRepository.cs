using MongoDB.Driver;
using Shared.ClassModel;
using Shared.Interfaces.Model;

namespace Shared.Interfaces;

public interface IRepository<TEntity> : ICrud<TEntity>, IUpdateProperty<TEntity> where TEntity : IModelObj
{
    IMongoCollection<TEntity> Collection { get; }
    string NameCollection { get; }
    
    
}