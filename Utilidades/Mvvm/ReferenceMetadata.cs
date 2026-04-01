using System;
using System.Reflection;

namespace Utilidades.Mvvm;

public class ReferenceMetadata
{
    public PropertyInfo Property { get; init; } = null!;
    public Type TargetType { get; init; } = null!;
}