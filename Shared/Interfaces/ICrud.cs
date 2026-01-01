using Shared.ClassModel;
using Shared.Interfaces.Model;

namespace Shared.Interfaces;

public interface ICrud<TEntity> where TEntity : IModelObj
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(string id);
    Task<bool> CreateAsync(TEntity entity);
    Task<bool> UpdateAsync(string id, TEntity entity);
    Task<bool> DeleteAsync(string id);
    Task<PagedResult<TEntity>> GetPagedAsync(int page, int pageSize);

}