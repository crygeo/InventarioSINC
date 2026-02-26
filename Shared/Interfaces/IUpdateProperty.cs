using System.Linq.Expressions;
using MongoDB.Driver;

namespace Shared.Interfaces;

public interface IUpdateProperty<TEntity>
{
    /// <summary>
    /// Actualiza una propiedad específica de una entidad en la base de datos.
    /// </summary>
    /// <param name="entityId">
    /// Identificador de la entidad a modificar.
    /// </param>
    /// <param name="selector">
    /// Nombre de la propiedad a actualizar.
    /// </param>
    /// <param name="newPropertyValue">
    /// Nuevo valor para la propiedad.
    /// </param>
    /// <returns></returns>
    Task<bool> UpdateProperty(string entityId,
        string selector,
        object newPropertyValue);

    /// <summary>
    /// Agrega un identificador a una colección de IDs de la entidad indicada,
    /// utilizando una operación atómica en la base de datos.
    /// </summary>
    /// <param name="entityId">
    /// Identificador de la entidad a modificar.
    /// </param>
    /// <param name="selector">
    /// Nombre de la propiedad de la entidad que contiene la colección de IDs.
    /// </param>
    /// <param name="itemId">
    /// Identificador que será agregado a la colección.
    /// </param>
    /// <returns>
    /// <c>true</c> si la entidad existe y la operación fue aplicada correctamente;
    /// de lo contrario, <c>false</c>.
    /// </returns>
    Task<bool> AddItemIdToListAsync(string entityId, string selector, object itemId);


    /// <summary>
    /// Elimina un identificador de una colección de IDs de la entidad indicada,
    /// utilizando una operación atómica en la base de datos.
    /// </summary>
    /// <param name="entityId">
    /// Identificador de la entidad a modificar.
    /// </param>
    /// <param name="selector">
    /// Nombre de la propiedad de la entidad que contiene la colección de IDs.
    /// </param>
    /// <param name="itemId">
    /// Identificador que será eliminado de la colección.
    /// </param>
    /// <returns>
    /// <c>true</c> si la entidad existe y la operación fue aplicada correctamente;
    /// de lo contrario, <c>false</c>.
    /// </returns>
    Task<bool> RemoveItemIdToListAsync(string entityId, string selector, object itemId);
    
    /// <summary>
    /// Elimina un valor específico de una lista (array) en todos los documentos 
    /// donde dicho valor exista.
    /// </summary>
    /// <param name="selector">
    /// Nombre del campo tipo lista (array) del cual se eliminará el valor.
    /// Debe coincidir con el nombre del campo almacenado en la base de datos.
    /// </param>
    /// <param name="itemId">Valor que se eliminará de todos los arreglos que lo contengan.</param>
    /// <returns>
    /// Cantidad de documentos que fueron modificados por la operación.
    /// </returns>
    Task<long> RemoveItemFromAllListsAsync(string selector, object itemId);

    /// <summary>
    /// Elimina múltiples documentos que cumplen con la condición especificada.
    /// Permite realizar eliminaciones masivas utilizando cualquier criterio
    /// soportado por expresiones LINQ traducibles por el proveedor de datos.
    /// </summary>
    /// <param name="predicate">
    /// Expresión que define la condición que deben cumplir los documentos
    /// para ser eliminados.
    /// </param>
    /// <returns>
    /// Cantidad de documentos eliminados.
    /// </returns>
    /// <example>
    /// Ejemplo de uso:
    /// <code>
    /// // Elimina todos los Turnos que pertenezcan al Area con Id "area1"
    /// var eliminados = await serviceTurno
    ///     .RemoveItemsAsync(x => x.AreaId == "area1");
    ///
    /// // Elimina todos los registros inactivos
    /// var eliminadosInactivos = await serviceTurno
    ///     .RemoveItemsAsync(x => !x.Activo);
    /// </code>
    /// </example>
    Task<long> RemoveItemsAsync(
        Expression<Func<TEntity, bool>> predicate);
    
    /// <summary>
    /// Obtiene una lista de entidades que cumplen con el criterio especificado.
    /// </summary>
    /// <param name="predicate">
    /// Expresión que define la condición de filtrado.
    /// </param>
    /// <returns>
    /// Lista de entidades que satisfacen el filtro indicado.
    /// </returns>
    /// <example>
    /// Ejemplo de uso:
    /// <code>
    /// var turnosActivos = await _serviceTurno
    ///     .GetItemsAsync(x => x.AreaId == areaId && x.Activo);
    /// </code>
    /// </example>
    Task<List<TEntity>> GetItemsAsync(
        Expression<Func<TEntity, bool>> predicate);
}
