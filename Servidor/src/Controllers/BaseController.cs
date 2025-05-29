using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Servidor.src.Helper;
using Servidor.src.Services;
using Shared.Interfaces;
using Shared.ObjectsResponse;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.src.Controllers
{
    [ApiController]
    public abstract class BaseController<TObj> : ControllerBase where TObj : IIdentifiable, IDeleteable
    {
        public readonly ServiceBase<TObj> _service;

        public string NamePermiso => $"{ControllerContext.ActionDescriptor.ControllerName}.{ControllerContext.ActionDescriptor.ActionName}";

        public BaseController(ServiceBase<TObj> service)
        {
            _service = service;
        }

        [HttpGet]
        [ActionName("Consultar")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var validacion = await ValidarUsuarioAsync();
                if (validacion != null) return validacion;

                var objetos = await _service.GetAllAsync();
                return Ok(objetos);
            }
            catch (MongoException ex)
            {
                return StatusCode(500, new ErrorResponse(500, "Error al obtener los objetos de la base de datos.", ex.Message));
            }
        }

        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var validacion = await ValidarUsuarioAsync();
                if (validacion != null) return validacion;

                if (!IdValidator.IsValidObjectId(id))
                    return BadRequest(new ErrorResponse(400, "El ID proporcionado no es válido."));

                var objeto = await _service.GetByIdAsync(id);
                return objeto != null
                    ? Ok(objeto)
                    : NotFound(new ErrorResponse(404, "El objeto no fue encontrado."));
            }
            catch (MongoException ex)
            {
                return StatusCode(500, new ErrorResponse(500, "Error al obtener el objeto de la base de datos.", ex.Message));
            }
        }

        [HttpPost]
        [ActionName("Agregar")]
        public async Task<IActionResult> Create(TObj objeto)
        {
            try
            {
                var validacion = await ValidarUsuarioAsync();
                if (validacion != null) return validacion;

                if (objeto == null)
                    return BadRequest(new ErrorResponse(400, "El objeto no puede ser nulo."));

                objeto.Deleteable = true;
                if (await _service.CreateAsync(objeto))
                    return Ok();
                else
                    return StatusCode(500, new ErrorResponse(500, "Error al crear el objeto en la base de datos."));
            }
            catch (MongoException ex)
            {
                return StatusCode(500, new ErrorResponse(500, "Error al crear el objeto en la base de datos.", ex.Message));
            }
        }

        [HttpPut("{id:length(24)}")]
        [ActionName("Editar")]
        public async Task<IActionResult> Update(string id, [FromBody] TObj objetoIn)
        {
            try
            {
                var validacion = await ValidarUsuarioAsync();
                if (validacion != null) return validacion;

                if (!IdValidator.IsValidObjectId(id))
                    return BadRequest(new ErrorResponse(400, "El ID proporcionado no es válido."));

                if (objetoIn == null)
                    return BadRequest(new ErrorResponse(400, "El objeto no puede ser nulo."));

                var existente = await _service.GetByIdAsync(id);
                if (existente == null)
                    return NotFound(new ErrorResponse(404, "El objeto no fue encontrado."));

                await _service.UpdateAsync(id, objetoIn);
                return NoContent();
            }
            catch (MongoException ex)
            {
                return StatusCode(500, new ErrorResponse(500, "Error al actualizar el objeto en la base de datos.", ex.Message));
            }
        }

        [HttpDelete("{id:length(24)}")]
        [ActionName("Eliminar")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var validacion = await ValidarUsuarioAsync();
                if (validacion != null) return validacion;

                if (!IdValidator.IsValidObjectId(id))
                    return BadRequest(new ErrorResponse(400, "El ID proporcionado no es válido."));

                var result = await _service.DeleteAsync(id);
                return result
                    ? NoContent()
                    : NotFound(new ErrorResponse(404, "El objeto no fue encontrado para eliminar."));
            }
            catch (MongoException ex)
            {
                return StatusCode(500, new ErrorResponse(500, "Error al eliminar el objeto de la base de datos.", ex.Message));
            }
        }

        public async Task<IActionResult?> ValidarUsuarioAsync()
        {
            string userId = User.Claims.FirstOrDefault()?.Value ?? "";

            if (!IdValidator.IsValidObjectId(userId))
                return BadRequest(new ErrorResponse(400, "El usuario del solicitante no es válido."));

            if (!await _service.VerificarPermiso(userId, NamePermiso))
                return Unauthorized(new ErrorResponse(401, "Permisos insuficientes."));

            return null;
        }
    }
}
