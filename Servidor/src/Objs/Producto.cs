using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Interfaces;
using Shared.Interfaces.ModelsBase;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Servidor.src.Objs
{
    public class Producto : IProducto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public string NickName { get; set; } = String.Empty;
        public string Descripcion { get; set; } = String.Empty;
        public bool Deleteable { get; set; } = true;
        public IEnumerable<AtributosEntity> Atributos { get; set; } = [];
        public IEnumerable<Variantes> Variantes { get; set; } = [];

        IEnumerable<IValorAtributo> IProducto.Atributos
        {
            get => Atributos;
            set => Atributos = value.Cast<AtributosEntity>();
        }

        IEnumerable<IVariantes> IProducto.Variantes
        {
            get => Variantes;
            set => Variantes = value.Cast<Variantes>();
        }

        public bool Updatable { get; set; } = true;
        public void Update(IModelObj entity)
        {
            throw new NotImplementedException();
        }
    }
}
