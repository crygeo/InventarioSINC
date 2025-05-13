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
    public class ProveedorController(ServiceProveedor servicio) : BaseController<IProveedor>(servicio) { }

    [ApiController]
    [Route("api/[controller]")]
    public class EmpresaController(ServiceEmpresa servicio) : BaseController<IEmpresa>(servicio) { }

    [ApiController]
    [Route("api/[controller]")]
    public class TallaController(ServiceTalla servicio) : BaseController<ITalla>(servicio) { }

    [ApiController]
    [Route("api/[controller]")]
    public class CalidadController(ServiceCalidad servicio) : BaseController<ICalidad>(servicio) { }

    [ApiController]
    [Route("api/[controller]")]
    public class ClaseController(ServiceClase servicio) : BaseController<IClase>(servicio) { }

    [ApiController]
    [Route("api/[controller]")]
    public class ClasificacionController(ServiceClasificacion servicio) : BaseController<IClasificacion>(servicio) { }

    [ApiController]
    [Route("api/[controller]")]
    public class RecepcionCargaController(ServiceRecepcionCarga servicio) : BaseController<IRecepcionCarga>(servicio) { }

}
