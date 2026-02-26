using System;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Attributes;
using Shared.Interfaces.Model;
using Shared.Interfaces.Model.Obj;

namespace Servidor.Model;

[AutoController]
public class Empleado : IEmpleado
{
    public string EstadoEmpleadoId { get; set; }
    public string EstadoActualNombre { get; set; }
    public DateTime FechaIngreso { get; set; }

    public bool EstaActivo { get; set; }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public bool Deleteable { get; set; }
    public bool Updatable { get; set; }
    public string PrimerNombre { get; set; }
    public string? SegundoNombre { get; set; }
    public string PrimerApellido { get; set; }
    public string? SegundoApellido { get; set; }
    public string Cedula { get; set; }
    public string Celular { get; set; }
    public DateTime FechaNacimiento { get; set; }

    [NotMapped]
    public string NombreCompleto =>
        $"{PrimerNombre} {SegundoNombre} {PrimerApellido} {SegundoApellido}"
            .Replace("  ", " ")
            .Trim();

    public void Update(IModelObj entity)
    {
        throw new NotImplementedException();
    }
}