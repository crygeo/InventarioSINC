using System.Collections;
using System.Collections.ObjectModel;
using System.Net.Http;
using Cliente.Extencions;
using Cliente.ServicesHub;
using Shared.ClassModel;
using Shared.Interfaces.Model;
using Utilidades.Interfaces;

namespace Cliente.Services.Model;

public class ServiceBase<TEntity> : HttpClientBase, IServiceClient<TEntity> where TEntity : class, IModelObj, new()
{
    private IHubService<TEntity>? _hubService;
    public IHubService<TEntity> HubService => _hubService ??= HubServiceFactory.GetHubService<TEntity>();

    public override string BaseUrl { get; } = $"{Config.FullUrl}/{typeof(TEntity).Name}";

    public Dictionary<string, TEntity> CacheById { get; } = [];

    public async Task InitializeAsync()
    {
        await HubService.StartConnectionAsync();

        HubService.OnCreated += OnEntityCreated;
        HubService.OnUpdated += OnEntityUpdated;
        HubService.OnDeleted += OnEntityDeleted;
    }

    public async Task ShutdownAsync()
    {
        HubService.OnCreated -= OnEntityCreated;
        HubService.OnUpdated -= OnEntityUpdated;
        HubService.OnDeleted -= OnEntityDeleted;

        await HubService.StopConnectionAsync();
    }
    //==============================
    // Metodos Get
    //==============================
    public async Task<IResultResponse<IEnumerable<TEntity>>> GetAllAsync()
    {
        var request = await GetRequest<TEntity>(HttpMethod.Get, BaseUrl);
        var result =  await HandleResponseAsync<IEnumerable<TEntity>, TEntity>(request, "Consulta exitosa");
        if (result.Success)
        {
            CacheById.Clear();
            foreach (var entity in result.EntityGet)
            {
                CacheById[entity.Id] = entity;
            }
        }
        return result;
    }

    public async Task<IResultResponse<PagedResult<TEntity>>> GetPagedAsync(int page, int pageSize)
    {
        var uri = $"{BaseUrl}/paged?page={page}&pageSize={pageSize}";
        var request = await GetRequest<PagedResult<TEntity>>(HttpMethod.Get, uri);
        var result = await HandleResponseAsync<PagedResult<TEntity>, PagedResult<TEntity>>(request, "Consulta exitosa");
        if (result.Success)
        {
            foreach (var entity in result.EntityGet.Items)
            {
                CacheById[entity.Id] = entity;
            }
        }
        return result;
    }

    public async Task<IResultResponse<TEntity>> GetByIdAsync(string id)
    {
        if (CacheById.TryGetValue(id, out var cached))
            return ResultResponse<TEntity>.Ok(cached);

        var request = await GetRequest<TEntity>(HttpMethod.Get, $"{BaseUrl}/{id}");
        var result = await HandleResponseAsync<TEntity, TEntity>(request, "Consulta exitosa");

        if (result.Success)
            CacheById[id] = result.EntityGet;

        return result;
    }

    
    //==============================
    // Metodos CUD
    //==============================

    public virtual async Task<IResultResponse<bool>> CreateAsync(TEntity entity)
    {
        var request = await GetRequest<TEntity>(HttpMethod.Post, BaseUrl, entity);
        return await HandleResponseAsync<bool, TEntity>(request, "Creado exitosamente", true);
    }

    public async Task<IResultResponse<bool>> DeleteAsync(string id)
    {
        var request = await GetRequest<TEntity>(HttpMethod.Delete, $"{BaseUrl}/{id}");
        return await HandleResponseAsync<bool, TEntity>(request, "Eliminado exitosamente", true);
    }

    public async Task<IResultResponse<bool>> UpdateAsync(string id, TEntity entity)
    {
        var request = await GetRequest<TEntity>(HttpMethod.Put, $"{BaseUrl}/{id}", entity);
        return await HandleResponseAsync<bool, TEntity>(request, "Actualizado correctamente", true);
    }

    public async Task<IResultResponse<object?>> GetByIdAsyncObj(string id)
    {
        var request = await GetRequest<object?>(HttpMethod.Get, $"{BaseUrl}/{id}");
        return await HandleResponseAsync<object?, object?>(request, "Consulta exitosa");
    }

    public TEntity? GetFromCache(string id)
    {
        return CacheById.TryGetValue(id, out var entity) ? entity : default;
    }

    public object? GetFromCacheObj(string id)
    {
        return GetFromCache(id);
    }

    public IEnumerable GetAllFromCacheObj()
    {
        return CacheById.Values;
    }

    public IEnumerable<TEntity> GetRangeIdCache(IList<string> ids)
    {
        var result = new List<TEntity>();

        foreach (var id in ids)
            result.Add(GetFromCache(id)!);

        return result;
    }

    // Métodos para aplicar cambios desde el Hub
    public void ApplyCreated(TEntity entity)
        => CacheById[entity.Id] = entity;

    public void ApplyUpdated(TEntity entity)
        => CacheById[entity.Id] = entity;

    public void ApplyDeleted(string id)
        => CacheById.Remove(id);

    // Eventos de Hub
    protected virtual void OnEntityCreated(TEntity entity) => ApplyCreated(entity);
    protected virtual void OnEntityUpdated(TEntity entity) => ApplyUpdated(entity);
    protected virtual void OnEntityDeleted(string id) => ApplyDeleted(id);
}

public class ResultResponse<TEntity> : IResultResponse<TEntity>, IResultResponse
{
    public string GetErrorFormat()
    {
        var nombreObjeto = ObjInteration?.Name ?? "Desconocido";

        var detalles = string.IsNullOrWhiteSpace(Error)
            ? "Sin detalles disponibles."
            : string.Join("\n\t", Error.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));

        return $"Entity relacionado: {nombreObjeto}\n\nDetalles del error:\n\t{detalles}";
    }

    public static IResultResponse<TEntity> Fail(string error)
    {
        return new ResultResponse<TEntity>()
        {
            Error = error,
            Success = false,
            Message = error
        };
    }
    
    public static IResultResponse<TEntity> Ok(TEntity entity)
    {
        return new ResultResponse<TEntity>()
        {
            EntityGet = entity,
            Success = true,
            Message = "Operación exitosa"
        };
    }

    public TEntity EntityGet { get; init; } = default!;
    public Type? ObjInteration { get; set; }

    public HttpMethod? Method { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
    

    public override string ToString()
    {
        return GetErrorFormat();
    }
}