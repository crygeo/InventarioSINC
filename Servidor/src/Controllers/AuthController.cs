using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Servidor.src.Helper;
using Servidor.src.Objs;
using Servidor.src.Repositorios;
using Servidor.src.Services;
using Shared.Response;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Servidor.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ServiceUsuario _service;

        public string NamePermiso = "Auth.Login";

        public AuthController(ServiceUsuario service)
        {
            _service = service;
        }

        [HttpGet("validate-token")]
        public async Task<IActionResult> ValidateToken()
        {
            var token = Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
            {
                return Unauthorized("Token no proporcionado o inválido");
            }

            token = token.Substring("Bearer ".Length).Trim(); // Eliminar el prefijo "Bearer " para obtener solo el token

            var principal = GenerateTokenForUser.ValidateToken(token); // Método para verificar y obtener el principal (usuario) del token

            if (principal == null || principal.Identity == null)
            {
                return Unauthorized("Token inválido o expirado");
            }

            string id = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            if (!string.IsNullOrEmpty(id))
            {
                var User = await _service.GetByIdAsync(id);
                if (User == null)
                    return Unauthorized("Usuario no encontrado");

                if (!await _service.VerificarPermiso(User, NamePermiso))
                    return Unauthorized("Usuario no permitido para iniciar seccion.");
            }
            // Obtener la fecha de expiración del token

            var expClaim = principal.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;
            if (!long.TryParse(expClaim, out long expTime))
            {
                return Unauthorized("Error al obtener la expiración del token");
            }

            var expirationTime = DateTimeOffset.FromUnixTimeSeconds(expTime).UtcDateTime;
            var timeRemaining = (expirationTime - DateTime.UtcNow).TotalSeconds;

            // Si el token es válido, puedes devolver la información del usuario o un nuevo token
            return Ok(new TokenValidationResponse { Message = "Sesión válida", User = principal.Identity.Name ?? "", TimeRemaining = timeRemaining });
        }

        [HttpGet("check")]
        public IActionResult Check()
        {
            return Ok(new { message = "El servidor está funcionando correctamente" });
        }

        [HttpPost("Login")]
        [ActionName("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            if (string.IsNullOrWhiteSpace(login.User) || string.IsNullOrWhiteSpace(login.Password))
            {
                return BadRequest("Usuario y contraseña son requeridos");
            }

            var usuario = await _service.GetByUser(login.User);


            if (usuario == null || string.IsNullOrWhiteSpace(usuario.Password) || !BCrypt.Net.BCrypt.Verify(login.Password, usuario.Password))
            {
                return Unauthorized("Credenciales inválidas");
            }


            if (!await _service.VerificarPermiso(usuario, NamePermiso))
                return Unauthorized($"Usuario no permitido para iniciar seccion.");

            var token = GenerateTokenForUser.GenerateToken(usuario);

            return Ok(new { token });
        }

    }

}
