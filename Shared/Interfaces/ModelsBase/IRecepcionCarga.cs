using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces.ModelsBase
{
    public interface IRecepcionCarga: IModelObj, IIdentificadoresEntity
    {
        IProveedor Proveedor { get; set; }
        DateTime FechaIngreso { get; set; }
        List<ICarga> Camiones { get; set; }
        float PesoTotal { get; set; }
        byte[]? GuiaGlobal { get; set; } // en IRecepcionCarga
        string Nota { get; set; }
        void CerrarEntrega();
    }
}
