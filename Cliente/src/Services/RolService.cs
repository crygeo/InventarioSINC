using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Cliente.src.Model;
using Cliente.src.ServicesHub;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using Shared.ObjectsResponse;
using Utilidades.Interfaces;
using Cliente.src.Extencions;

namespace Cliente.src.Services
{
    public class RolService : ServiceBase<Rol>
    {
        private static readonly Lazy<RolService> _instance = new(() => new RolService());
        public static RolService Instance => _instance.Value;

        public RolService() : base(new RolHubService()) { }

        public Rol? ObtenerPorId(string idRol) => Collection.FirstOrDefault((r) => r.Id == idRol);
        public async Task InicializarAsync() => await InitAsync(); // ✅ Se ejecuta después de que `HubService` ya esté listo

        public async Task<IResultResponse<List<string>>> GetAllPermisos()
        {
            var client = GetClient();
            var request = await GetRequest(HttpMethod.Get, $"{BaseUrl}/Perms");
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return await HandleError<List<string>>(response);

            return await JsonHelper.TryDeserializeAsync<List<string>>(response, "Permisos cargados correctamente");
        }

    }
}
