using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using Servidor.Hubs;

namespace Servidor.Extensiones;

public static class HubRouteExtensions
{
    public static void MapHubByConvention<THub>(this IEndpointRouteBuilder app) where THub : Hub
    {
        var name = typeof(THub).Name;
        if (name.StartsWith("Hub"))
            name = name.Substring(3);
        app.MapHub<THub>($"/hub{name}");
    }

    public static void MapAllGenericHubs<TInterface>(this IEndpointRouteBuilder app)
    {
        // Buscar todos los tipos que implementen IModelObj y no sean abstractos
        var modelos = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(TInterface).IsAssignableFrom(t) && t is { IsClass: true, IsAbstract: false });

        foreach (var modelo in modelos)
        {
            var hubType = typeof(HubBase<>).MakeGenericType(modelo);

            var mapHubMethod = typeof(HubEndpointRouteBuilderExtensions)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .First(m => m.Name == "MapHub" && m.IsGenericMethod && m.GetParameters().Length == 2);

            var genericMap = mapHubMethod.MakeGenericMethod(hubType);

            var name = modelo.Name;
            var path = $"/hub{name}";

            // Ejecuta: app.MapHub<HubBase<T>>("/hub{T}")
            genericMap.Invoke(null, new object[] { app, path });
        }
    }
}