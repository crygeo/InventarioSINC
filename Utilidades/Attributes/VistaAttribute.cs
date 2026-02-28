using System;

namespace Utilidades.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class VistaAttribute : Attribute
{
    public VistaAttribute(string nombre = null, int orden = 0)
    {
        Nombre = nombre;
        Orden = orden;
    }

    public string Nombre { get; set; }
    public bool Visible { get; set; } = true;
    public int Orden { get; set; }
    public Type LookupType { get; set; } = null;
}