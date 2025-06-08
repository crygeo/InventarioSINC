using System.Net.Http;
using Cliente.Extencions;
using Cliente.Obj.Model;
using Cliente.ServicesHub;
using Shared.Interfaces.ModelsBase;
using Utilidades.Interfaces;

namespace Cliente.Services.Model;

public class ServiceRol : ServiceBase<Rol>, ICustomObjs
{

    public Rol? ObtenerPorId(string idRol) => Collection.FirstOrDefault((r) => r.Id == idRol);
    public async Task InicializarAsync() => await InitAsync(); // ✅ Se ejecuta después de que `HubService` ya esté listo

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