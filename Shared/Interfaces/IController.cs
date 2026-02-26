using System.Linq.Expressions;
using Shared.Interfaces.Model;
using Shared.Request;

namespace Shared.Interfaces;

public interface IController<TEntity, IResult> where TEntity : IModelObj
{
    IService<TEntity> Service { get; }

    Task<IResult> GetAllAsync();
    Task<IResult> GetByIdAsync(string id);
    Task<IResult> CreateAsync(TEntity entity);
    Task<IResult> UpdateAsync(string id, TEntity entity);
    Task<IResult> DeleteAsync(string childId);
    Task<IResult> UpdateProperty(PropertyChangedEventRequest request);
    Task<IResult> AddItemToListAsync(PropertyChangedEventRequest request);
    Task<IResult> RemoveItemFromListAsync(PropertyChangedEventRequest request);
}