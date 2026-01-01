using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Attributes;
using Shared.Interfaces.Model;
using Shared.Interfaces.Model.Obj;

namespace Servidor.Model;

[AutoController]
public class Clasificacion : IClasificacion
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    public string IdRecepcionCarga { get; set; } = string.Empty;
    public float PesoDesecho { get; set; }
    public float PesoNeto { get; set; }
    public bool Deleteable { get; set; }
    public bool Updatable { get; set; }

    public void Update(IModelObj entity)
    {
        throw new NotImplementedException();
    }
}