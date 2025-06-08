using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Interfaces.Model;
using Shared.Interfaces.Model.Obj;
using Shared.Interfaces.ModelsBase;
using System;
using System.Collections.Generic;

namespace Servidor.src.Model
{
    public class RecepcionCarga : IRecepcionCarga
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public IEnumerable<string> IdIdentificadores { get; set; }
        public string IdProveedor { get; set; }
        public DateTime FechaIngreso { get; set; }
        public IEnumerable<ICarga> Camiones { get; set; }
        public float PesoTotal { get; set; }
        public byte[]? GuiaGlobal { get; set; }
        public string Nota { get; set; }
        public bool Updatable { get; set; }
        public bool Deleteable { get; set; }
        public bool VerView { get; set; }
        public void Update(IModelObj entity)
        {
            throw new NotImplementedException();
        }

    }
}
