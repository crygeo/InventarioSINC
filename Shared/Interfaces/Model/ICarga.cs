using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces.ModelsBase
{
    public interface ICarga
    {
        DateTime FechaIngreso { get; set; }
        IVehiculo Vehiculo { get; set; }
        IPersona Chofer { get; set; }
        List<IMaterialAuxiliar> MaterialesAuxiliares { get; set; }
        int Libras { get; set; }
        string Nota { get; set; }
        byte[]? GuiaChofer { get; set; } // en ICarga

    }
}
