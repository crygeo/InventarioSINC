using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces.ModelsBase
{
    public interface IPersona
    {
        string PrimerNombre { get; }
        string SegundoNombre { get; }
        string PrimerApellido { get; }
        string SegundoApellido { get; }
        string Cedula { get; }
        string Celular { get; }
        DateTime FechaNacimiento { get; }
    }
}
