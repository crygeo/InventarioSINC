using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servidor.src.Helper;
using Servidor.src.Objs;
using Servidor.src.Repositorios;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Servidor.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly RepositorioUsuario _usuarioRepositorio;

        public AuthController(RepositorioUsuario usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        [HttpGet("validate-token")]
        public IActionResult ValidateToken()
        {
            var token = Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
            {
                return Unauthorized(new { message = "Token no proporcionado o inválido" });
            }

            token = token.Substring("Bearer ".Length).Trim(); // Eliminar el prefijo "Bearer " para obtener solo el token

            var principal = JwtHelper.ValidateToken(token); // Método para verificar y obtener el principal (usuario) del token

            if (principal == null || principal.Identity == null)
            {
                return Unauthorized(new { message = "Token inválido o expirado" });
            }

            // Obtener la fecha de expiración del token

            var expClaim = principal.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;
            if (!long.TryParse(expClaim, out long expTime))
            {
                return Unauthorized(new { message = "Error al obtener la expiración del token" });
            }

            var expirationTime = DateTimeOffset.FromUnixTimeSeconds(expTime).UtcDateTime;
            var timeRemaining = (expirationTime - DateTime.UtcNow).TotalSeconds;

            // Si el token es válido, puedes devolver la información del usuario o un nuevo token
            return Ok(new { message = "Sesión válida", user = principal.Identity.Name, timeRemaining });
        }



        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            if (string.IsNullOrWhiteSpace(login.User) || string.IsNullOrWhiteSpace(login.Password))
            {
                return BadRequest(new { message = "Usuario y contraseña son requeridos" });
            }

            var usuario = await _usuarioRepositorio.GetByUser(login.User);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(login.Password, usuario.Password))
            {
                return Unauthorized(new { message = "Credenciales inválidas" });
            }

            var token = JwtHelper.GenerateToken(usuario);

            return Ok(new { token });
        }

        [Authorize]
        [HttpGet("perfil")]
        public async Task<IActionResult> GetPerfil()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "Acceso no autorizado." });
            }

            var usuario = await _usuarioRepositorio.GetById(userId);
            return usuario != null ? Ok(usuario) : NotFound(new { message = "Usuario no encontrado." });
        }

        [Authorize]
        [HttpGet("protected-resource")]
        public IActionResult GetProtectedResource()
        {
            // Aquí se validará el token automáticamente con el middleware de autenticación
            return Ok(new { message = "Acceso permitido" });
        }


    }

}
