using Microsoft.AspNetCore.SignalR;
using Servidor.src.Hubs;
using Servidor.src.Objs;

namespace Servidor.src.HubsService
{
    public class HubsServiceRol : HubServiceBase<Rol>
    {
        public HubsServiceRol(IHubContext<HubRol> hubContext): base(hubContext)
        {
            
        }
    }
}
