using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Attributes;
using Shared.Interfaces.Model;
using Shared.Interfaces.Model.Obj;

namespace Servidor.Model;

[AutoController]
public class Turno : ITurno
{
    private string _idTurnoTrabajo;

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public bool Deleteable { get; set; }
    public bool Updatable { get; set; }

    public string Nombre { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string IdTurnoTrabajo { get; set; }

    public bool EsRotativo { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string AreaId { get; set; }
    
    public void Update(IModelObj entity)
    {
    }
}