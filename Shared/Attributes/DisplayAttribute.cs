using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class DisplayAttribute : Attribute
{
    public string NombreOriginal { get; }
    public string NombreParaMostrar { get; }

    public DisplayAttribute(string nombreOriginal, string nombreParaMostrar)
    {
        NombreOriginal = nombreOriginal;
        NombreParaMostrar = nombreParaMostrar;
    }
}

