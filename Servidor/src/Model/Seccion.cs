using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Attributes;
using Shared.Interfaces.Model;
using Shared.Interfaces.Model.Obj;

namespace Servidor.Model;

[AutoController]
public class Seccion : ISeccion
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public bool Deleteable { get; set; }
    public bool Updatable { get; set; }

    public string Nombre { get; set; }
    public int Cupo { get; set; }
    public bool EsGrupo { get; set; }
    public IList<string> EmpleadoIds { get; set; }
    public string TurnoId { get; set; }

    public void Update(IModelObj entity)
    {
    }
}