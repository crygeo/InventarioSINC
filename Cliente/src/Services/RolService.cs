using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Cliente.src.Model;
using Cliente.src.ServicesHub;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;

namespace Cliente.src.Services
{
    public class RolService : ServiceBase<Rol>
    {
        private static readonly Lazy<RolService> _instance = new(() => new RolService());
        public static RolService Instance => _instance.Value;

        public override HubServiceBase<Rol> HubService { get; }

        public override string BaseUrl { get; } = $"{Config.FullUrl}/Roles";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection">Instancia de una collecion para la sincronizacion de los datos.</param>
        public RolService()
        {
            HubService = new RolHubService(Collection);
        }

        public Rol? ObtenerPorId(string idRol) => Collection.FirstOrDefault((r) => r.Id == idRol);
        public async Task InicializarAsync() => await InitAsync(); // ✅ Se ejecuta después de que `HubService` ya esté listo

        public async Task<(List<string>, string)> GetAllPermisos()
        {
            var client = GetClient();
            var request = await GetRequest(HttpMethod.Get, $"{BaseUrl}/Perms");
            var response = await client.SendAsync(request);

            var result = await ManejarErrores(response);
            
            if (result.Item1) return ([], result.Item2);

            var json = await response.Content.ReadAsStringAsync();
            var datos = JsonConvert.DeserializeObject<List<string>>(json);
            return (datos ?? [], " ");
        }
    }
}
