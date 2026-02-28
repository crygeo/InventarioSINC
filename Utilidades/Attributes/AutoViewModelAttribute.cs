using System;

namespace Utilidades.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class AutoViewModelAttribute : Attribute
{
    public string Prefix { get; init; } = "Page";
    public string Suffix { get; init; } = "VM";
    
    public Type? ViewType { get; }
    
    public AutoViewModelAttribute(Type? view = null)
    {
        ViewType = view;
    }
}