using System.Net.Http;
using Cliente.Obj.Model;
using Shared.Interfaces.ModelsBase;
using Utilidades.Interfaces;

namespace Cliente.Services.Model;

public class ServiceGrupo : ServiceBase<Grupo>, ICustomObjs
{
    
    public async Task<IResultResponse<bool>> CreateInSeccionAsync(Grupo grupo, string? seccionId)
    {
        if (string.IsNullOrWhiteSpace(seccionId))
            return ResultResponse<bool>.Fail("No hay Grupo seleccionado.");

        grupo.SeccionId = seccionId;
        return await CreateAsync(grupo);
        
    }

}