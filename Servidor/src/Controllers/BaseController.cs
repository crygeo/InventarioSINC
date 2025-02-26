using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Servidor.src.Atributos;
using Servidor.src.Helper;
using Servidor.src.Objs.Interfaces;
using Servidor.src.Repositorios;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Servidor.src.Controllers
{
    [NameAccion("General")]
    public class BaseController<TObj> : ControllerBase where TObj : IIdentifiable
    {
        private readonly RepositorioBase<TObj> _repositorio;

        public BaseController(RepositorioBase<TObj> repositorioBase)
        {
            _repositorio = repositorioBase;
        }

        /// <summary>
        /// Obtiene todos los objetos de la colección.
        /// </summary>
        /// <returns>Lista de objetos.</returns>
        [HttpGet]
        [ActionName("[controller].ConsultarTodo")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var objetos = await _repositorio.GetAll();
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
        [ActionName("[controller].ConsultarUno")]
        public async Task<IActionResult> GetById(string id)
        {
            if (!IdValidator.IsValidObjectId(id))
            {
                return BadRequest(new { message = "El ID proporcionado no es válido." });
            }

            try
            {
                var objeto = await _repositorio.GetById(id);
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
        [ActionName("[controller].Agregar")]
        public async Task<IActionResult> Create(TObj objeto)
        {
            if (objeto == null)
            {
                return BadRequest(new { message = "El objeto no puede ser nulo." });
            }

            try
            {
                await _repositorio.Create(objeto);
                return CreatedAtRoute("GetObjectoById", new { id = objeto.Id }, objeto);
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
        [ActionName("[controller].Editar")]
        public async Task<IActionResult> Update(string id, TObj objetoIn)
        {
            if (!IdValidator.IsValidObjectId(id))
            {
                return BadRequest(new { message = "El ID proporcionado no es válido." });
            }

            if (objetoIn == null)
            {
                return BadRequest(new { message = "El objeto no puede ser nulo." });
            }

            try
            {
                var result = await _repositorio.Update(id, objetoIn);
                return result.MatchedCount == 0
                    ? NotFound(new { message = "El objeto no fue encontrado para actualizar." })
                    : NoContent();
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
        [ActionName("[controller].Eliminar")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!IdValidator.IsValidObjectId(id))
            {
                return BadRequest(new { message = "El ID proporcionado no es válido." });
            }

            try
            {
                var result = await _repositorio.Delete(id);
                return result.DeletedCount == 0
                    ? NotFound(new { message = "El objeto no fue encontrado para eliminar." })
                    : NoContent();
            }
            catch (MongoException ex)
            {
                return StatusCode(500, new { message = "Error al eliminar el objeto en la base de datos.", error = ex.Message });
            }
        }
    }
}
