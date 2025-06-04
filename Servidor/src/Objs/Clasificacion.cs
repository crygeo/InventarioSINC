using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Interfaces.ModelsBase;
using System.Collections.Generic;

namespace Servidor.src.Objs
{
    public class Clasificacion : IClasificacion
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        public bool Deleteable { get; set; }
        public IRecepcionCarga RecepcionCarga { get; set; } = new RecepcionCarga();
        public float PesoDesecho { get; set; }
        public float PesoNeto { get; set; }
        public float PesoBruto { get; }
        public float PesoBrutoProcesado { get; }
        public float Rendimiento { get; }
        public IReadOnlyList<ICoche> CochesList { get; set; } = [];
        public bool Updatable { get; set; }


        public void AgregarProducto(IEmpaqueUnidad empaqueUnidad)
        {
            throw new System.NotImplementedException();
        }

        public void QuitarProducto(IEmpaqueUnidad empaqueUnidad)
        {
            throw new System.NotImplementedException();
        }

        public void Update(IModelObj entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
