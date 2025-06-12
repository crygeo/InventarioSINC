﻿using Microsoft.AspNetCore.Mvc;
using Servidor.src.Hubs;
using Servidor.src.HubsService;
using Servidor.src.Repositorios;
using Shared.Extensions;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Servidor.Services;
using Servidor.src.Model;

namespace Servidor.src.Services
{
    public class ServiceUsuario : ServiceBase<Usuario>
    {

        public static readonly string ADMIN_ID = "6650c6a2b5cf184a0a8a0f3a";

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

        private async Task CrearUsuarioAdmin()
        {
            var usuarios = await Repository.GetByIdAsync(ADMIN_ID);

            if (usuarios != null) return; // Si ya existe el usuario admin, no hacemos nada
            // Crear un usuario admin por defecto si no existe
            var usuarioAdmin = new Usuario
            {
                PrimerNombre = "",
                SegundoNombre = "",
                PrimerApellido = "",
                SegundoApellido = "",
                Cedula = "",
                Celular = "",
                FechaNacimiento = DateTime.Now,
                Id = ADMIN_ID,
                Password = BCrypt.Net.BCrypt.HashPassword("admin"),
                Roles = new List<string> {ServiceRol.GENERAL_ROLE_ID},
                User = "admin",
                Deleteable = false
            };

            await Repository.CreateAsync(usuarioAdmin);
        }

        override public async Task InitServiceAsync()
        {
            await CrearUsuarioAdmin();
        }


    }
}
