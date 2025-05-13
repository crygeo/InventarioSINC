using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.ComponentModel;
using Servidor.src.Repositorios;
using Servidor.src.Objs;
using Servidor.src.Services;
using System.Linq;
using Servidor.src.Atributos;
using Servidor.src.Helper;
using System;

namespace Servidor.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController(ServiceUsuario servicioUsuario) : BaseController<Usuario>(servicioUsuario)
    {
        [HttpGet("this")]
        public async Task<IActionResult> GetThisUser()
        {
            string userId = User.Claims.FirstOrDefault()?.Value ?? "";

            if (userId == null)
            {
                return Unauthorized();
            }
            var usuario = await servicioUsuario.GetByIdAsync(userId);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }

        [HttpPut("change-password")]
        [ActionName("CambiarContraseña")]
        public async Task<IActionResult> ChangedPassword([FromBody] ChangedPasswordRequest request)
        {
            try
            {
                string solicitanteId = User.Claims.FirstOrDefault()?.Value ?? "";

                if (!IdValidator.IsValidObjectId(solicitanteId) || !IdValidator.IsValidObjectId(request.UserId))
                    return BadRequest("ID(s) no válidos.");

                bool esMismoUsuario = solicitanteId == request.UserId;

                // Si el usuario NO se está cambiando su propia contraseña, validar permisos
                if (!esMismoUsuario)
                {
                    var validacion = await ValidarUsuarioAsync();
                    if (validacion != null) return validacion;
                }

                // Si es su propia cuenta, verifica que haya enviado la contraseña actual y que sea válida
                if (esMismoUsuario)
                {
                    if (string.IsNullOrWhiteSpace(request.OldPassword))
                        return BadRequest("Debe ingresar su contraseña actual.");

                    var usuario = await _service.GetByIdAsync(solicitanteId);
                    if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.OldPassword, usuario.Password))
                        return Unauthorized("La contraseña actual es incorrecta.");
                }

                var actualizado = await servicioUsuario.ActualizarPasswordAsync(request.UserId, request.NewPassword);
                if (!actualizado)
                    return NotFound("No se encontró el usuario o falló la actualización.");

                return Ok(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex + "\n" + request.ToString());
                return StatusCode(500, $"Error al cambiar la contraseña. {ex.Message}");
            }
        }

        [HttpPut("asignar-rol")]
        [ActionName("AsignarRol")]
        public async Task<IActionResult> AsignarRol([FromBody] AsignarRolRequest request)
        {
            try
            {
                string solicitanteId = User.Claims.FirstOrDefault()?.Value ?? "";

                if (!IdValidator.IsValidObjectId(solicitanteId) || !IdValidator.IsValidObjectId(request.UserId) || !IdValidator.IsValidObjectId(request.RolId))
                    return BadRequest("ID(s) no válidos.");

                var validacion = await ValidarUsuarioAsync();
                if (validacion != null) return validacion;

                await servicioUsuario.AsignarRol(request.UserId, request.RolId);

                return Ok(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex + "\n" + request.ToString());
                return StatusCode(500, $"Error al cambiar la contraseña. {ex.Message}");
            }
        }
    }
}
