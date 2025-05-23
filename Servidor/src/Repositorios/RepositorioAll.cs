﻿using Shared.Interfaces;
using Shared.Interfaces.ModelsBase;

namespace Servidor.src.Repositorios
{
    public class RepositorioTalla : RepositorioBase<ITalla> { }
    public class RepositorioRecepcionCarga : RepositorioBase<IRecepcionCarga> { }
    public class RepositorioProveedor : RepositorioBase<IProveedor> { }
    public class RepositorioClasificacion : RepositorioBase<IClasificacion> { }
    public class RepositorioClase : RepositorioBase<IClase> { }
    public class RepositorioCalidad : RepositorioBase<ICalidad> { }

}
