using Servidor.src.Objs;
using Shared.Interfaces.ModelsBase;

namespace Servidor.src.Hubs
{
    public class HubProveedorEmpresa : HubBase<ProveedorEmpresa> { }
    public class HubProveedorPersona : HubBase<ProveedorPersona> { } 
    public class HubClasificacion : HubBase<Clasificacion> { }
    public class HubRecepcionCarga : HubBase<RecepcionCarga> { }
    public class HubIdentificador : HubBase<Identificador> { }
    public class HubProducto : HubBase<Producto> { }
}
