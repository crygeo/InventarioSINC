using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Attributes;
using Shared.Interfaces.Model;
using Shared.Interfaces.Model.Obj;

namespace Servidor.Model;

[AutoController]
public class Identificador : IIdentificador
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Name { get; set; }
    public string Descripcion { get; set; }
    public DateTime FechaCreacion { get; set; }
    public bool Deleteable { get; set; }
    public bool Updatable { get; set; }

    public void Update(IModelObj entity)
    {
        throw new NotImplementedException();
    }
}