using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Servidor.Model;

namespace Servidor.HubsService;

public class HubServiceArea : HubServiceBase<Area>
{
    public virtual async Task NewTurnoId(string areaId, string turnoId)
    {
        await HubContext.Clients.All.SendAsync($"NewTurnoId{typeof(Area).Name}", areaId, turnoId);
    }
}