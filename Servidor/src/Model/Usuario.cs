using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using Shared.Interfaces.Model.Obj;
using Shared.Interfaces.Model;

namespace Servidor.src.Model
{
    public class Usuario : IUsuario
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public bool Deleteable { get; set; }
        public bool Updatable { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Cedula { get; set; }
        public string Celular { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Roles { get; set; }
        public void Update(IModelObj entity)
        {
            throw new NotImplementedException();
        }


    }

}
