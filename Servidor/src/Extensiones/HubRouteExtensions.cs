using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;

namespace Servidor.src.Extensiones
{
    public static class HubRouteExtensions
    {
        public static void MapHubByConvention<THub>(this IEndpointRouteBuilder app) where THub : Hub
        {
            string name = typeof(THub).Name;
            if (name.StartsWith("Hub"))
                name = name.Substring(3);
            app.MapHub<THub>($"/hub{name}");
        }
    }
}
