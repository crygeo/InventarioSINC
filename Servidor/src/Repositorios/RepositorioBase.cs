using MongoDB.Bson;
using MongoDB.Driver;
using Servidor.src.Objs.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Servidor.src.Repositorios
{
    public abstract class RepositorioBase<TObj> where TObj : IIdentifiable
    {
        private readonly MongoDBConnection _conect;
        public IMongoCollection<TObj> Collection { get; private set; }
        public abstract string NameCollection { get; set; }

        /// <summary>
        /// Constructor base que inicializa la conexión a la colección MongoDB.
        /// </summary>
        public RepositorioBase()
        {
            _conect = new MongoDBConnection();
            Collection = MongoDBConnection._database?.GetCollection<TObj>(NameCollection)
                ?? throw new InvalidOperationException($"No se pudo encontrar la colección: {NameCollection}");
        }

        /// <summary>
        /// Obtiene todos los objetos de la colección.
        /// </summary>
        /// <returns>Lista de objetos.</returns>
        public async virtual Task<IEnumerable<TObj>> GetAll()
        {
            return await Collection.Find(_ => true).ToListAsync();
        }


        /// <summary>
        /// Obtiene un objeto por su ID.
        /// </summary>
        /// <param name="id">ID del objeto.</param>
        /// <returns>El objeto encontrado o null.</returns>
        public async virtual Task<TObj> GetById(string id)
        {
            return await Collection.Find(u => u.Id == id).FirstOrDefaultAsync();
        }


        /// <summary>
        /// Crea un nuevo objeto en la colección.
        /// </summary>
        /// <param name="obj">El objeto a insertar.</param>
        /// <returns>Task completada cuando se inserta el objeto.</returns>
        public async virtual Task Create(TObj obj)
        {
            obj.Id = string.Empty; // MongoDB asignará automáticamente el ID.
            await Collection.InsertOneAsync(obj);
        }

        /// <summary>
        /// Actualiza un objeto existente en la colección.
        /// </summary>
        /// <param name="id">ID del objeto a actualizar.</param>
        /// <param name="obj">Datos del objeto actualizado.</param>
        /// <returns>El resultado de la operación de reemplazo.</returns>
        public async virtual Task<ReplaceOneResult> Update(string id, TObj obj)
        {
            return await Collection.ReplaceOneAsync(u => u.Id == id, obj);
        }

        /// <summary>
        /// Elimina un objeto de la colección.
        /// </summary>
        /// <param name="id">ID del objeto a eliminar.</param>
        /// <returns>El resultado de la operación de eliminación.</returns>
        public async virtual Task<DeleteResult> Delete(string id)
        {
            return await Collection.DeleteOneAsync(u => u.Id == id);
        }
    }
}
