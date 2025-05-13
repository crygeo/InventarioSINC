using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Servidor.src.Atributos;
using Servidor.src.Helper;
using Servidor.src.Objs;
using Servidor.src.Repositorios;
using Servidor.src.Services;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.src.Controllers
{
    [ApiController]
    public abstract class BaseController<TObj> : ControllerBase where TObj : IIdentifiable
    {
        public readonly ServiceBase<TObj> _service;

        public string NamePermiso { get => $"{ControllerContext.ActionDescriptor.ControllerName}.{ControllerContext.ActionDescriptor.ActionName}"; }
        public BaseController(ServiceBase<TObj> service)
        {
            _service = service;
        }

        /// <summary>
        /// Obtiene todos los objetos de la colección.
        /// </summary>
        /// <returns>Lista de objetos.</returns>
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
                return StatusCode(500, new { message = "Error al obtener los objetos de la base de datos.", error = ex.Message });
            }
        }


        /// <summary>
        /// Obtiene un objeto por su ID.
        /// </summary>
        /// <param name="id">ID del objeto.</param>
        /// <returns>Objeto solicitado.</returns>
        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> GetById(string id)
        {

            try
            {
                var validacion = await ValidarUsuarioAsync();
                if (validacion != null) return validacion;

                if(!IdValidator.IsValidObjectId(id))
                    return BadRequest(new { message = "El ID proporcionado no es válido." });

                var objeto = await _service.GetByIdAsync(id);
                return objeto != null
                    ? Ok(objeto)
                    : NotFound(new { message = "El objeto no fue encontrado." });
            }
            catch (MongoException ex)
            {
                return StatusCode(500, new { message = "Error al obtener el objeto de la base de datos.", error = ex.Message });
            }
        }


        /// <summary>
        /// Crea un nuevo objeto.
        /// </summary>
        /// <param name="objeto">Objeto a crear.</param>
        /// <returns>El objeto creado con su ubicación.</returns>
        [HttpPost]
        [ActionName("Agregar")]
        public async Task<IActionResult> Create(TObj objeto)
        {

            try
            {
                var validacion = await ValidarUsuarioAsync();
                if (validacion != null) return validacion;

                if (objeto == null)
                    return BadRequest(new { message = "El objeto no puede ser nulo." });

                if (await _service.CreateAsync(objeto))
                    return Ok();
                else
                    return StatusCode(500, new { message = "Error al crear el objeto en la base de datos." });
            }
            catch (MongoException ex)
            {
                return StatusCode(500, new { message = "Error al crear el objeto en la base de datos.", error = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza un objeto existente.
        /// </summary>
        /// <param name="id">ID del objeto a actualizar.</param>
        /// <param name="objetoIn">Datos del objeto actualizado.</param>
        /// <returns>Estado de la operación.</returns>
        [HttpPut("{id:length(24)}")]
        [ActionName("Editar")]
        public async Task<IActionResult> Update(string id, [FromBody] TObj objetoIn)
        {
            try
            {

                var validacion = await ValidarUsuarioAsync();
                if (validacion != null) return validacion;

                if (!IdValidator.IsValidObjectId(id))
                    return BadRequest(new { message = "El ID proporcionado no es válido." });

                if (objetoIn == null)
                    return BadRequest(new { message = "El objeto no puede ser nulo." });

                if (GetById(id) == null)
                    return NotFound(new { message = "El objeto no fue encontrado." });

                var result = await _service.UpdateAsync(id, objetoIn);
                return NoContent();
            }
            catch (MongoException ex)
            {
                return StatusCode(500, new { message = "Error al actualizar el objeto en la base de datos.", error = ex.Message });
            }
        }


        /// <summary>
        /// Elimina un objeto existente.
        /// </summary>
        /// <param name="id">ID del objeto a eliminar.</param>
        /// <returns>Estado de la operación.</returns>
        [HttpDelete("{id:length(24)}")]
        [ActionName("Eliminar")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var validacion = await ValidarUsuarioAsync();
                if (validacion != null) return validacion;

                if (!IdValidator.IsValidObjectId(id))
                    return BadRequest(new { message = "El ID proporcionado no es válido." });

                var result = await _service.DeleteAsync(id);
                return result == false
                    ? NotFound(new { message = "El objeto no fue encontrado para eliminar." })
                    : NoContent();
            }
            catch (MongoException ex)
            {
                return StatusCode(500, new { message = "Error al eliminar el objeto en la base de datos.", error = ex.Message });
            }
        }

        // Método reutilizable para validar el usuario y los permisos
        public async Task<IActionResult?> ValidarUsuarioAsync()
        {
            string userId = User.Claims.FirstOrDefault()?.Value ?? "";

            if (!IdValidator.IsValidObjectId(userId))
                return BadRequest(new { message = "El Usuario del solicitante no es válido." });

            if (!await _service.VerificarPermiso(userId, NamePermiso))
                return Unauthorized($"Permisos insuficientes.");

            return null; // Todo está ok
        }


    }
}
