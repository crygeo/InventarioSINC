using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Attributes;
using Shared.Interfaces.Model;
using Shared.Interfaces.Model.Obj;
using Shared.Interfaces.ModelsBase;

namespace Servidor.Model;

[AutoController]
public class RecepcionCarga : IRecepcionCarga
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public IEnumerable<string> IdIdentificadores { get; set; }

    public string IdProveedor { get; set; }
    public DateTime FechaIngreso { get; set; }
    public IEnumerable<ICarga> Camiones { get; set; }
    public float PesoTotal { get; set; }
    public byte[]? GuiaGlobal { get; set; }
    public string Nota { get; set; }
    public bool Updatable { get; set; }
    public bool Deleteable { get; set; }

    public void Update(IModelObj entity)
    {
        throw new NotImplementedException();
    }
}