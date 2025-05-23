using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Cliente.src.Model;
using Cliente.src.ServicesHub;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using Utilidades.Interfaces;

namespace Cliente.src.Services
{
    public class UsuarioService : ServiceBase<Usuario>
    {
        private static readonly Lazy<UsuarioService> _instance = new(() => new UsuarioService());
        public static UsuarioService Instance => _instance.Value;

        public UsuarioService() : base(new UsuarioHubService()){}

        public async Task<IResultResponse<Usuario>> GetThisUser()
        {
            var request = await GetRequest(HttpMethod.Get, $"{BaseUrl}/this");
            return await HandleResponseAsync<Usuario>(request, "Consulta exitosa");
        }

        public async Task<IResultResponse<bool>> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            var request = await GetRequest(HttpMethod.Put, $"{BaseUrl}/change-password", new { UserId = userId, OldPassword = oldPassword, NewPassword = newPassword });
            return await HandleResponseAsync<bool>(request, "Contraseña cambiada correctamente");
        }
        public async Task<IResultResponse<bool>> AsignarRol(string userId, string rolId)
        {
            var request = await GetRequest(HttpMethod.Put, $"{BaseUrl}/asignar-rol", new { UserId = userId, RolId = rolId });
            return await HandleResponseAsync<bool>(request, "Rol asignado correctamente.");
        }
    }
}
