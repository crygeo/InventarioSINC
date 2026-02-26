using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Servidor.Helper;
using Servidor.Services;
using Shared.Interfaces;
using Shared.Interfaces.Model;
using Shared.ObjectsResponse;
using Shared.Request;

namespace Servidor.Controllers;

[ApiController]
public class BaseController<TEntity> : ControllerBase, IController<TEntity, IActionResult>
    where TEntity : class, IModelObj
{
    private ServiceBase<TEntity>? _service;

    public IService<TEntity> Service =>
        _service ??= ServiceFactory.GetService<TEntity>();

    public string NamePermiso =>
        $"{ControllerContext.ActionDescriptor.ControllerName}.{ControllerContext.ActionDescriptor.ActionName}";

    // =========================
    // CRUD
    // =========================

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
        => await ExecuteSafeAsync(async () =>
        {
            var auth = await ValidateUserAsync();
            if (auth != null) return auth;

            return Ok(await Service.GetAllAsync());
        }, "Error no controlado al obtener los objetos de la base de datos.");

    [HttpGet("paged")]
    public async Task<IActionResult> GetPagedAsync([FromQuery] int page = 0, [FromQuery] int pageSize = 50)
        => await ExecuteSafeAsync(async () =>
        {
            var auth = await ValidateUserAsync();
            if (auth != null) return auth;

            return Ok(await Service.GetPagedAsync(page, pageSize));
        }, "Error no controlado al obtener los objetos de la base de datos.");

    [HttpGet("{id:length(24)}")]
    public async Task<IActionResult> GetByIdAsync(string id)
        => await ExecuteSafeAsync(async () =>
        {
            var auth = await ValidateUserAsync();
            if (auth != null) return auth;

            var idError = ValidateEntityId(id);
            if (idError != null) return idError;

            var entity = await Service.GetByIdAsync(id);
            return entity != null
                ? Ok(entity)
                : NotFound(new ErrorResponse(404, "El objeto no fue encontrado."));
        }, "Error no controlado al obtener el objeto de la base de datos.");

    [HttpPost]
    public virtual async Task<IActionResult> CreateAsync([FromBody] TEntity entity)
        => await ExecuteSafeAsync(async () =>
        {
            var auth = await ValidateUserAsync();
            if (auth != null) return auth;

            if (entity == null)
                return BadRequest(new ErrorResponse(400, "El objeto no puede ser nulo."));

            entity.Deleteable = true;

            return await Service.CreateAsync(entity)
                ? Ok()
                : StatusCode(500, new ErrorResponse(500, "Error al crear el objeto en la base de datos."));
        }, "Error no controlado al crear el objeto en la base de datos.");

    [HttpPut("{id:length(24)}")]
    public virtual async Task<IActionResult> UpdateAsync(string id, [FromBody] TEntity entity)
        => await ExecuteSafeAsync(async () =>
        {
            var auth = await ValidateUserAsync();
            if (auth != null) return auth;

            var idError = ValidateEntityId(id);
            if (idError != null) return idError;

            if (entity == null)
                return BadRequest(new ErrorResponse(400, "El objeto no puede ser nulo."));

            if (await Service.GetByIdAsync(id) == null)
                return NotFound(new ErrorResponse(404, "El objeto no fue encontrado."));

            await Service.UpdateAsync(id, entity);
            return NoContent();
        }, "Error no controlado al actualizar el objeto en la base de datos.");

    [HttpDelete("{childId:length(24)}")]
    public virtual async Task<IActionResult> DeleteAsync(string childId)
        => await ExecuteSafeAsync(async () =>
        {
            var auth = await ValidateUserAsync();
            if (auth != null) return auth;

            var idError = ValidateEntityId(childId);
            if (idError != null) return idError;

            return await Service.DeleteAsync(childId)
                ? NoContent()
                : NotFound(new ErrorResponse(404, "El objeto no fue encontrado para eliminar."));
        }, "Error no controlado al eliminar el objeto de la base de datos.");

    // =========================
    // PROPERTY COMMANDS
    // =========================

    [HttpPut("property/update")]
    public async Task<IActionResult> UpdateProperty([FromBody] PropertyChangedEventRequest request)
        => await ExecutePropertyCommandAsync(
            request,
            () =>
            {
                if (!IsValueCompatibleWithProperty(
                        request.Selector,
                        request.NewValue,
                        out var convertedValue))
                {
                    return Task.FromResult<IActionResult>(
                        BadRequest(new ErrorResponse(
                            400,
                            "El nuevo valor no es compatible con el tipo de la propiedad.")));
                }

                return ExecutePropertyActionAsync(
                    () => Service.UpdateProperty(
                        request.EntityId,
                        request.Selector,
                        convertedValue),
                    "El objeto no fue encontrado para actualizar.");
            });

    [HttpPut("property/add")]
    public async Task<IActionResult> AddItemToListAsync([FromBody] PropertyChangedEventRequest request)
        => await ExecutePropertyCommandAsync(
            request,
            () =>
            {
                if (!IsNewValueCompatibleWithProperty(request.Selector, request.NewValue))
                    return Task.FromResult<IActionResult>(
                        BadRequest(new ErrorResponse(400,
                            "El valor para agregar no es compatible con el tipo de la propiedad.")));

                return ExecutePropertyActionAsync(
                    () => Service.AddItemIdToListAsync(request.EntityId, request.Selector, request.NewValue),
                    "El objeto no fue encontrado para modificar la lista.");
            });

    [HttpPut("property/remove")]
    public async Task<IActionResult> RemoveItemFromListAsync([FromBody] PropertyChangedEventRequest request)
        => await ExecutePropertyCommandAsync(
            request,
            () =>
            {
                if (!IsNewValueCompatibleWithProperty(request.Selector, request.NewValue))
                    return Task.FromResult<IActionResult>(
                        BadRequest(new ErrorResponse(400,
                            "El valor a remover no es compatible con el tipo de la propiedad.")));

                return ExecutePropertyActionAsync(
                    () => Service.RemoveItemIdToListAsync(request.EntityId, request.Selector, request.NewValue),
                    "El objeto no fue encontrado para remover el elemento.");
            });

    // =========================
    // HELPERS
    // =========================

    protected async Task<IActionResult?> ValidateUserAsync()
    {
        var userId = User.Claims.FirstOrDefault()?.Value ?? "";

        if (!IdValidator.IsValidObjectId(userId))
            return BadRequest(new ErrorResponse(400, "El usuario del solicitante no es válido."));

        if (!await ((ServiceBase<TEntity>)Service).VerificarPermiso(userId, NamePermiso))
            return Unauthorized(new ErrorResponse(401, "Permisos insuficientes."));

        return null;
    }

    protected IActionResult? ValidateEntityId(string id)
        => !IdValidator.IsValidObjectId(id)
            ? BadRequest(new ErrorResponse(400, "El ID proporcionado no es válido."))
            : null;

    protected async Task<IActionResult> ExecuteSafeAsync(Func<Task<IActionResult>> action, string errorMessage)
    {
        try
        {
            return await action();
        }
        catch (MongoException ex)
        {
            return StatusCode(500,
                new ErrorResponse(500, errorMessage, ex.Message));
        }
    }

    protected async Task<IActionResult> ExecutePropertyCommandAsync(PropertyChangedEventRequest request,
        Func<Task<IActionResult>> action)
    {
        var auth = await ValidateUserAsync();
        if (auth != null) return auth;

        if (!IdValidator.IsValidObjectId(request.EntityId))
            return BadRequest(new ErrorResponse(400, "El ID proporcionado no es válido."));

        if (!HasProperty(request.Selector))
            return BadRequest(new ErrorResponse(400, "La propiedad proporcionada no existe en la entidad."));

        return await action();
    }

    protected async Task<IActionResult> ExecutePropertyActionAsync(Func<Task<bool>> action, string notFoundMessage)
    {
        try
        {
            return await action()
                ? NoContent()
                : NotFound(new ErrorResponse(404, notFoundMessage));
        }
        catch (MongoException ex)
        {
            return StatusCode(500,
                new ErrorResponse(500, "Error al modificar la propiedad.", ex.Message));
        }
    }

    // =========================
    // REFLECTION VALIDATORS
    // =========================

    private static bool HasProperty(string propertyName)
        => !string.IsNullOrWhiteSpace(propertyName)
           && typeof(TEntity).GetProperty(propertyName,
               BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase) != null;

    protected static bool IsNewValueCompatibleWithProperty(string propertyName, object newValue)
    {
        if (newValue is null) return false;

        var property = typeof(TEntity).GetProperty(
            propertyName,
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

        if (property == null || !typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
            return false;

        if (!property.PropertyType.IsGenericType)
            return false;

        var itemType = property.PropertyType.GetGenericArguments()[0];
        return itemType.IsInstanceOfType(newValue);
    }

    protected static bool IsValueCompatibleWithProperty(
        string propertyName,
        object? rawValue,
        out object? convertedValue)
    {
        convertedValue = null;

        var property = typeof(TEntity).GetProperty(
            propertyName,
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

        if (property == null)
            return false;

        if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType)
            && property.PropertyType != typeof(string))
            return false;

        var targetType = Nullable.GetUnderlyingType(property.PropertyType)
                         ?? property.PropertyType;

        return TryConvertValue(rawValue, targetType, out convertedValue);
    }

    protected static bool TryConvertValue(object? value, Type targetType, out object? result)
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
}