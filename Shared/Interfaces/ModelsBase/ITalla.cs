using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces.ModelsBase
{
    public interface ITalla : IIdentifiable
    {
        string Name { get; }
        string Description { get; }
        List<IClase> ClasesPermitidas { get; }
    }
}
