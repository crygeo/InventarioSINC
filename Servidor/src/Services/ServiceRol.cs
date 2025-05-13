using Servidor.src.Hubs;
using Servidor.src.HubsService;
using Servidor.src.Objs;
using Servidor.src.Repositorios;
using Servidor.src.Statics;
using Shared.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Servidor.src.Services
{
    public class ServiceRol : ServiceBase<Rol>
    {
        public override IRepository<Rol> Repository { get; } = new RepositorioRol();
        public override IHubService<Rol> HubService { get; }

        public ServiceRol(HubsServiceRol hubService) : base()
        {
            HubService = hubService;
        }

        public Task<List<string>> GetAllPermisos()
        {
            return Task.FromResult(Permisos.List);
        }
    }
}
