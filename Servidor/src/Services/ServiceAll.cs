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
    public class ServiceProveedor : ServiceBase<IProveedor>
    {
        public override IRepository<IProveedor> Repository { get; } = new RepositorioProveedor();
        public override IHubService<IProveedor> HubService { get; }

        public ServiceProveedor(HubsServiceProveedor hubService)
        {
            HubService = hubService;
        }
    }
    
    public class ServiceTalla : ServiceBase<ITalla>
    {
        public override IRepository<ITalla> Repository { get; } = new RepositorioTalla();
        public override IHubService<ITalla> HubService { get; }

        public ServiceTalla(HubsServiceTalla hubService)
        {
            HubService = hubService;
        }
    }

    public class ServiceRecepcionCarga : ServiceBase<IRecepcionCarga>
    {
        public override IRepository<IRecepcionCarga> Repository { get; } = new RepositorioRecepcionCarga();
        public override IHubService<IRecepcionCarga> HubService { get; }

        public ServiceRecepcionCarga(HubsServiceRecepcionCarga hubService)
        {
            HubService = hubService;
        }
    }

    public class ServiceClasificacion : ServiceBase<IClasificacion>
    {
        public override IRepository<IClasificacion> Repository { get; } = new RepositorioClasificacion();
        public override IHubService<IClasificacion> HubService { get; }

        public ServiceClasificacion(HubsServiceClasificacion hubService)
        {
            HubService = hubService;
        }
    }

    public class ServiceClase : ServiceBase<Clase>
    {
        public override IRepository<Clase> Repository { get; } = new RepositorioClase();
        public override IHubService<Clase> HubService { get; }

        public ServiceClase(HubsServiceClase hubService)
        {
            HubService = hubService;
        }
    }

    public class ServiceCalidad : ServiceBase<Calidad>
    {
        public override IRepository<Calidad> Repository { get; } = new RepositorioCalidad();
        public override IHubService<Calidad> HubService { get; }

        public ServiceCalidad(HubsServiceCalidad hubService)
        {
            HubService = hubService;
        }
    }

}
