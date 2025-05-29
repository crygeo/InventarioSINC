using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Interfaces.ModelsBase;

namespace Servidor.src.Objs
{
    [Obsolete("Esta clase está deshabilitada. No debe usarse.", true)]
    public class Calidad : ICalidad
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty; // ID de la calidad
        public string Nombre { get; set; } = ""; // Ejemplo: "A1", "A2", "A3"
        public string Descripcion { get; set; } = ""; // Se indica en que caso se usa.
        public bool Deleteable { get; set; } = true; // Indica si se puede eliminar o no

    }
}
