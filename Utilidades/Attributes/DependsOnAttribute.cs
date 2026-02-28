using System;

namespace Utilidades.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public sealed class DependsOnAttribute : Attribute
{
    public string PropertyName { get; }

    public DependsOnAttribute(string propertyName)
    {
        PropertyName = propertyName;
    }
}