using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces.ModelsBase
{
    public interface ICalidad : IIdentifiable
    {
        string Nombre { get; set; } //A1, A2, A3
        string Descripcion { get; set; } //Se indica en que caso se usa.
    }
}
