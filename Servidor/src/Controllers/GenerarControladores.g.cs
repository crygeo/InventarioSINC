

//
//    ✔ Genera controladores basados en archivos .cs en /Model
//    ❌ Ignora modelos que ya tienen controladores manuales (basado en el nombre de archivo)
//    📁 Requiere:
//        - Este archivo .tt en carpeta /Controllers
//        - Modelos en carpeta /Model
//    ✨ No requiere cargar ensamblados, funciona solo con nombres de archivo.
//    ⚠️ No se recomienda para producción, solo para desarrollo y pruebas rápidas
//
//	  📅 Última actualización: 2025-06-06
//    🛠️ Creado por ChatGPT + CryGeo
//


// Codigo generado automáticamente por GenerarControladores.tt
// No modificar este archivo directamente, edita el archivo .tt para regenerar los controladores
// Requiere los siguientes espacios de nombres

using Microsoft.AspNetCore.Mvc;
using Servidor.Controllers;
using Servidor.src.Controllers;
using Servidor.src.Model;

namespace Servidor.src.Controllers.AutoGenerados;
[ApiController]
[Route("api/[controller]")]
public class ClasificacionController : BaseController<Clasificacion> { }


[ApiController]
[Route("api/[controller]")]
public class ElementoJerarquicoController : BaseController<ElementoJerarquico> { }


[ApiController]
[Route("api/[controller]")]
public class IdentificadorController : BaseController<Identificador> { }


[ApiController]
[Route("api/[controller]")]
public class ProductoController : BaseController<Producto> { }


[ApiController]
[Route("api/[controller]")]
public class ProveedorController : BaseController<Proveedor> { }


[ApiController]
[Route("api/[controller]")]
public class RecepcionCargaController : BaseController<RecepcionCarga> { }

