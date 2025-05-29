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
        public string? Label { get; }

        public SolicitarAttribute(string? label = null)
        {
            Label = label;
        }
    }
}
