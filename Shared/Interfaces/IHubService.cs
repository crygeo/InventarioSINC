using Microsoft.AspNetCore.SignalR;
using Shared.Interfaces.Model;

namespace Shared.Interfaces;

public interface IHubService<T> : IHubNotification<T> where T : IModelObj
{
    IHubContext<Hub> HubContext { get; }
}