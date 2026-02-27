namespace Shared.Request;

/// <summary>
/// Petición de búsqueda de texto libre.
/// El cliente recoge las propiedades marcadas con [Buscable] del tipo de entidad
/// y las incluye aquí junto con el texto a buscar.
/// El servidor aplica un filtro OR regex sobre esas propiedades en MongoDB.
/// </summary>
public sealed class SearchRequest
{
    /// <summary>
    /// Texto libre a buscar. Puede contener múltiples palabras separadas por espacio.
    /// Ejemplo: "Geovanny 2350946592"
    /// </summary>
    public string Query { get; init; } = string.Empty;

    /// <summary>
    /// Nombres exactos de las propiedades de la entidad sobre las que se buscará.
    /// Ejemplo: ["PrimerNombre", "SegundoNombre", "PrimerApellido", "SegundoApellido", "Cedula"]
    /// </summary>
    public List<string> Propiedades { get; init; } = [];

    /// <summary>
    /// Cantidad máxima de resultados a devolver. Por defecto 20.
    /// </summary>
    public int PageSize { get; init; } = 20;
}