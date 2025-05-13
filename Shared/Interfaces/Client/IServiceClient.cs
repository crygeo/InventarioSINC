using Microsoft.AspNetCore.SignalR;
using Shared.Interfaces.ModelsBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces.Client
{
    public interface IServiceClient<TObj>
    {
        Task<(bool, string)> InitAsync();
        Task StopASunc();
    }
}
