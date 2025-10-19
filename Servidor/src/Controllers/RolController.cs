using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Servidor.src.Services;
using Servidor.src.Model;

namespace Servidor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolController() : BaseController<Rol>()
    {
        private ServiceRol ServiceRol => (ServiceRol)Service;

        [HttpGet("Perms")]
        public async Task<ActionResult> GetPermss()
        {
            var perm = await ServiceRol.GetAllPermisos();
            return Ok(perm);
        }
    }
}
