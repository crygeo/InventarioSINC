using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Servidor.Model;
using Servidor.Services;
using Shared.Interfaces.Model.Obj;
using Shared.ObjectsResponse;

namespace Servidor.Controllers;

public partial class GrupoController : BaseController<Grupo>
{
    
    private ServiceBase<Seccion> ServiceSeccion => ServiceFactory.GetService<Seccion>();

    [HttpPost("{seccionId}")]
    [ActionName("AgregarEnSeccion")]
    public async Task<IActionResult> CrearGrupoEnSeccion(string seccionId, [FromBody] Grupo grupo)
    {
        try
        {
            // 1. Validar usuario (MISMO chequeo que CreateAsync base)
            var validacion = await ValidarUsuarioAsync();
            if (validacion != null) return validacion;

            if (grupo == null)
                return BadRequest(new ErrorResponse(400, "La sección no puede ser nula."));

            // ----------------------------------------------------------------
            // 2. Crear sección usando LA MISMA LÓGICA del CreateAsync original
            // ----------------------------------------------------------------

            grupo.Deleteable = true;

            if (!await Service.CreateAsync(grupo))
                return StatusCode(500,
                    new ErrorResponse(500, "Error al crear la sección en la base de datos."));

            // ----------------------------------------------------------------
            // 3. Asociar grupo al seccion
            // ----------------------------------------------------------------

            var seccion = await ServiceSeccion.GetByIdAsync(seccionId);

            if (seccion == null)
                return BadRequest(new ErrorResponse(404, $"El seccion {seccionId} no existe."));

            seccion.GrupoIds ??= new List<string>();
            seccion.GrupoIds.Add(grupo.Id);

            if (!await ServiceSeccion.UpdateAsync(seccion.Id, seccion))
                return StatusCode(500,
                    new ErrorResponse(500, "El grupo se creó, pero no se pudo asociar al seccion."));

            return Ok(); // Éxito total
        }
        catch (MongoException ex)
        {
            return StatusCode(500,
                new ErrorResponse(500, "Error al crear la sección y asociarla al turno.", ex.Message));
        }
    }

}