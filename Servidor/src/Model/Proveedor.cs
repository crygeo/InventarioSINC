using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Attributes;
using Shared.Interfaces.Model;
using Shared.Interfaces.Model.Obj;

namespace Servidor.Model;

[AutoController]
public class Proveedor : IProveedor
{
    public bool VerView { get; set; } = true;

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    public string RUC { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;

    public bool EsEmpresa { get; set; }

    // Empresa
    public string RazonSocial { get; set; } = string.Empty;
    public string RepresentanteLegal { get; set; } = string.Empty;

    // Persona
    public string PrimerNombre { get; set; } = string.Empty;
    public string SegundoNombre { get; set; } = string.Empty;
    public string PrimerApellido { get; set; } = string.Empty;
    public string SegundoApellido { get; set; } = string.Empty;
    public string Cedula { get; set; } = string.Empty;
    public string Celular { get; set; } = string.Empty;
    public DateTime FechaNacimiento { get; set; } = DateTime.Now;

    public bool Deleteable { get; set; } = true;
    public bool Updatable { get; set; } = true;

    public string NombreCompleto =>
        $"{PrimerNombre} {SegundoNombre} {PrimerApellido} {SegundoApellido}"
            .Replace("  ", " ")
            .Trim();

    public void Update(IModelObj entity)
    {
        if (entity is not Proveedor other)
            throw new ArgumentException("Entidad no válida para actualización", nameof(entity));

        RUC = other.RUC;
        Direccion = other.Direccion;
        EsEmpresa = other.EsEmpresa;

        RazonSocial = other.RazonSocial;
        RepresentanteLegal = other.RepresentanteLegal;

        PrimerNombre = other.PrimerNombre;
        SegundoNombre = other.SegundoNombre;
        PrimerApellido = other.PrimerApellido;
        SegundoApellido = other.SegundoApellido;
        Cedula = other.Cedula;
        Celular = other.Celular;
        FechaNacimiento = other.FechaNacimiento;

        Deleteable = other.Deleteable;
        Updatable = other.Updatable;
        VerView = other.VerView;
    }
}