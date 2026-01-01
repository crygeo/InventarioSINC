using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Servidor.Helper;
using Servidor.Model;
using Servidor.Objs;
using Servidor.Services;
using Shared.ObjectsResponse;

namespace Servidor.Controllers;

public partial class UsuarioController : BaseController<Usuario>
{
    private ServiceUsuario ServiceUsuario => (ServiceUsuario)Service;

    [HttpGet("this")]
    public async Task<IActionResult> GetThisUser()
    {
        var userId = User.Claims.FirstOrDefault()?.Value ?? "";

        if (string.IsNullOrWhiteSpace(userId))
            return Unauthorized(new ErrorResponse("No se pudo identificar al usuario actual."));

        var usuario = await ServiceUsuario.GetByIdAsync(userId);
        if (usuario == null) return NotFound(new ErrorResponse("Usuario no encontrado."));

        return Ok(usuario);
    }

    [HttpPut("change-password")]
    [ActionName("CambiarContraseña")]
    public async Task<IActionResult> ChangedPassword([FromBody] ChangedPasswordRequest request)
    {
        try
        {
            var solicitanteId = User.Claims.FirstOrDefault()?.Value ?? "";

            if (!IdValidator.IsValidObjectId(solicitanteId) || !IdValidator.IsValidObjectId(request.UserId))
                return BadRequest(new ErrorResponse("ID(s) no válidos."));

            var esMismoUsuario = solicitanteId == request.UserId;

            if (!esMismoUsuario)
            {
                var validacion = await ValidarUsuarioAsync();
                if (validacion != null) return validacion;
            }

            if (esMismoUsuario)
            {
                if (string.IsNullOrWhiteSpace(request.OldPassword))
                    return BadRequest(new ErrorResponse("Debe ingresar su contraseña actual."));

                var usuario = await ServiceUsuario.GetByIdAsync(solicitanteId);
                if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.OldPassword, usuario.Password))
                    return Unauthorized(new ErrorResponse("La contraseña actual es incorrecta."));
            }

            var actualizado = await ServiceUsuario.ActualizarPasswordAsync(request.UserId, request.NewPassword);
            if (!actualizado)
                return NotFound(new ErrorResponse("No se encontró el usuario o falló la actualización."));

            return Ok(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex + "\n" + request);
            return StatusCode(500, new ErrorResponse("Error al cambiar la contraseña.", ex.Message));
        }
    }

    [HttpPut("asignar-rol")]
    [ActionName("AsignarRol")]
    public async Task<IActionResult> AsignarRol([FromBody] AsignarRolRequest request)
    {
        try
        {
            var solicitanteId = User.Claims.FirstOrDefault()?.Value ?? "";

            if (!IdValidator.IsValidObjectId(solicitanteId) ||
                !IdValidator.IsValidObjectId(request.UserId) ||
                !IdValidator.IsValidObjectId(request.RolId))
                return BadRequest(new ErrorResponse("ID(s) no válidos."));

            var validacion = await ValidarUsuarioAsync();
            if (validacion != null) return validacion;

            await ServiceUsuario.AsignarRol(request.UserId, request.RolId);

            return Ok(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex + "\n" + request);
            return StatusCode(500, new ErrorResponse("Error al asignar el rol.", ex.Message));
        }
    }
}