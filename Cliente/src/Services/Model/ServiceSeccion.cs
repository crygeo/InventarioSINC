using System.Net.Http;
using Cliente.Obj.Model;
using Shared.Interfaces.ModelsBase;
using Utilidades.Interfaces;

namespace Cliente.Services.Model;

public class ServiceSeccion : ServiceBase<Seccion>, ICustomObjs
{
    
    public async Task<IResultResponse<bool>> CreateInTurnoAsync(Seccion seccion, string? turnoId)
    {
        if (string.IsNullOrWhiteSpace(turnoId))
            return ResultResponse<bool>.Fail("No hay turno seleccionado.");

        string url = $"{BaseUrl}/{turnoId}"; // Coincide con [HttpPost("{turnoId}")]
    
        var request = await GetRequest<Seccion>(HttpMethod.Post, url, seccion);
        return await HandleResponseAsync<bool, Seccion>(request, "Creado exitosamente", true);
    }

}