using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces.ModelsBase
{
    public interface IPersona
    {
        string PrimerNombre { get; set; }
        string SegundoNombre { get; set; }
        string PrimerApellido { get; set; }
        string SegundoApellido { get; set; }
        string Cedula { get; set; }
        string Celular { get; set; }
        DateTime FechaNacimiento { get; set; }
    }
}
