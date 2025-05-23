using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Servidor.src.Helper;
using Servidor.src.Objs;
using Servidor.src.Repositorios;
using Servidor.src.Services;
using Shared.ObjectsResponse;
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
            try
            {
                var token = Request.Headers["Authorization"].ToString();

                if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
                {
                    return Unauthorized(new ErrorResponse(401, "Token no proporcionado o inválido"));
                }

                token = token.Substring("Bearer ".Length).Trim();

                var principal = GenerateTokenForUser.ValidateToken(token);

                if (principal?.Identity == null)
                {
                    return Unauthorized(new ErrorResponse(401, "Token inválido o expirado"));
                }

                string id = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
                if (!string.IsNullOrEmpty(id))
                {
                    var user = await _service.GetByIdAsync(id);
                    if (user == null)
                        return Unauthorized(new ErrorResponse(401, "Usuario no encontrado"));

                    if (!await _service.VerificarPermiso(user, NamePermiso))
                        return Unauthorized(new ErrorResponse(401, "Usuario no permitido para iniciar sesión"));
                }

                var expClaim = principal.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;
                if (!long.TryParse(expClaim, out long expTime))
                {
                    return Unauthorized(new ErrorResponse(401, "Error al obtener la expiración del token"));
                }

                var expirationTime = DateTimeOffset.FromUnixTimeSeconds(expTime).UtcDateTime;
                var timeRemaining = (expirationTime - DateTime.UtcNow).TotalSeconds;

                return Ok(new TokenValidationResponse
                {
                    Message = "Sesión válida",
                    User = principal.Identity.Name ?? "",
                    TimeRemaining = timeRemaining
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse(500, "Ocurrió un error al validar el token", ex.Message));
            }
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
            try
            {
                if (string.IsNullOrWhiteSpace(login.User) || string.IsNullOrWhiteSpace(login.Password))
                {
                    return BadRequest(new ErrorResponse(400, "Usuario y contraseña son requeridos"));
                }

                var usuario = await _service.GetByUser(login.User);

                if (usuario == null || string.IsNullOrWhiteSpace(usuario.Password) || !BCrypt.Net.BCrypt.Verify(login.Password, usuario.Password))
                {
                    return Unauthorized(new ErrorResponse(401, "Credenciales inválidas"));
                }

                if (!await _service.VerificarPermiso(usuario, NamePermiso))
                {
                    return Unauthorized(new ErrorResponse(401, "Usuario no permitido para iniciar sesión"));
                }

                var token = GenerateTokenForUser.GenerateToken(usuario);

                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse(500, "Ocurrió un error durante el inicio de sesión", ex.Message));
            }
        }
    }
}
