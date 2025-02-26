using Microsoft.AspNetCore.Mvc;

namespace Servidor.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet("check")]
        public IActionResult Check()
        {
            return Ok(new { message = "El servidor está funcionando correctamente" });
        }
    }

}
