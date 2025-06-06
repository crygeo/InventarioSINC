using Microsoft.AspNetCore.SignalR;
using Shared.Interfaces.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
    public interface IService<TObj> : ICrud<TObj> where TObj : IModelObj
    {
        IRepository<TObj> Repository { get; }
        IHubService<TObj> HubService { get; }

        

    }
}
