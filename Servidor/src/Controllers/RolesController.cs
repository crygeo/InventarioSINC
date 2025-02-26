using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Servidor.src.Repositorios;
using Servidor.src.Objs;

namespace Servidor.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : BaseController<Rol>
    {

        public RolesController(RepositorioRol repositorio) : base(repositorio)
        {
        }


    }
}
