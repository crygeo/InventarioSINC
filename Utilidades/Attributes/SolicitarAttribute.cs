using System;
using Utilidades.Controls;

namespace Utilidades.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class SolicitarAttribute : Attribute
{
    public string Nombre { get; set; }
    public bool Requerido { get; set; } = false;
    public int MinLength { get; set; } = 0;
    public int MaxLength { get; set; } = 0;
    public int MinItem { get; set; } = 0;
    public Type ItemType { get; set; }
    public InputBoxType InputBoxConvert { get; set; }
    public string? VisibleWhen { get; set; }
    public object? VisibleWhenValue { get; set; }
    public Type? EntityType  { get; set; }

    public SolicitarAttribute(string nombre, InputBoxType inputBoxConvert, Type itemType)
    {
        Nombre = nombre;
        InputBoxConvert = inputBoxConvert;
        ItemType = itemType;
    }
}