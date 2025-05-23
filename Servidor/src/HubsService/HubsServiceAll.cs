using Microsoft.AspNetCore.SignalR;
using Servidor.src.Hubs;
using Servidor.src.Objs;
using Shared.Interfaces.ModelsBase;

namespace Servidor.src.HubsService
{
    public class HubsServiceProveedor : HubServiceBase<IProveedor>
    {
        public HubsServiceProveedor(IHubContext<HubProveedor> hubContext) : base(hubContext) { }
    }

    public class HubsServiceTalla : HubServiceBase<ITalla>
    {
        public HubsServiceTalla(IHubContext<HubTalla> hubContext) : base(hubContext) { }
    }

    public class HubsServiceRecepcionCarga : HubServiceBase<IRecepcionCarga>
    {
        public HubsServiceRecepcionCarga(IHubContext<HubRecepcionCarga> hubContext) : base(hubContext) { }
    }

    public class HubsServiceClasificacion : HubServiceBase<IClasificacion>
    {
        public HubsServiceClasificacion(IHubContext<HubClasificacion> hubContext) : base(hubContext) { }
    }

    public class HubsServiceClase : HubServiceBase<IClase>
    {
        public HubsServiceClase(IHubContext<HubClase> hubContext) : base(hubContext) { }
    }

    public class HubsServiceCalidad : HubServiceBase<ICalidad>
    {
        public HubsServiceCalidad(IHubContext<HubCalidad> hubContext) : base(hubContext) { }
    }

}
