using Microsoft.Extensions.DependencyInjection;

namespace Shared.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class AutoServiceAttribute : Attribute
{
    public ServiceLifetime Lifetime { get; }

    public AutoServiceAttribute(ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        Lifetime = lifetime;
    }
}