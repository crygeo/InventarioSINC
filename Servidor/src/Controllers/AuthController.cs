using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Servidor.Helper;
using Servidor.Model;
using Servidor.Objs;
using Servidor.Services;
using Shared.ObjectsResponse;
using Shared.Response;

namespace Servidor.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private ServiceUsuario? _usuarioService;

    public string NamePermiso = "Auth.Login";

    public ServiceUsuario UsuarioService =>
        _usuarioService ??= (ServiceUsuario)ServiceFactory.GetService<Usuario>();


    [HttpGet("validate-token")]
    public async Task<IActionResult> ValidateToken()
    {
        try
        {
            var token = Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
                return Unauthorized(new ErrorResponse(401, "Token no proporcionado o inválido"));

            token = token.Substring("Bearer ".Length).Trim();

            var principal = GenerateTokenForUser.ValidateToken(token);

            if (principal?.Identity == null) return Unauthorized(new ErrorResponse(401, "Token inválido o expirado"));

            var id = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            if (!string.IsNullOrEmpty(id))
            {
                var user = await UsuarioService.GetByIdAsync(id);
                if (user == null)
                    return Unauthorized(new ErrorResponse(401, "Usuario no encontrado"));

                if (!await UsuarioService.VerificarPermiso(user, NamePermiso))
                    return Unauthorized(new ErrorResponse(401, "Usuario no permitido para iniciar sesión"));
            }

            var expClaim = principal.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;
            if (!long.TryParse(expClaim, out var expTime))
                return Unauthorized(new ErrorResponse(401, "Error al obtener la expiración del token"));

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
                return BadRequest(new ErrorResponse(400, "Usuario y contraseña son requeridos"));

            var usuario = await UsuarioService.GetByUser(login.User);

            if (usuario == null || string.IsNullOrWhiteSpace(usuario.Password) ||
                !BCrypt.Net.BCrypt.Verify(login.Password, usuario.Password))
                return Unauthorized(new ErrorResponse(401, "Credenciales inválidas"));

            if (!await UsuarioService.VerificarPermiso(usuario, NamePermiso))
                return Unauthorized(new ErrorResponse(401, "Usuario no permitido para iniciar sesión"));

            var token = GenerateTokenForUser.GenerateToken(usuario);

            return Ok(new { token });
        }
        catch (Exception ex)
        {
            return StatusCode(500,
                new ErrorResponse(500, "Ocurrió un error durante el inicio de sesión", ex.Message));
        }
    }
}