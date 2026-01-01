using System.Linq.Expressions;

namespace Shared.Interfaces;

public interface IUpdateListProperty<TEntity>
{
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
    Task<bool> AddItemIdToListAsync(
        string entityId,
        string selector,
        string itemId);


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
    Task<bool> RemoveItemToListAsync(
        string entityId,
        string selector,
        string itemId);

}