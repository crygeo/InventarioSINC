using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
    public interface IService<TObj> : ICrud<TObj> where TObj : IIdentifiable
    {
        IRepository<TObj> Repository { get; }
        IHubService<TObj> HubService { get; }

        

    }
}
