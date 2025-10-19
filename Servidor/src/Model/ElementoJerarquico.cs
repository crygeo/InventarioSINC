using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Interfaces.Model;
using Shared.Interfaces.Model.Obj;

namespace Servidor.src.Model
{
    public class ElementoJerarquico : IElementoJerarquico
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdPerteneciente { get; set; }
        public string Nombre { get; set; }
        public string Valor { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Updatable { get; set; }
        public bool Deleteable { get; set; }
        public void Update(IModelObj entity)
        {
            throw new System.NotImplementedException();
        }

    }
}
