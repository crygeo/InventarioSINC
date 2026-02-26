using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Attributes;
using Shared.Interfaces;
using Shared.Interfaces.Model;
using Shared.Interfaces.Model.Obj;

namespace Servidor.Model;

[AutoController]
public class Area : IArea
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public bool Deleteable { get; set; }
    public bool Updatable { get; set; }

    public string Nombre { get; set; }

    public void Update(IModelObj entity)
    {
        throw new NotImplementedException();
    }
}