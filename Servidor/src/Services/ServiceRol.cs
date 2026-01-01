using System.Collections.Generic;
using System.Threading.Tasks;
using Servidor.Model;
using Servidor.Statics;

namespace Servidor.Services;

public class ServiceRol : ServiceBase<Rol>
{
    public const string GENERAL_ROLE_ID = "6650c6a2b5cf184a0a8a0f3b";

    public Task<List<string>> GetAllPermisos()
    {
        return Task.FromResult(Permisos.List);
    }

    private async Task ActualizarRolGeneral()
    {
        var roles = await Repository.GetByIdAsync(GENERAL_ROLE_ID);
        if (roles != null)
        {
            roles.Permisos = Permisos.List;
            await Repository.UpdateAsync(roles.Id, roles);
            return;
        }

        // Crear un rol general por defecto si no existe

        var rolGeneral = new Rol
        {
            Id = GENERAL_ROLE_ID,
            Nombre = "General",
            Permisos = Permisos.List,
            IsAdmin = true,
            Deleteable = false,
            Updatable = false
        };

        await Repository.CreateAsync(rolGeneral);
    }

    public override async Task InitServiceAsync()
    {
        await ActualizarRolGeneral();
    }
}