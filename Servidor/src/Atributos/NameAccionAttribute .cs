using System;

namespace Servidor.src.Atributos;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class NameAccionAttribute : Attribute
{
    public NameAccionAttribute(string accion)
    {
        Accion = accion;
    }

    public string Accion { get; }
}