using Microsoft.AspNetCore.SignalR;
using Servidor.src.Hubs;
using Servidor.src.Objs;

namespace Servidor.src.HubsService
{
    public class HubsServiceUsuario : HubServiceBase<Usuario>
    {
        public HubsServiceUsuario(IHubContext<HubUsuario> hubContext): base(hubContext)
        {
            
        }
    }
}
