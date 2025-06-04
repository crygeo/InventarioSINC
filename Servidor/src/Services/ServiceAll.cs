using Microsoft.AspNetCore.Mvc;
using Servidor.src.Hubs;
using Servidor.src.HubsService;
using Servidor.src.Objs;
using Servidor.src.Repositorios;
using Shared.Extensions;
using Shared.Interfaces;
using Shared.Interfaces.ModelsBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Servidor.src.Services
{
    public class ServiceProveedorEmpresa : ServiceBase<ProveedorEmpresa>
    {
        public override IRepository<ProveedorEmpresa> Repository { get; } = new RepositorioProveedorEmpresa();
        public override IHubService<ProveedorEmpresa> HubService { get; }

        public ServiceProveedorEmpresa(HubsServiceProveedorEmpresa hubService)
        {
            HubService = hubService;
        }
    }

    public class ServiceProveedorPersona : ServiceBase<ProveedorPersona>
    {
        public override IRepository<ProveedorPersona> Repository { get; } = new RepositorioProveedorPersona();
        public override IHubService<ProveedorPersona> HubService { get; }

        public ServiceProveedorPersona(HubsServiceProveedorPersona hubService)
        {
            HubService = hubService;
        }
    }
    

    public class ServiceRecepcionCarga : ServiceBase<RecepcionCarga>
    {
        public override IRepository<RecepcionCarga> Repository { get; } = new RepositorioRecepcionCarga();
        public override IHubService<RecepcionCarga> HubService { get; }

        public ServiceRecepcionCarga(HubsServiceRecepcionCarga hubService)
        {
            HubService = hubService;
        }
    }

    public class ServiceClasificacion : ServiceBase<Clasificacion>
    {
        public override IRepository<Clasificacion> Repository { get; } = new RepositorioClasificacion();
        public override IHubService<Clasificacion> HubService { get; }

        public ServiceClasificacion(HubsServiceClasificacion hubService)
        {
            HubService = hubService;
        }
    }

    public class ServiceIdentificador : ServiceBase<Identificador>
    {
        public override IRepository<Identificador> Repository { get; } = new RepositorioIdentificador();
        public override IHubService<Identificador> HubService { get; }

        public ServiceIdentificador(HubsServiceIdentificador hubService)
        {
            HubService = hubService;
        }
    }

    public class ServiceProducto : ServiceBase<Producto>
    {
        public override IRepository<Producto> Repository { get; } = new RepositorioProducto();
        public override IHubService<Producto> HubService { get; }

        public ServiceProducto(HubsServiceProducto hubService)
        {
            HubService = hubService;
        }
    }



}
