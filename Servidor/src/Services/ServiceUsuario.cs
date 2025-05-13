using Microsoft.AspNetCore.Mvc;
using Servidor.src.Hubs;
using Servidor.src.HubsService;
using Servidor.src.Objs;
using Servidor.src.Repositorios;
using Shared.Extensions;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Servidor.src.Services
{
    public class ServiceUsuario : ServiceBase<Usuario>
    {

        public override IRepository<Usuario> Repository { get; } = new RepositorioUsuario();
        public override IHubService<Usuario> HubService { get; }


        public ServiceUsuario(HubsServiceUsuario hubService)
        {
            HubService = hubService;
        }
        public Task<Usuario> GetByUser(string user) => ((RepositorioUsuario)Repository).GetByUser(user);
        
        public async Task<bool> ActualizarPasswordAsync(string userId, string newPassword)
        {
            RepositorioUsuario repUser = (RepositorioUsuario)Repository;
            var result = await repUser.ActualizarPassword(userId, newPassword);
            //HubService.CerrarTodasLasConexiones(userId);

            return result;
        }

        public async Task<bool> AsignarRol(string userId, string rolId)
        {
            var usuario = await Repository.GetByIdAsync(userId);
            if (usuario == null) return false;
            
            usuario.Roles.AddOrRemove(rolId);
            await Repository.UpdateAsync(userId, usuario);
            await HubService.UpdateItem(usuario);
            return true;
        }



    }
}
