using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces.ModelsBase
{
    [Obsolete("Esta clase está deshabilitada. No debe usarse.", true)]
    public interface ICalidad : IIdentifiable, IDeleteable
    {
        string Nombre { get; set; } //A1, A2, A3
        string Descripcion { get; set; } //Se indica en que caso se usa.
    }
}
