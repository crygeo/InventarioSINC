using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces.ModelsBase
{
    [Obsolete("Esta clase está deshabilitada. No debe usarse.", true)]
    public interface IClase : IIdentifiable, IDeleteable
    {
        string Name { get; set; }
        string Description { get; set; }
    }
}
