using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Shared.ClassModel;
using Shared.Interfaces.Model;
using Shared.Request;

namespace Utilidades.Interfaces;

public interface IServiceClient<TEntity> : IServiceClient where TEntity : class, IModelObj, new()

{
    string BaseUrl { get; }
    
    Dictionary<string, TEntity> CacheById { get; }
    
    Task InitializeAsync();
    Task ShutdownAsync();
    
    Task<IResultResponse<IEnumerable<TEntity>>> GetAllAsync();
    Task<IResultResponse<PagedResult<TEntity>>> GetPagedAsync(int page, int pageSize);
    Task<IResultResponse<TEntity>> GetByIdAsync(string id);
    Task<IResultResponse<bool>> CreateAsync(TEntity entity);
    Task<IResultResponse<bool>> UpdateAsync(string id, TEntity entity);
    Task<IResultResponse<bool>> DeleteAsync(string id);

        
    Task<IResultResponse<bool>> PropertyUpdateAsync(string idEntity, string nameProperty, object newValue);
    Task<IResultResponse<bool>> ItemAddedToListAsync(string idEntity, string nameProperty, object newValue);
    Task<IResultResponse<bool>> ItemRemovedToListAsync(string idEntity, string nameProperty, object newValue);
    
    
    void ApplyCreated(TEntity entity);
    void ApplyUpdated(TEntity entity);
    void ApplyDeleted(string id);
    void AplyPropertyUpdated(string idEntity, string nameProperty, object newValue);
    void AplyItemAddedToList(string idEntity, string nameProperty, object item);
    void AplyItemRemovedToList(string idEntity, string nameProperty, object item);
    
    TEntity? GetFromCache(string id);

    Task<IResultResponse<IEnumerable<TEntity>>> SearchAsync(SearchRequest request);

}

public interface IServiceClient
{
    Task<IResultResponse<object?>> GetByIdAsync(string id);
    Task<IResultResponse<PagedResult>> GetPagedAsync(int page, int pageSize);
    Task<IResultResponse<IEnumerable>> SearchAsync(SearchRequest request);
    object? GetFromCacheObj(string id);
    IEnumerable GetAllFromCacheObj();
    
    

}