using Servidor.src.Objs;
using Shared.Interfaces.ModelsBase;

namespace Servidor.src.Hubs
{
    public class HubProveedor : HubBase<IProveedor> { }
    public class HubEmpresa : HubBase<IEmpresa> { }
    public class HubTalla : HubBase<ITalla> { }
    public class HubCalidad : HubBase<ICalidad> { }
    public class HubClase : HubBase<IClase> { }
    public class HubClasificacion : HubBase<IClasificacion> { }
    public class HubRecepcionCarga : HubBase<IRecepcionCarga> { }
}
