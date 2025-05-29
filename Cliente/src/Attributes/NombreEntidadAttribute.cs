using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente.src.Attributes
{
    public class NombreEntidadAttribute : Attribute
    {
        public string Singular { get; }
        public string Plural { get; }

        public NombreEntidadAttribute(string singular, string? plural = null)
        {
            Singular = singular;
            Plural = plural ?? singular + "s";
        }
    }
}
