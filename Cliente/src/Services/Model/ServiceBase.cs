using System.Collections.ObjectModel;
using System.Net.Http;
using Cliente.Extencions;
using Cliente.src.ServicesHub;
using Shared.Interfaces.Model;
using Utilidades.Interfaces;

namespace Cliente.Services.Model;

public class ServiceBase<TEntity> : HttpClientBase, IServiceClient<TEntity> where TEntity : class, IModelObj, new()
{
    private IHubService<TEntity>? _hubService;
    public IHubService<TEntity> HubService => _hubService ??= HubServiceFactory.GetHubService<TEntity>();

    public ObservableCollection<TEntity> Collection => HubService.Collection;

    public override string BaseUrl { get; } = $"{Config.FullUrl}/{typeof(TEntity).Name}";

    public ServiceBase()
    {
    }

    public virtual async Task<IResultResponse<bool>> InitAsync()
    {
        await HubService.StartConnectionAsync();
        Collection.Clear();

        var result = await GetAllAsync();
        if (!result.Success || result.EntityGet == null)
            return ResultError<bool>(result.Message, result.Error);

        Collection.ReplaceWith(result.EntityGet);

        return ResultSuccess(true, "Colección inicializada");
    }

    public virtual async Task<IResultResponse<bool>> StopAsync()
    {
        Collection.Clear();
        await HubService.StopConnectionAsync();
        return ResultSuccess(true, "Conexión detenida");
    }

    public virtual async Task<IResultResponse<bool>> UpdateCollection()
    {
        var result = await GetAllAsync();
        if (!result.Success || result.EntityGet == null)
            return ResultError<bool>(result.Message, result.Error);

        Collection.ReplaceWith(result.EntityGet);

        return ResultSuccess(true, "Colección actualizada");
    }

    public async Task<IResultResponse<IEnumerable<TEntity>>> GetAllAsync()
    {
        var request = await GetRequest<TEntity>(HttpMethod.Get, BaseUrl);
        return await HandleResponseAsync<IEnumerable<TEntity>, TEntity>(request, "Consulta exitosa");
    }

    public async Task<IResultResponse<TEntity>> GetByIdAsync(string id)
    {
        var request = await GetRequest<TEntity>(HttpMethod.Get, $"{BaseUrl}/{id}");
        return await HandleResponseAsync<TEntity, TEntity>(request, "Consulta exitosa");
    }

    public TEntity GetById(string id)
    {
        return Collection.First(a => a.Id == id);
    }

    public async Task<IResultResponse<bool>> CreateAsync(TEntity entity)
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

        

       

       
}


public class ResultResponse<TObj> : IResultResponse<TObj>, IResultResponse
{
    public TObj EntityGet { get; init; } = default!;
    public Type? ObjInteration { get; set; }

    public HttpMethod? Method { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;

    public override string ToString()
    {
        return GetErrorFormat();
    }

    public string GetErrorFormat()
    {
        var nombreObjeto = ObjInteration?.Name ?? "Desconocido";

        var detalles = string.IsNullOrWhiteSpace(Error)
            ? "Sin detalles disponibles."
            : string.Join("\n\t", Error.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));

        return $"Objeto relacionado: {nombreObjeto}\n\nDetalles del error:\n\t{detalles}";
    }

}