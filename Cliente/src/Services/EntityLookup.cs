using System.Collections;
using Cliente.Services.Model;
using Utilidades.Interfaces;

namespace Cliente.Services;

public class EntityLookup : IEntityLookup
{
    private readonly Dictionary<(Type, string), string> _cache = new();

    /*
    Plan (pseudocódigo detallado):

    - Objetivo: separar la lógica de GetDisplayName en varios métodos según el tipo de entrada.
      - Mantener `GetDisplayName(Type entityType, string id)` como manejo de un solo id (existente).
      - Implementar `GetDisplayName(Type entityType, IEnumerable<string> ids)` que:
        - Si la colección es nula, devolver cadena vacía.
        - Filtrar ids nulos o whitespace.
        - Para cada id válido, llamar a `GetDisplayName(Type, string)` para obtener el display.
        - Eliminar resultados vacíos y duplicados.
        - Unir los resultados con ", " y devolver la cadena resultante o vacío si no hay resultados.
      - Implementar `GetDisplayName(Type entityType, object value)` como dispatcher principal que:
        - Si value es nulo devolver cadena vacía.
        - Si value es `string` -> llamar a la sobrecarga de `string`.
        - Si value es `IEnumerable<string>` -> llamar a la sobrecarga de `IEnumerable<string>.
        - Si value es `IEnumerable<object>` -> convertir cada elemento a `string` (si no nulo) y delegar a la sobrecarga de `IEnumerable<string>`.
        - Si value es `IEnumerable` no genérico -> iterar y convertir a `string`.
        - Para cualquier otro tipo -> usar `ToString()` y delegar a la sobrecarga de `string`.
      - Consideraciones:
        - Respetar la cache existente: la sobrecarga de `string` escribe/lee de `_cache`.
        - Evitar excepciones por colecciones nulas o elementos nulos.
        - Mantener estilo y firma pública de los métodos solicitados.
    */

    public string GetDisplayName(Type entityType, string id)
    {
        if (string.IsNullOrWhiteSpace(id)) return string.Empty;

        if (_cache.TryGetValue((entityType, id), out var cached))
            return cached;

        var service = (IServiceClient)ServiceFactory.GetService(entityType);
        var entity = service.GetFromCacheObj(id); // tu estilo


        var display = entity?.ToString() ?? id;

        _cache[(entityType, id)] = display;
        return display;
    }

    public string GetDisplayName(Type entityType, IEnumerable<string> ids)
    {
        if (ids == null) return string.Empty;

        // Normalizar y obtener los display names individuales usando la sobrecarga de string.
        var displays = ids
            .Where(id => !string.IsNullOrWhiteSpace(id))
            .Select(id => GetDisplayName(entityType, id))
            .Where(s => !string.IsNullOrEmpty(s))
            .Distinct()
            .ToArray();

        return displays.Length == 0 ? string.Empty : string.Join("\n", displays);
    }

    public string GetDisplayName(Type entityType, object value)
    {
        if (value == null) return string.Empty;

        // Casos directos
        if (value is string s) return GetDisplayName(entityType, s);
        if (value is IEnumerable<string> stringEnumerable) return GetDisplayName(entityType, stringEnumerable);

        // IEnumerable<object> y IEnumerable no genérico
        if (value is IEnumerable<object> objEnumerable)
        {
            var items = objEnumerable
                .Where(o => o != null)
                .Select(o => o.ToString())
                .Where(str => !string.IsNullOrWhiteSpace(str));
            return GetDisplayName(entityType, items);
        }

        if (value is IEnumerable nonGenericEnumerable)
        {
            var list = new List<string>();
            foreach (var item in nonGenericEnumerable)
            {
                if (item == null) continue;
                var str = item.ToString();
                if (!string.IsNullOrWhiteSpace(str)) list.Add(str);
            }

            return GetDisplayName(entityType, list);
        }

        // Valor único de otro tipo; convertir a string y delegar a la sobrecarga de string.
        return GetDisplayName(entityType, value.ToString());
    }
}