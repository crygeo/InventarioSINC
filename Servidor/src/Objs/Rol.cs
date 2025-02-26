using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Servidor.src.Objs.Interfaces;

namespace Servidor.src.Objs
{
    public class Rol : IIdentifiable
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } // ID del usuario
        public string Nombre { get; set; } // Ejemplo: "Administrador", "Editor", "Usuario"
        public List<string> Permisos { get; set; } = new List<string>(); // Ejemplo: ["Usuarios.Crear", "Usuarios.Eliminar"]

    }
}
