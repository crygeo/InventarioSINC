using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.ComponentModel;
using Servidor.src.Repositorios;
using Servidor.src.Objs;

namespace Servidor.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : BaseController<Usuario>
    {

        public UsuariosController(RepositorioUsuario repositorio) : base(repositorio)
        {
        }


    }
}
