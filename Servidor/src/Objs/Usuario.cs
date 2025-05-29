using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Shared.Interfaces.ModelsBase;
using System;
using System.Collections.Generic;

namespace Servidor.src.Objs
{
    public class Usuario : IUsuario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        public string PrimerNombre { get; set; } = "";
        public string SegundoNombre { get; set; } = "";
        public string PrimerApellido { get; set; } = "";
        public string SegundoApellido { get; set; } = "";
        public string Cedula { get; set; } = "";
        public string Celular { get; set; } = "";
        public DateTime FechaNacimiento { get; set; } = new();
        public string User { get; set; } = "";
        public string Password { get; set; } = "";

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Roles { get;  set; } = [];
        public bool Deleteable { get; set; }
    }

}
