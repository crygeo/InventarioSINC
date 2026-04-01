// Servidor/src/Model/TurnoTrabajo.cs

using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Attributes;
using Shared.Interfaces.Model;
using Shared.Interfaces.Model.Obj;

namespace Servidor.Model;

[AutoController]
public class TurnoTrabajo : ITurnoTrabajo
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraSalidaMinima { get; set; }
    public TimeSpan HorasMinimas { get; set; }
    public bool Deleteable { get; set; } = true;
    public bool Updatable { get; set; } = true;

    public void Update(IModelObj entity)
    {
        if (entity is not TurnoTrabajo other) return;
        Nombre = other.Nombre;
        HoraInicio = other.HoraInicio;
        HoraSalidaMinima = other.HoraSalidaMinima;
        HorasMinimas = other.HorasMinimas;
    }
}