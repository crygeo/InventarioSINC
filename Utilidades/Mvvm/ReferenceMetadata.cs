using System;
using System.Reflection;

namespace Utilidades.Mvvm;

// Utilidades/Mvvm/ReferenceMetadataCache.cs
public class ReferenceMetadata
{
    public PropertyInfo Property { get; init; } = null!;
    public Type TargetType { get; init; } = null!;
    public bool IsCollection { get; init; }      // ← nuevo
    public string DisplayMember { get; init; } = "Nombre"; // ← nuevo
}