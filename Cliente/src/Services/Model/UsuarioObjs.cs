using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Cliente.src.Model;
using Cliente.src.ServicesHub;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using Shared.Interfaces.ModelsBase;
using Utilidades.Interfaces;

namespace Cliente.src.Services.Model
{
    public class UsuarioObjs : ServiceBase<Usuario>, ICustomObjs
    {
        public UsuarioObjs() : base(new UsuarioHubService()){}

        public async Task<IResultResponse<Usuario>> GetThisUser()
        {
            var request = await GetRequest<Usuario>(HttpMethod.Get, $"{BaseUrl}/this");
            return await HandleResponseAsync<Usuario, Usuario>(request, "Consulta exitosa");
        }

        public async Task<IResultResponse<bool>> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            var request = await GetRequest<Usuario>(HttpMethod.Put, $"{BaseUrl}/change-password", new { UserId = userId, OldPassword = oldPassword, NewPassword = newPassword });
            return await HandleResponseAsync<bool, Usuario>(request, "Contraseña cambiada correctamente");
        }
        public async Task<IResultResponse<bool>> AsignarRol(string userId, string rolId)
        {
            var request = await GetRequest<Usuario>(HttpMethod.Put, $"{BaseUrl}/asignar-rol", new { UserId = userId, RolId = rolId });
            return await HandleResponseAsync<bool, Usuario>(request, "Rol asignado correctamente.");
        }
    }
}
