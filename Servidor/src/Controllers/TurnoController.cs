using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Servidor.Model;
using Servidor.Services;
using Shared.ObjectsResponse;

namespace Servidor.Controllers;

public partial class TurnoController : BaseController<Turno>
{
    private ServiceArea ServiceArea => (ServiceArea)ServiceFactory.GetService<Area>();

    [HttpPost("{areaId}")]
    [ActionName("CrearTurnoYAsociarAlArea")]
    public async Task<IActionResult> CrearTurnoYAsociarAlArea(string areaId, [FromBody] Turno turno)
    {
        try
        {
            // 1. Validar usuario (MISMO chequeo que CreateAsync base)
            var validacion = await ValidarUsuarioAsync();
            if (validacion != null) return validacion;

            if (turno == null)
                return BadRequest(new ErrorResponse(400, "El turno no puede ser nulo."));

            // ---------------------------------------------------------------
            // 2. Crear turno usando LA MISMA LÓGICA del CreateAsync original
            // ---------------------------------------------------------------

            turno.Deleteable = true;
            if (!await Service.CreateAsync(turno))
                return StatusCode(500,
                    new ErrorResponse(500, "Error al crear el turno en la base de datos."));

            // ---------------------------------------------------------------
            // 3. Asociar Turno al Área
            // ---------------------------------------------------------------

            var okArea = await ServiceArea.AddItemIdToListAsync(areaId, nameof(Area.TurnoIds), turno.Id);

            if (!okArea)
                return StatusCode(500,
                    new ErrorResponse(500, "El turno se creó, pero no se pudo asociar al área."));

            return Ok(); // Éxito total
        }
        catch (MongoException ex)
        {
            return StatusCode(500,
                new ErrorResponse(500, "Error al crear el turno y asociarlo al área.", ex.Message));
        }
    }


}