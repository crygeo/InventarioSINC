using Shared.ClassModel;
using Shared.Interfaces.Model;

namespace Shared.Interfaces;

/// <summary>
/// Define las operaciones básicas de tipo CRUD (Crear, Leer, Actualizar y Eliminar)
/// para una entidad persistente.
/// </summary>
/// <typeparam name="TEntity">
/// Tipo de entidad que implementa <see cref="IModelObj"/>.
/// </typeparam>
public interface ICrud<TEntity> where TEntity : IModelObj
{
    /// <summary>
    /// Obtiene todos los registros de la entidad de forma asíncrona.
    /// </summary>
    /// <returns>
    /// Colección de entidades recuperadas desde la fuente de datos.
    /// </returns>
    Task<IEnumerable<TEntity>> GetAllAsync();

    /// <summary>
    /// Obtiene una entidad por su identificador único.
    /// </summary>
    /// <param name="id">Identificador único de la entidad.</param>
    /// <returns>
    /// La entidad encontrada si existe; en caso contrario, null.
    /// </returns>
    Task<TEntity?> GetByIdAsync(string id);

    /// <summary>
    /// Crea una nueva entidad en la fuente de datos.
    /// </summary>
    /// <param name="entity">Entidad que se desea persistir.</param>
    /// <returns>
    /// Devuelve true si la operación fue exitosa; de lo contrario, false.
    /// </returns>
    Task<bool> CreateAsync(TEntity entity);

    /// <summary>
    /// Actualiza una entidad existente en la fuente de datos.
    /// </summary>
    /// <param name="id">Identificador único de la entidad a actualizar.</param>
    /// <param name="entity">Entidad con los nuevos valores.</param>
    /// <returns>
    /// Devuelve true si la operación fue exitosa; de lo contrario, false.
    /// </returns>
    Task<bool> UpdateAsync(string id, TEntity entity);

    /// <summary>
    /// Elimina una entidad de la fuente de datos según su identificador.
    /// </summary>
    /// <param name="id">Identificador único de la entidad a eliminar.</param>
    /// <returns>
    /// Devuelve true si la operación fue exitosa; de lo contrario, false.
    /// </returns>
    Task<bool> DeleteAsync(string id);

    /// <summary>
    /// Obtiene un conjunto paginado de entidades.
    /// </summary>
    /// <param name="page">Número de página a recuperar (basado en 1).</param>
    /// <param name="pageSize">Cantidad de registros por página.</param>
    /// <returns>
    /// Resultado paginado que contiene los elementos solicitados y metadatos de paginación.
    /// </returns>
    Task<PagedResult<TEntity>> GetPagedAsync(int page, int pageSize);
}