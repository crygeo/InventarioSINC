using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente.src.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SolicitarAttribute : Attribute
    {
        public string Nombre { get; }
        public bool Requerido { get; set; } = false;
        public int MinLength { get; set; } = 0;
        public int MinItem { get; set; } = 0;
        public required Type ItemType { get; set; }
        public SolicitarAttribute(string? nombre)
        {
            Nombre = nombre;
        }

        
    }
}
