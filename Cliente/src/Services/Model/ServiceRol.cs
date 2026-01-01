using System.Net.Http;
using Cliente.Extencions;
using Cliente.Obj.Model;
using Shared.Interfaces.ModelsBase;
using Utilidades.Interfaces;

namespace Cliente.Services.Model;

public class ServiceRol : ServiceBase<Rol>, ICustomObjs
{
    

    public async Task<IResultResponse<List<string>>> GetAllPermisos()
    {
        var client = GetClient();
        var request = await GetRequest<Rol>(HttpMethod.Get, $"{BaseUrl}/Perms");
        var response = await client.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            return await HandleError<List<string>>(response);

        return await JsonHelper.TryDeserializeAsync<List<string>>(response, "Permisos cargados correctamente");
    }
}