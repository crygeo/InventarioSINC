using System.Collections;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using Cliente.Extencions;
using Cliente.ServicesHub;
using Shared.ClassModel;
using Shared.Interfaces.Model;
using Shared.Request;
using Utilidades.Interfaces;

namespace Cliente.Services.Model;

public class ServiceBase<TEntity> : HttpClientBase, IServiceClient<TEntity> where TEntity : class, IModelObj, new()
{
    protected IHubService<TEntity> HubService = HubServiceFactory.GetHubService<TEntity>();

    public override string BaseUrl { get; } = $"{Config.FullUrl}/{typeof(TEntity).Name}";

    public Dictionary<string, TEntity> CacheById { get; } = [];
    public event Action<EntityChangeType, string, TEntity?>? CollectionChanged;


    public async Task InitializeAsync()
    {
        await HubService.StartConnectionAsync();

        HubService.OnCreated += OnEntityCreated;
        HubService.OnUpdated += OnEntityUpdated;
        HubService.OnDeleted += OnEntityDeleted;

        HubService.OnPropertyUpdated += OnPropertyUpdated;
        HubService.OnItemAddedToList += OnItemAddedToList;
        HubService.OnItemRemovedFromList += OnItemRemovedToList;
    }

    public async Task ShutdownAsync()
    {
        HubService.OnCreated -= OnEntityCreated;
        HubService.OnUpdated -= OnEntityUpdated;
        HubService.OnDeleted -= OnEntityDeleted;

        HubService.OnPropertyUpdated -= OnPropertyUpdated;
        HubService.OnItemAddedToList -= OnItemAddedToList;
        HubService.OnItemRemovedFromList -= OnItemRemovedToList;

        //await HubService.StopConnectionAsync();
    }

    //==============================
    // Metodos Get
    //==============================
    public async Task<IResultResponse<IEnumerable<TEntity>>> GetAllAsync()
    {
        var request = await GetRequest<TEntity>(HttpMethod.Get, BaseUrl);
        var result = await HandleResponseAsync<IEnumerable<TEntity>, TEntity>(request, "Consulta exitosa");
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

    public async Task<IResultResponse<object?>> GetByIdAsyncObj(string id)
    {
        var request = await GetRequest<object?>(HttpMethod.Get, $"{BaseUrl}/{id}");
        return await HandleResponseAsync<object?, object?>(request, "Consulta exitosa");
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

    public async Task<IResultResponse<bool>> PropertyUpdateAsync(string idEntity, string nameProperty, object newValue)
    {
        var data = GetData(idEntity, nameProperty, newValue);
        var request = await GetRequest<TEntity>(HttpMethod.Put, $"{BaseUrl}/property/update", data);
        return await HandleResponseAsync<bool, TEntity>(request, "Actualizado correctamente", true);
    }

    public async Task<IResultResponse<bool>> ItemAddedToListAsync(string idEntity, string nameProperty, object newValue)
    {
        var data = GetData(idEntity, nameProperty, newValue);
        var request = await GetRequest<TEntity>(HttpMethod.Put, $"{BaseUrl}/property/update", data);
        return await HandleResponseAsync<bool, TEntity>(request, "Actualizado correctamente", true);
    }

    public async Task<IResultResponse<bool>> ItemRemovedToListAsync(string idEntity, string nameProperty, object newValue)
    {
        var data = GetData(idEntity, nameProperty, newValue);
        var request = await GetRequest<TEntity>(HttpMethod.Put, $"{BaseUrl}/property/update", data);
        return await HandleResponseAsync<bool, TEntity>(request, "Actualizado correctamente", true);
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
    {
        CacheById[entity.Id] = entity;
        NotifyChange(EntityChangeType.Created, entity.Id, entity);
    }

    public void ApplyUpdated(TEntity entity)
    {
        CacheById[entity.Id] = entity;
        NotifyChange(EntityChangeType.Updated, entity.Id, entity);
    }

    public void ApplyDeleted(string id)
    {
        CacheById.Remove(id);
        NotifyChange(EntityChangeType.Deleted, id, null);
    }

    public void AplyPropertyUpdated(string idEntity, string nameProperty, object newValue)
    {
        if (!CacheById.TryGetValue(idEntity, out var itemCache))
            return;

        var property = GetPropertyIfExists(itemCache.GetType(), nameProperty);
        if (property == null)
            return;

        // No permitir colecciones aquí
        if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType)
            && property.PropertyType != typeof(string))
            return;

        TrySetPropertyValue(itemCache, property, newValue);
    }

    public void AplyItemAddedToList(string idEntity, string nameProperty, object item)
    {
        if (!CacheById.TryGetValue(idEntity, out var itemCache))
            return;

        var property = GetPropertyIfExists(itemCache.GetType(), nameProperty);
        if (property == null)
            return;

        if (!TryGetCollectionItemType(property, out var itemType))
            return;

        if (!itemType.IsInstanceOfType(item))
            return;

        var collection = property.GetValue(itemCache) as IList;
        if (collection == null)
            return;

        // Evitar duplicados
        if (collection.Contains(item))
            return;

        collection.Add(item);
    }

    public void AplyItemRemovedToList(string idEntity, string nameProperty, object item)
    {
        if (!CacheById.TryGetValue(idEntity, out var itemCache))
            return;

        var property = GetPropertyIfExists(itemCache.GetType(), nameProperty);
        if (property == null)
            return;

        if (!TryGetCollectionItemType(property, out var itemType))
            return;

        if (!itemType.IsInstanceOfType(item))
            return;

        var collection = property.GetValue(itemCache) as IList;
        if (collection == null)
            return;

        if (!collection.Contains(item))
            return;

        collection.Remove(item);
    }


    // Eventos de Hub
    protected virtual void OnEntityCreated(TEntity entity) => ApplyCreated(entity);
    protected virtual void OnEntityUpdated(TEntity entity) => ApplyUpdated(entity);
    protected virtual void OnEntityDeleted(string id) => ApplyDeleted(id);

    protected virtual void OnPropertyUpdated(PropertyChangedEventRequest request) =>
        AplyPropertyUpdated(request.EntityId, request.Selector, request.NewValue);

    protected virtual void OnItemAddedToList(PropertyChangedEventRequest request) =>
        AplyItemAddedToList(request.EntityId, request.Selector, request.NewValue);

    protected virtual void OnItemRemovedToList(PropertyChangedEventRequest request) =>
        AplyItemRemovedToList(request.EntityId, request.Selector, request.NewValue);

    private static PropertyInfo? GetPropertyIfExists(Type type, string propertyName)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
            return null;

        return type.GetProperty(
            propertyName,
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
    }

    private static bool TrySetPropertyValue(object target, PropertyInfo property, object? rawValue)
    {
        var targetType = Nullable.GetUnderlyingType(property.PropertyType)
                         ?? property.PropertyType;

        // 🔽 Conversión genérica (JsonElement incluido)
        if (!TryConvertValue(rawValue, targetType, out var converted))
            return false;

        // null válido
        if (converted == null)
        {
            if (property.PropertyType.IsValueType &&
                Nullable.GetUnderlyingType(property.PropertyType) == null)
                return false;

            property.SetValue(target, null);
            return true;
        }

        property.SetValue(target, converted);
        return true;
    }
    private static bool TryConvertValue(object? value, Type targetType, out object? result)
    {
        result = null;

        // null siempre es válido (la validación de nullable va afuera)
        if (value == null)
            return true;

        // Ya es del tipo correcto
        if (targetType.IsInstanceOfType(value))
        {
            result = value;
            return true;
        }

        // Caso normal cuando viene de [FromBody] object
        if (value is JsonElement json)
        {
            if (json.ValueKind == JsonValueKind.Null)
                return true;

            try
            {
                result = JsonSerializer.Deserialize(
                    json.GetRawText(),
                    targetType,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                return true;
            }
            catch
            {
                return false;
            }
        }

        // Fallback para conversiones simples (int -> long, etc.)
        try
        {
            result = Convert.ChangeType(value, targetType);
            return true;
        }
        catch
        {
            return false;
        }
    }
    private static bool TryGetCollectionItemType(PropertyInfo property, out Type itemType)
    {
        itemType = null!;

        if (!typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
            return false;

        if (!property.PropertyType.IsGenericType)
            return false;

        itemType = property.PropertyType.GetGenericArguments()[0];
        return true;
    }

    private PropertyChangedEventRequest GetData(string id, string nameProperty, object newValue)
    {
        return new PropertyChangedEventRequest
        {
            EntityId = id,
            Selector = nameProperty,
            NewValue = newValue
        };
    }
    
    private CancellationTokenSource? _debounceCts;

    private void NotifyChange(EntityChangeType type, string id, TEntity entity)
    {
        _debounceCts?.Cancel();
        _debounceCts = new CancellationTokenSource();

        var token = _debounceCts.Token;

        Application.Current.Dispatcher.InvokeAsync(async () =>
        {
            await Task.Delay(200, token);
            if (!token.IsCancellationRequested)
                CollectionChanged?.Invoke(type, id, entity);
        });
    }


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
public enum EntityChangeType
{
    Created,
    Updated,
    Deleted
}