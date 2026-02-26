using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Servidor.DomainService;
using Servidor.Model;
using Shared.ObjectsResponse;

namespace Servidor.Controllers;

public partial class AreaController : BaseController<Area>
{
    private readonly DomainAreaTurno Domain;

    public AreaController(DomainAreaTurno domain)
    {
        Domain = domain;
    }

    public override async Task<IActionResult> DeleteAsync(string childId)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var auth = await ValidateUserAsync();
            if (auth != null) return auth;

            var idError = ValidateEntityId(childId);
            if (idError != null) return idError;

            return await Domain.EliminarAreaHeHijosAsync(childId)
                ? NoContent()
                : NotFound(new ErrorResponse(404,
                    "Error al eliminar el área. Es posible que el área no exista o que ocurriera un error al eliminar los turnos asociados."));
        }, "Error no controlado al eliminar el objeto de la base de datos.");
    }
}

public partial class TurnoController : BaseController<Turno>
{
    private DomainTurnoSeccion Domain => new();

    public override async Task<IActionResult> DeleteAsync(string childId)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var auth = await ValidateUserAsync();
            if (auth != null) return auth;

            var idError = ValidateEntityId(childId);
            if (idError != null) return idError;

            return await Domain.EliminarTurnoHeHijosAsync(childId)
                ? NoContent()
                : NotFound(new ErrorResponse(404,
                    "Error al eliminar el Turno. Es posible que el Turno no exista o que ocurriera un error al eliminar las secciones asociadas."));
        }, "Error no controlado al eliminar el objeto de la base de datos.");
    }
}

public partial class SeccionController : BaseController<Seccion>
{
    private DomainSeccionGrupo Domain => new();

    public override async Task<IActionResult> DeleteAsync(string childId)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var auth = await ValidateUserAsync();
            if (auth != null) return auth;

            var idError = ValidateEntityId(childId);
            if (idError != null) return idError;

            return await Domain.EliminarSeccionHeHijosAsync(childId)
                ? NoContent()
                : NotFound(new ErrorResponse(404,
                    "Error al eliminar la Seccion. Es posible que la Seccion no exista o que ocurriera un error al eliminar los Grupos asociadas."));
        }, "Error no controlado al eliminar el objeto de la base de datos.");
    }
}