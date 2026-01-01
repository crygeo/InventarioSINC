using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Attributes;
using Shared.Interfaces.Model;
using Shared.Interfaces.ModelsBase;

namespace Servidor.Model;

[AutoController]
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