using System.Net.Http;
using Cliente.Obj.Model;
using Shared.Interfaces.ModelsBase;
using Utilidades.Interfaces;

namespace Cliente.Services.Model;

public class ServiceGrupo : ServiceBase<Grupo>, ICustomObjs
{
    
    public async Task<IResultResponse<bool>> CreateInSeccionAsync(Grupo seccion, string? seccionId)
    {
        if (string.IsNullOrWhiteSpace(seccionId))
            return ResultResponse<bool>.Fail("No hay seccion seleccionado.");

        string url = $"{BaseUrl}/{seccionId}";
    
        var request = await GetRequest<Grupo>(HttpMethod.Post, url, seccion);
        return await HandleResponseAsync<bool, Grupo>(request, "Creado exitosamente", true);
    }

}