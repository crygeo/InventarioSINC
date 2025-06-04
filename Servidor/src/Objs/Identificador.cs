using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Interfaces;
using Shared.Interfaces.ModelsBase;

namespace Servidor.src.Objs
{
    public class Identificador: IIdentificador
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string NickName { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public bool Deleteable { get; set; } = true;
        public IEnumerable<IAtributo> Atributos { get; set; }

        public bool Updatable { get; set; }
        public void Update(IModelObj entity)
        {
            throw new NotImplementedException();
        }
    }
}
