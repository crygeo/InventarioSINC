using System.Reflection;
using Shared.Factory;
using Shared.Interfaces.Model;

namespace Cliente.ServicesHub;

public class HubInitializer
{
     private static bool _initialized = false;
    private static readonly List<Type> _modelTypes = new();
    private static readonly List<Task> _hubTasks = new();

    /// <summary>
    /// Evento disparado cuando TODOS los hubs están conectados.
    /// </summary>
    public static event Action? OnHubsReady;

    /// <summary>
    /// Inicializa automáticamente todos los hubs basados en modelos con IModelObj.
    /// </summary>
    public static async Task InitializeAsync()
    {
        if (_initialized)
            return;

        DiscoverModelTypes();
        await StartAllHubsAsync();

        _initialized = true;
        OnHubsReady?.Invoke();
    }

    /// <summary>
    /// Descubre automáticamente todas las clases del dominio que implementan IModelObj.
    /// </summary>
    private static void DiscoverModelTypes()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        var allModels = assemblies
            .SelectMany(a =>
            {
                try { return a.GetTypes(); }
                catch (ReflectionTypeLoadException ex) { return ex.Types.Where(t => t != null)!; }
            })
            .Where(t =>
                t is { IsClass: true, IsAbstract: false } &&
                typeof(IModelObj).IsAssignableFrom(t))
            .ToList();

        // Orden natural
        _modelTypes.Clear();
        _modelTypes.AddRange(allModels);

        Console.WriteLine("Modelos detectados automáticamente:");
        foreach (var m in _modelTypes)
            Console.WriteLine($" - {m.Name}");
    }

    /// <summary>
    /// Inicia todos los hubs para los modelos detectados.
    /// </summary>
    private static async Task StartAllHubsAsync()
    {
        foreach (var type in _modelTypes)
            _hubTasks.Add(StartHubForModelAsync(type));

        await Task.WhenAll(_hubTasks);

        Console.WriteLine("Todos los hubs están conectados.");
    }

    /// <summary>
    /// Inicia el hub correspondiente a un modelo de manera segura.
    /// </summary>
    private static async Task StartHubForModelAsync(Type modelType)
    {
        try
        {
            var hubServiceType = typeof(HubServiceBase<>).MakeGenericType(modelType);
            dynamic hubService = FactoryResolver.Resolve(hubServiceType);

            await hubService.StartConnectionAsync();

            Console.WriteLine($"Hub iniciado para: {modelType.Name}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] No se pudo iniciar el hub de {modelType.Name}: {ex.Message}");
        }
    }
}