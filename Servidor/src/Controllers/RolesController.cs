using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Servidor.src.Repositorios;
using Servidor.src.Objs;
using Servidor.src.Services;
using Microsoft.AspNetCore.Authorization;

namespace Servidor.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController(ServiceRol servicioRol) : BaseController<Rol>(servicioRol)
    {
        [HttpGet("Perms")]
        public async Task<ActionResult> GetPermss()
        {
            var perm = await servicioRol.GetAllPermisos();
            return Ok(perm);
        }
    }
}
