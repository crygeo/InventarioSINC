using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Servidor.Helper;
using Servidor.Services;
using Shared.Interfaces;
using Shared.Interfaces.Model;
using Shared.ObjectsResponse;

namespace Servidor.Controllers;

[ApiController]
public class BaseController<TEntity> : ControllerBase, IController<TEntity, IActionResult>
    where TEntity : class, IModelObj
{
    private ServiceBase<TEntity>? _service;

    public string NamePermiso =>
        $"{ControllerContext.ActionDescriptor.ControllerName}.{ControllerContext.ActionDescriptor.ActionName}";

    public IService<TEntity> Service => _service ??= ServiceFactory.GetService<TEntity>();


    [HttpGet]
    [ActionName("Consultar")]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var validacion = await ValidarUsuarioAsync();
            if (validacion != null) return validacion;

            var objetos = await Service.GetAllAsync();
            return Ok(objetos);
        }
        catch (MongoException ex)
        {
            return StatusCode(500,
                new ErrorResponse(500, "Error al obtener los objetos de la base de datos.", ex.Message));
        }
    }
    
    [HttpGet("paged")]
    [ActionName("Consultar")]
    public async Task<IActionResult> GetPagedAsync([FromQuery] int page = 0, [FromQuery] int pageSize = 50)
    {
        try
        {
            var validacion = await ValidarUsuarioAsync();
            if (validacion != null) return validacion;

            var objetos = await Service.GetPagedAsync(page, pageSize);
            return Ok(objetos);
        }
        catch (MongoException ex)
        {
            return StatusCode(500,
                new ErrorResponse(500, "Error al obtener los objetos de la base de datos.", ex.Message));
        }
    }

    [HttpGet("{id:length(24)}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        try
        {
            var validacion = await ValidarUsuarioAsync();
            if (validacion != null) return validacion;

            if (!IdValidator.IsValidObjectId(id))
                return BadRequest(new ErrorResponse(400, "El ID proporcionado no es válido."));

            var objeto = await Service.GetByIdAsync(id);
            return objeto != null
                ? Ok(objeto)
                : NotFound(new ErrorResponse(404, "El objeto no fue encontrado."));
        }
        catch (MongoException ex)
        {
            return StatusCode(500,
                new ErrorResponse(500, "Error al obtener el objeto de la base de datos.", ex.Message));
        }
    }

    [HttpPost]
    [ActionName("Agregar")]
    public virtual async Task<IActionResult> CreateAsync(TEntity objeto)
    {
        try
        {
            var validacion = await ValidarUsuarioAsync();
            if (validacion != null) return validacion;

            if (objeto == null)
                return BadRequest(new ErrorResponse(400, "El objeto no puede ser nulo."));

            objeto.Deleteable = true;
            if (await Service.CreateAsync(objeto))
                return Ok();
            return StatusCode(500, new ErrorResponse(500, "Error al crear el objeto en la base de datos."));
        }
        catch (MongoException ex)
        {
            return StatusCode(500,
                new ErrorResponse(500, "Error al crear el objeto en la base de datos.", ex.Message));
        }
    }

    [HttpPut("{id:length(24)}")]
    [ActionName("Editar")]
    public async Task<IActionResult> UpdateAsync(string id, [FromBody] TEntity objetoIn)
    {
        try
        {
            var validacion = await ValidarUsuarioAsync();
            if (validacion != null) return validacion;

            if (!IdValidator.IsValidObjectId(id))
                return BadRequest(new ErrorResponse(400, "El ID proporcionado no es válido."));

            if (objetoIn == null)
                return BadRequest(new ErrorResponse(400, "El objeto no puede ser nulo."));

            var existente = await Service.GetByIdAsync(id);
            if (existente == null)
                return NotFound(new ErrorResponse(404, "El objeto no fue encontrado."));

            await Service.UpdateAsync(id, objetoIn);
            return NoContent();
        }
        catch (MongoException ex)
        {
            return StatusCode(500,
                new ErrorResponse(500, "Error al actualizar el objeto en la base de datos.", ex.Message));
        }
    }

    [HttpDelete("{id:length(24)}")]
    [ActionName("Eliminar")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        try
        {
            var validacion = await ValidarUsuarioAsync();
            if (validacion != null) return validacion;

            if (!IdValidator.IsValidObjectId(id))
                return BadRequest(new ErrorResponse(400, "El ID proporcionado no es válido."));

            var result = await Service.DeleteAsync(id);
            return result
                ? NoContent()
                : NotFound(new ErrorResponse(404, "El objeto no fue encontrado para eliminar."));
        }
        catch (MongoException ex)
        {
            return StatusCode(500,
                new ErrorResponse(500, "Error al eliminar el objeto de la base de datos.", ex.Message));
        }
    }

    [HttpPost("property/update")]
    [ActionName("UpdateProperty")]
    public Task<IActionResult> UpdateProperty(string entityId, string selector, object newValue)
    {
        throw new System.NotImplementedException();
    }

    public async Task<IActionResult?> ValidarUsuarioAsync()
    {
        var userId = User.Claims.FirstOrDefault()?.Value ?? "";

        if (!IdValidator.IsValidObjectId(userId))
            return BadRequest(new ErrorResponse(400, "El usuario del solicitante no es válido."));


        if (!await ((ServiceBase<TEntity>)Service).VerificarPermiso(userId, NamePermiso))
            return Unauthorized(new ErrorResponse(401, "Permisos insuficientes."));

        return null;
    }
}