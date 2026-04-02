using System.Threading.Tasks;

namespace Utilidades.Interfaces;

/// <summary>
/// Fábrica de diálogos para desacoplar VMs de la capa de UI concreta.
/// Permite testing y sustitución sin romper contratos.
/// </summary>
public interface IDialogFactory<TEntity> where TEntity : class
{
    Task<TEntity> ShowCreateDialogAsync(TEntity entity);
    Task<TEntity?> ShowEditDialogAsync(TEntity entity);
    Task<bool> ShowDeleteConfirmAsync(TEntity entity);
}