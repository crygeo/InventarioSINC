using Servidor.src.Objs;
using Shared.Interfaces;
using Shared.Interfaces.ModelsBase;

namespace Servidor.src.Repositorios
{
    public class RepositorioProveedorEmpresa : RepositorioBase<ProveedorEmpresa> { }
    public class RepositorioProveedorPersona: RepositorioBase<ProveedorPersona> { }
    public class RepositorioClasificacion : RepositorioBase<Clasificacion> { }
    public class RepositorioRecepcionCarga : RepositorioBase<RecepcionCarga> { }
    public class RepositorioIdentificador : RepositorioBase<Identificador> { }
    public class RepositorioProducto : RepositorioBase<Producto> { }

}
