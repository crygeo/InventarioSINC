using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Servidor.Model;
using Servidor.Services;

namespace Servidor.Controllers;

public partial class RolController : BaseController<Rol>
{
    private ServiceRol ServiceRol => (ServiceRol)Service;

    [HttpGet("Perms")]
    public async Task<ActionResult> GetPermss()
    {
        var perm = await ServiceRol.GetAllPermisos();
        return Ok(perm);
    }
}