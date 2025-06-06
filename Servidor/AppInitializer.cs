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
            ass();

            await ServiceFactory.GetService<Usuario>().InitServiceAsync();
            await ServiceFactory.GetService<Usuario>().InitServiceAsync();
        }
        private static void ass()
        {
            var a = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == "Servidor");
            if (a == null) return;

            try
            {
                var al = a.GetExportedTypes();
                foreach ( var type in al)
                {
                    Console.WriteLine(type.Name);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
