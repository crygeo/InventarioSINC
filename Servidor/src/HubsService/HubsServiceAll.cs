using Microsoft.AspNetCore.SignalR;
using Servidor.src.Hubs;
using Servidor.src.Objs;
using Shared.Interfaces.ModelsBase;

namespace Servidor.src.HubsService
{
    public class HubsServiceProveedorEmpresa : HubServiceBase<ProveedorEmpresa>
    {
        public HubsServiceProveedorEmpresa(IHubContext<HubProveedorEmpresa> hubContext) : base(hubContext) { }
    }

    public class HubsServiceProveedorPersona : HubServiceBase<ProveedorPersona>
    {
        public HubsServiceProveedorPersona(IHubContext<HubProveedorPersona> hubContext) : base(hubContext) { }
    }

    public class HubsServiceRecepcionCarga : HubServiceBase<RecepcionCarga>
    {
        public HubsServiceRecepcionCarga(IHubContext<HubRecepcionCarga> hubContext) : base(hubContext) { }
    }

    public class HubsServiceClasificacion : HubServiceBase<Clasificacion>
    {
        public HubsServiceClasificacion(IHubContext<HubClasificacion> hubContext) : base(hubContext) { }
    }

    public class HubsServiceIdentificador : HubServiceBase<Identificador>
    {
        public HubsServiceIdentificador(IHubContext<HubIdentificador> hubContext) : base(hubContext) { }
    }

    public class HubsServiceProducto : HubServiceBase<Producto>
    {
        public HubsServiceProducto(IHubContext<HubProducto> hubContext) : base(hubContext) { }
    }

}
