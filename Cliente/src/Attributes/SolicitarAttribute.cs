using Utilidades.Controls;

namespace Cliente.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class SolicitarAttribute : Attribute
{
    public string Nombre { get; set; }
    public bool Requerido { get; set; } = false;
    public int MinLength { get; set; } = 0;
    public int MaxLength { get; set; } = 0;
    public int MinItem { get; set; } = 0;
    public required Type ItemType { get; set; }
    public required InputBoxType InputBoxConvert { get; set; }
    public SolicitarAttribute(string? nombre)
    {
        Nombre = nombre;
    }

        
}