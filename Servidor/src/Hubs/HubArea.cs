using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Servidor.Model;

namespace Servidor.Hubs;

public class HubArea : HubBase<Area>
{
    public virtual async Task NewTurnoId(string areaId, string turnoId)
    {
        await Clients.All.SendAsync($"NewTurnoId{typeof(Area).Name}", areaId, turnoId);
    }
}