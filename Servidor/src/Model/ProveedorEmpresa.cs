using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Interfaces.Model.Obj;
using Shared.Interfaces.Model;

namespace Servidor.src.Model
{
    public class ProveedorEmpresa : IProveedorEmpresa
    {
        private bool _verView;

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string RUC { get; set; }
        public string Direccion { get; set; }
        public string RazonSocial { get; set; }
        public string RepresentanteLegal { get; set; }
        public bool Deleteable { get; set; }
        public bool Updatable { get; set; }
        public bool VerView { get; set; }
        public void Update(IModelObj entity)
        {
            throw new NotImplementedException();
        }


    }


}
