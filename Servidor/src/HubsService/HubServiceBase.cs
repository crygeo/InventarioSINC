using Microsoft.AspNetCore.SignalR;
using Servidor.src.Hubs;
using Shared.Interfaces;
using Shared.Interfaces.Model;
using System.Threading.Tasks;

namespace Servidor.src.HubsService
{
    public abstract class HubServiceBase<T> : IHubService<T> where T : IModelObj
    {
        public IHubContext<Hub> HubContext { get; }
        public HubServiceBase(IHubContext<Hub> hubContext)
        {
            HubContext = hubContext;
        }

        public async Task NewItem(T obj)
        {
            await HubContext.Clients.All.SendAsync($"New{typeof(T).Name}", obj);
        }

        public async Task UpdateItem(T obj)
        {
            await HubContext.Clients.All.SendAsync($"Update{typeof(T).Name}", obj);
        }

        public async Task DeleteItem(T obj)
        {
            await HubContext.Clients.All.SendAsync($"Delete{typeof(T).Name}", obj);
        }
    }
}
