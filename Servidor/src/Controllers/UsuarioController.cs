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

    // =========================
    // USUARIO ACTUAL
    // =========================

    [HttpGet("this")]
    public async Task<IActionResult> GetThisUser()
        => await ExecuteSafeAsync(async () =>
        {
            var userId = User.Claims.FirstOrDefault()?.Value ?? "";

            if (!IdValidator.IsValidObjectId(userId))
                return Unauthorized(new ErrorResponse("No se pudo identificar al usuario actual."));

            var usuario = await ServiceUsuario.GetByIdAsync(userId);
            return usuario != null
                ? Ok(usuario)
                : NotFound(new ErrorResponse("Usuario no encontrado."));
        }, "Error al obtener el usuario actual.");

    // =========================
    // CAMBIAR CONTRASEÑA
    // =========================

    [HttpPut("change-password")]
    [ActionName("CambiarContraseña")]
    public async Task<IActionResult> ChangedPassword([FromBody] ChangedPasswordRequest request)
        => await ExecuteSafeAsync(async () =>
        {
            var solicitanteId = User.Claims.FirstOrDefault()?.Value ?? "";

            // Validación de IDs
            if (!IdValidator.IsValidObjectId(solicitanteId) ||
                !IdValidator.IsValidObjectId(request.UserId))
                return BadRequest(new ErrorResponse("ID(s) no válidos."));

            var esMismoUsuario = solicitanteId == request.UserId;

            // Si NO es el mismo usuario → requiere permiso
            if (!esMismoUsuario)
            {
                var auth = await ValidateUserAsync();
                if (auth != null) return auth;
            }

            // Si ES el mismo usuario → validar contraseña actual
            if (esMismoUsuario)
            {
                if (string.IsNullOrWhiteSpace(request.OldPassword))
                    return BadRequest(new ErrorResponse("Debe ingresar su contraseña actual."));

                var usuario = await ServiceUsuario.GetByIdAsync(solicitanteId);
                if (usuario == null ||
                    !BCrypt.Net.BCrypt.Verify(request.OldPassword, usuario.Password))
                    return Unauthorized(new ErrorResponse("La contraseña actual es incorrecta."));
            }

            var actualizado = await ServiceUsuario.ActualizarPasswordAsync(
                request.UserId,
                request.NewPassword);

            return actualizado
                ? Ok(true)
                : NotFound(new ErrorResponse("No se encontró el usuario o falló la actualización."));
        }, "Error al cambiar la contraseña.");

    // =========================
    // ASIGNAR ROL
    // =========================

    [HttpPut("asignar-rol")]
    [ActionName("AsignarRol")]
    public async Task<IActionResult> AsignarRol([FromBody] AsignarRolRequest request)
        => await ExecuteSafeAsync(async () =>
        {
            var solicitanteId = User.Claims.FirstOrDefault()?.Value ?? "";

            if (!IdValidator.IsValidObjectId(solicitanteId) ||
                !IdValidator.IsValidObjectId(request.UserId) ||
                !IdValidator.IsValidObjectId(request.RolId))
                return BadRequest(new ErrorResponse("ID(s) no válidos."));

            var auth = await ValidateUserAsync();
            if (auth != null) return auth;

            await ServiceUsuario.AsignarRol(request.UserId, request.RolId);

            return Ok(true);
        }, "Error al asignar el rol.");
}
