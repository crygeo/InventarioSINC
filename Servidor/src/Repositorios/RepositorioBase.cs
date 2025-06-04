using MongoDB.Bson;
using MongoDB.Driver;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Interfaces.ModelsBase;

namespace Servidor.src.Repositorios
{
    public abstract class RepositorioBase<TObj> : IRepository<TObj> where TObj : IModelObj
    {
        public IMongoCollection<TObj> Collection { get; }

        public string NameCollection => $"Rep{typeof(TObj).Name}";

        /// <summary>
        /// Constructor base que inicializa la conexión a la colección MongoDB.
        /// </summary>
        public RepositorioBase()
        {

            _ = new MongoDBConnection(); // Inicializa la conexión a la base de datos
            Collection = MongoDBConnection._database?.GetCollection<TObj>(NameCollection)
                ?? throw new InvalidOperationException($"No se pudo encontrar la colección: {NameCollection}");
        }

        /// <summary>
        /// Obtiene todos los objetos de la colección.
        /// </summary>
        /// <returns>Lista de objetos.</returns>
        public virtual async Task<IEnumerable<TObj>> GetAllAsync()
        {
            try
            {
                return await Collection.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener todos los documentos: {ex.Message}");
                return Enumerable.Empty<TObj>(); // Retorna una lista vacía en caso de error
            }
        }

        /// <summary>
        /// Obtiene un objeto por su ID.
        /// </summary>
        /// <param name="id">ID del objeto.</param>
        /// <returns>El objeto encontrado o null.</returns>
        public virtual async Task<TObj?> GetByIdAsync(string id)
        {
            try
            {
                return await Collection.Find(x => x.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el documento con ID {id}: {ex.Message}");
                return default;
            }
        }

        /// <summary>
        /// Crea un nuevo objeto en la colección.
        /// </summary>
        /// <param name="obj">El objeto a insertar.</param>
        /// <returns>Task completada cuando se inserta el objeto.</returns>
        public virtual async Task<bool> CreateAsync(TObj entity)
        {
            try
            {
                await Collection.InsertOneAsync(entity);
                return true;  // Inserción exitosa
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al insertar el documento: {ex.Message}");
                return false; // Fallo en la inserción
            }
        }

        /// <summary>
        /// Actualiza un objeto existente en la colección.
        /// </summary>
        /// <param name="id">ID del objeto a actualizar.</param>
        /// <param name="obj">Datos del objeto actualizado.</param>
        /// <returns>El resultado de la operación de reemplazo.</returns>
        public virtual async Task<bool> UpdateAsync(string id, TObj entity)
        {
            try
            {
                var result = await Collection.ReplaceOneAsync(x => x.Id == id, entity);
                return result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el documento con ID {id}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Elimina un objeto de la colección.
        /// </summary>
        /// <param name="id">ID del objeto a eliminar.</param>
        /// <returns>El resultado de la operación de eliminación.</returns>
        public virtual async Task<bool> DeleteAsync(string id)
        {
            try
            {
                var obj = await GetByIdAsync(id);
                if (obj == null || !obj.Deleteable) return false; // Si el objeto no existe, no se puede eliminar

                var result = await Collection.DeleteOneAsync(x => x.Id == id);
                return result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el documento con ID {id}: {ex.Message}");
                return false;
            }
        }
        
    }
}
