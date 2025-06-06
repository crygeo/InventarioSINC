using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Interfaces;
using Shared.Interfaces.Model;
using Shared.Interfaces.ModelsBase;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Servidor.src.Model
{
    public class Producto : IProducto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Updatable { get; set; }
        public bool Deleteable { get; set; }
        public void Update(IModelObj entity)
        {
            throw new NotImplementedException();
        }
    }
}
