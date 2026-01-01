using System;
using Shared.Interfaces.Model;
using Utilidades.Interfaces;

namespace Cliente.ViewModel.Model;

/// <summary>
/// Fábrica central de ViewModels.
/// La tabla de resolución (_map) es generada automáticamente
/// por el Source Generator.
/// </summary>
public static partial class ViewModelFactory
{
    /// <summary>
    /// Obtiene una instancia del ViewModel asociado a la entidad TEntity.
    /// </summary>
    /// <typeparam name="TEntity">
    /// Tipo de entidad de dominio.
    /// </typeparam>
    /// <exception cref="InvalidOperationException">
    /// Se lanza si no existe un ViewModel registrado para la entidad.
    /// </exception>
    public static ViewModelServiceBase<TEntity> GetViewModel<TEntity>()
        where TEntity : class, IModelObj, ISelectable, new()
    {
        if (_map.TryGetValue(typeof(TEntity), out var factory))
            return (ViewModelServiceBase<TEntity>)factory();

        throw new InvalidOperationException(
            $"No hay ViewModel registrado para {typeof(TEntity).Name}");
    }
}