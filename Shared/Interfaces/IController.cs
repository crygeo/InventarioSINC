using System.Linq.Expressions;
using Shared.Interfaces.Model;

namespace Shared.Interfaces;

public interface IController<TEntity, IResult> where TEntity : IModelObj
{
    IService<TEntity> Service { get; }

    Task<IResult> GetAllAsync();
    Task<IResult> GetByIdAsync(string id);
    Task<IResult> CreateAsync(TEntity entity);
    Task<IResult> UpdateAsync(string id, TEntity entity);
    Task<IResult> DeleteAsync(string id);
    Task<IResult> UpdateProperty(string entityId, string selector, object newValue);
}