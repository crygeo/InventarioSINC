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
        public static readonly string GENERAL_ROLE_ID = "6650c6a2b5cf184a0a8a0f3b";


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

        private async Task CrearRolGeneral()
        {
            var roles = await Repository.GetByIdAsync(GENERAL_ROLE_ID);
            if (roles != null) return; // Si ya existe el rol general, no hacemos nada
            // Crear un rol general por defecto si no existe
            var rolGeneral = new Rol
            {
                Nombre = "General",
                Permisos = Permisos.List,
                IsAdmin = true,
                Deleteable = false,
                Id = GENERAL_ROLE_ID,
            };

            await Repository.CreateAsync(rolGeneral);
        }

        public override async Task InitServiceAsync()
        {
            await CrearRolGeneral();
        }

    }
}
