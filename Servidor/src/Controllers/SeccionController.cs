using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Servidor.Model;
using Servidor.Services;
using Shared.Interfaces.Model.Obj;
using Shared.ObjectsResponse;

namespace Servidor.Controllers;

public partial class SeccionController : BaseController<Seccion>
{
    
    private ServiceBase<Turno> ServiceTurno => ServiceFactory.GetService<Turno>();

    [HttpPost("{turnoId}")]
    [ActionName("AgregarEnTurno")]
    public async Task<IActionResult> CrearSeccionEnTurno(string turnoId, [FromBody] Seccion seccion)
    {
        try
        {
            // 1. Validar usuario (MISMO chequeo que CreateAsync base)
            var validacion = await ValidarUsuarioAsync();
            if (validacion != null) return validacion;

            if (seccion == null)
                return BadRequest(new ErrorResponse(400, "La sección no puede ser nula."));

            // ----------------------------------------------------------------
            // 2. Crear sección usando LA MISMA LÓGICA del CreateAsync original
            // ----------------------------------------------------------------

            seccion.Deleteable = true;

            if (!await Service.CreateAsync(seccion))
                return StatusCode(500,
                    new ErrorResponse(500, "Error al crear la sección en la base de datos."));

            // ----------------------------------------------------------------
            // 3. Asociar sección al Turno
            // ----------------------------------------------------------------

            var turno = await ServiceTurno.GetByIdAsync(turnoId);

            if (turno == null)
                return BadRequest(new ErrorResponse(404, $"El turno {turnoId} no existe."));

            turno.SeccionIds ??= new List<string>();
            turno.SeccionIds.Add(seccion.Id);

            if (!await ServiceTurno.UpdateAsync(turno.Id, turno))
                return StatusCode(500,
                    new ErrorResponse(500, "La sección se creó, pero no se pudo asociar al turno."));

            return Ok(); // Éxito total
        }
        catch (MongoException ex)
        {
            return StatusCode(500,
                new ErrorResponse(500, "Error al crear la sección y asociarla al turno.", ex.Message));
        }
    }

}