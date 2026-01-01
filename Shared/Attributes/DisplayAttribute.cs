namespace Shared.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class DisplayAttribute : Attribute
{
    public DisplayAttribute(string nombreOriginal, string nombreParaMostrar)
    {
        NombreOriginal = nombreOriginal;
        NombreParaMostrar = nombreParaMostrar;
    }

    public string NombreOriginal { get; }
    public string NombreParaMostrar { get; }
}