using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Interfaces.Model;
using Shared.Interfaces.Model.Obj;
using Shared.Interfaces.ModelsBase;
using System;
using System.Collections.Generic;

namespace Servidor.src.Objs
{
    public class RecepcionCarga : IRecepcionCarga
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;
        public IProveedor Proveedor { get; set; } = new ProveedorPersona();
        public DateTime FechaIngreso { get; set; }
        public List<ICarga> Camiones { get; set; } = [];
        public float PesoTotal { get; set; }
        public byte[]? GuiaGlobal { get; set; }
        public string Nota { get; set; } = string.Empty;
        public IEnumerable<IIdentificador> Identificadores { get; set; } = [];
        public bool Deleteable { get; set; } = true;
        public bool Updatable { get; set; } = true;
        public void Update(IModelObj entity)
        {
            throw new NotImplementedException();
        }
        public void CerrarEntrega()
        {
            throw new NotImplementedException();
        }
    }
}
