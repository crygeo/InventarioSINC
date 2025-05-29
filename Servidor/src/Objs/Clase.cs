using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Interfaces.ModelsBase;
using System;

namespace Servidor.src.Objs
{
    [Obsolete("Esta clase está deshabilitada. No debe usarse.", true)]
    public class Clase : IClase
    {
        public string Name { get; set; }

        public string Description { get; set; }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public bool Deleteable { get; set; } = true; // Indica si se puede eliminar o no
    }
}
