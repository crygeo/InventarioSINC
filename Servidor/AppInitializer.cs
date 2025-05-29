using Servidor.src.Services;
using System.Threading.Tasks;

namespace Servidor
{
    public class AppInitializer
    {
        private readonly ServiceUsuario _serviceUsuario;
        private readonly ServiceRol _serviceRol;

        public AppInitializer(ServiceUsuario serviceUsuario, ServiceRol serviceRol)
        {
            _serviceUsuario = serviceUsuario;
            _serviceRol = serviceRol;
        }

        public async Task InitAsync()
        {
            await _serviceRol.InitServiceAsync();
            await _serviceUsuario.InitServiceAsync();
        }
    }
}
