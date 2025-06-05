using Shared.Interfaces.ModelsBase;
using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Interfaces.Model.Obj;

namespace Servidor.src.Objs
{
    public class ProveedorEmpresa : IProveedorEmpresa
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string RUC { get; set; }
        public string Direccion { get; set; }
        public string RazonSocial { get; set; }
        public string RepresentanteLegal { get; set; }
        public bool Deleteable { get; set; }
        public bool Updatable { get; set; }
        public void Update(IModelObj entity)
        {
            throw new NotImplementedException();
        }
    }

    public class ProveedorPersona : IProveedorPersona
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string RUC { get; set; }
        public string Direccion { get; set; }

        public string PrimerNombre { get; set; }

        public string SegundoNombre { get; set; }

        public string PrimerApellido { get; set; }

        public string SegundoApellido { get; set; }

        public string Cedula { get; set; }
        public string Celular { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public bool Deleteable { get; set; }
        public bool Updatable { get; set; }
        public void Update(IModelObj entity)
        {
            throw new NotImplementedException();
        }
    }
}
