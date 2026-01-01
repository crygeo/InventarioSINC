using System.Threading.Tasks;
using Servidor.Model;
using Servidor.Services;

namespace Servidor;

public class AppInitializer
{
    public async Task InitAsync()
    {
        await ServiceFactory.GetService<Usuario>().InitServiceAsync();
        await ServiceFactory.GetService<Rol>().InitServiceAsync();
    }
}