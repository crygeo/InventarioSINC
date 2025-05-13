using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
    public interface IHubService<T> : IHubNotification<T> where T : IIdentifiable
    {
        IHubContext<Hub> HubContext { get; }
    }

}
