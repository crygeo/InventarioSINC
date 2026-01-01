using System.Reflection;
using Cliente.Attributes;
using Cliente.Obj;
using Cliente.ViewModel.Model;
using Shared.Interfaces.Model;
using Utilidades.Interfaces;

namespace Cliente.Helpers;

public static class NavegacionFactory
{
    // ===============================
    //   CACHÉ
    // ===============================
    private static readonly Lazy<List<ItemNavigationM>> _cacheLista = new(CrearTodos);

    private static readonly Lazy<Dictionary<string, List<ItemNavigationM>>> _cachePorIndicador =
        new(ConstruirCachePorIndicador);

    // ===============================
    //   MÉTODO QUE YA TENÍAS
    // ===============================
    public static List<ItemNavigationM> CrearTodos()
    {
        var ls = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(IModelObj).IsAssignableFrom(t)
                        && typeof(ISelectable).IsAssignableFrom(t)
                        && t is { IsClass: true, IsAbstract: false }
                        && t.GetCustomAttribute<NavegacionAttribute>() != null)
            .Select(t =>
            {
                var method = typeof(NavegacionFactory)
                    .GetMethod(nameof(CrearItemNavigation))!
                    .MakeGenericMethod(t);

                return method.Invoke(null, null) as ItemNavigationM;
            })
            .Where(i => i != null)!
            .ToList();

        if (ls.Count == 0)
            throw new InvalidOperationException(
                "No se encontraron clases con NavegacionAttribute.");

        return ls;
    }

    // ===============================
    //   CREACIÓN DEL ITEM INDIVIDUAL
    // ===============================
    public static ItemNavigationM CrearItemNavigation<T>()
        where T : class, IModelObj, ISelectable, new()
    {
        var tipoEntidad = typeof(T);

        var attr = tipoEntidad.GetCustomAttribute<NavegacionAttribute>();

        if (attr == null)
            throw new InvalidOperationException(
                $"La clase {tipoEntidad.Name} no tiene NavegacionAttribute.");

        var viewModel = ViewModelFactory.GetViewModel<T>();

        return new ItemNavigationM
        {
            Indicador = attr.Indicador, // 👈 EL CAMPO CLAVE
            Title = attr.TituloS,
            SelectedIcon = attr.SelectedIcon,
            UnselectedIcon = attr.UnselectedIcon,
            Notification = attr.Notification,
            Page = viewModel
        };
    }

    // ===============================
    //   AGRUPADOR AUTOMÁTICO
    // ===============================
    private static Dictionary<string, List<ItemNavigationM>> ConstruirCachePorIndicador()
    {
        return _cacheLista.Value
            .GroupBy(i => i.Indicador)
            .ToDictionary(g => g.Key, g => g.ToList());
    }

    // ===============================
    //   MÉTODOS PÚBLICOS
    // ===============================

    /// <summary>Obtiene todos los items generados.</summary>
    public static List<ItemNavigationM> ObtenerTodos()
    {
        return _cacheLista.Value;
    }

    /// <summary>Obtiene los items correspondientes a un indicador.</summary>
    public static List<ItemNavigationM> ObtenerTodosPorIndicador(string indicador)
    {
        return _cachePorIndicador.Value.TryGetValue(indicador, out var lista)
            ? lista
            : new List<ItemNavigationM>();
    }
}