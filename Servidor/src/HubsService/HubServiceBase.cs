using Microsoft.AspNetCore.SignalR;
using Servidor.src.Hubs;
using Shared.Interfaces;
using System.Threading.Tasks;
using Shared.Interfaces.Model;

namespace Servidor.src.HubsService
{
    public class HubServiceBase<T> : IHubService<T> where T : class, IModelObj
    {

        private IHubContext<Hub>? _hubContext;
        public IHubContext<Hub> HubContext =>  _hubContext ??= (IHubContext<Hub>) HubFactory.GetHubContext<T>();


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
