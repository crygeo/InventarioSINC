using DnsClient.Protocol;
using Servidor.Services;
using Servidor.src.Model;
using Servidor.src.Services;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Servidor
{
    public class AppInitializer
    {
        public async Task InitAsync()
        {
            await ServiceFactory.GetService<Usuario>().InitServiceAsync();
            await ServiceFactory.GetService<Usuario>().InitServiceAsync();
        }
    }
}
