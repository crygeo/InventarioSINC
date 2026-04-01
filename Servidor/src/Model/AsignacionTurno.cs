
using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Attributes;
using Shared.Interfaces.Model;
using Shared.Interfaces.Model.Obj;

namespace Servidor.Model;

[AutoController]
public class AsignacionTurno : IAsignacionTurno
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonRepresentation(BsonType.ObjectId)]
    public string IdEmpleado { get; set; } = string.Empty;

    [BsonRepresentation(BsonType.ObjectId)]
    public string IdTurnoTrabajo { get; set; } = string.Empty;

    public DateTime FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
    public bool Deleteable { get; set; } = true;
    public bool Updatable { get; set; } = true;

    public void Update(IModelObj entity)
    {
        if (entity is not AsignacionTurno other) return;
        IdEmpleado = other.IdEmpleado;
        IdTurnoTrabajo = other.IdTurnoTrabajo;
        FechaDesde = other.FechaDesde;
        FechaHasta = other.FechaHasta;
    }
}