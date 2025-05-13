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
    public class UsuarioService : ServiceBase<Usuario>
    {
        private static readonly Lazy<UsuarioService> _instance = new(() => new UsuarioService());
        public static UsuarioService Instance => _instance.Value;


        public override HubServiceBase<Usuario> HubService { get; }

        public override string BaseUrl { get; } = $"{Config.FullUrl}/Usuarios";

        public UsuarioService()
        {
            HubService = new UsuarioHubService(Collection);
        }

        public async Task<(Usuario?, string)> GetThisUser()
        {
            var client = GetClient();
            var request = await GetRequest(HttpMethod.Get, $"{BaseUrl}/this");
            var response = await client.SendAsync(request);
            var error = await ManejarErrores(response);

            if (error.Item1) return (null, error.Item2);

            var json = await response.Content.ReadAsStringAsync();
            var usuario = JsonConvert.DeserializeObject<Usuario>(json);
            return (usuario, error.Item2);
        }

        public async Task InicializarAsync() => await InitAsync(); // ✅ Se ejecuta después de que `HubService` ya esté listo

        public async Task<(bool, string)> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            var client = GetClient();
            var request = await GetRequest(HttpMethod.Put, $"{BaseUrl}/change-password", new { UserId = userId, OldPassword = oldPassword, NewPassword = newPassword });
            var response = await client.SendAsync(request);
            var error = await ManejarErrores(response);
            if (error.Item1) return (false, error.Item2);

            return (true, "");
        }
        public async Task<(bool, string)> AsignarRol(string userId, string rolId)
        {
            var client = GetClient();
            var request = await GetRequest(HttpMethod.Put, $"{BaseUrl}/asignar-rol", new { UserId = userId, RolId = rolId });
            var response = await client.SendAsync(request);
            var error = await ManejarErrores(response);
            if (error.Item1) return (false, error.Item2);
            return (true, "");
        }
    }
}
