using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.ComponentModel;
using Servidor.src.Repositorios;
using Servidor.src.Objs;
using Servidor.src.Services;
using System.Linq;
using Servidor.src.Atributos;
using Servidor.src.Helper;
using System;
using Shared.Interfaces.ModelsBase;

namespace Servidor.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedorEmpresaController(ServiceProveedorEmpresa servicio) : BaseController<ProveedorEmpresa>(servicio) { }

    [ApiController]
    [Route("api/[controller]")]
    public class ProveedorPersonaController(ServiceProveedorPersona servicio) : BaseController<ProveedorPersona>(servicio) { }

    [ApiController]
    [Route("api/[controller]")]
    public class ClasificacionController(ServiceClasificacion servicio) : BaseController<Clasificacion>(servicio) { }

    [ApiController]
    [Route("api/[controller]")]
    public class RecepcionCargaController(ServiceRecepcionCarga servicio) : BaseController<RecepcionCarga>(servicio) { }

    [ApiController]
    [Route("api/[controller]")]
    public class IdentificadorController(ServiceIdentificador servicio) : BaseController<Identificador>(servicio) { }

    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController(ServiceProducto servicio) : BaseController<Producto>(servicio) { }

}
