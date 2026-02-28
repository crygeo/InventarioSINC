using System;

namespace Utilidades.Attributes;

/// <summary>
/// Marca las propiedades de una entidad que se usarán
/// como campos de búsqueda en el servidor (regex OR en MongoDB).
/// El cliente recoge estas propiedades y las envía en un <see cref="Shared.Request.SearchRequest"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class BuscableAttribute : Attribute
{
}