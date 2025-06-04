using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Shared.Interfaces.ModelsBase;
using Shared.Interfaces;
using System.Collections.Generic;
using Microsoft.AspNetCore.DataProtection.Repositories;

namespace Servidor.src.Objs
{
    public class Rol : IRol
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty; // ID del usuario
        public string Nombre { get; set; } = ""; // Ejemplo: "Administrador", "Editor", "Usuario"
        public List<string> Permisos { get; set; } = []; // Ejemplo: ["Usuarios.Crear", "Usuarios.Eliminar"]
        public bool IsAdmin { get; set; }
        public bool Deleteable { get; set; }
        public bool Updatable { get; set; }
        public void Update(IModelObj entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
