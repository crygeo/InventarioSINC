using System.Net.Http;
using Cliente.Obj.Model;
using Shared.Interfaces.ModelsBase;
using Utilidades.Interfaces;

namespace Cliente.Services.Model;

public class ServiceTurno : ServiceBase<Turno>, ICustomObjs
{
    public async Task<IResultResponse<bool>> CreateInAreaAsync(Turno turno, string? areaId)
    {
        if (string.IsNullOrWhiteSpace(areaId))
            return ResultResponse<bool>.Fail("No hay área seleccionada.");
        string url = $"{BaseUrl}/{areaId}";

        var request = await GetRequest<Turno>(HttpMethod.Post, url, turno);
        return await HandleResponseAsync<bool, Turno>(request, "Creado exitosamente", true);
    }

}