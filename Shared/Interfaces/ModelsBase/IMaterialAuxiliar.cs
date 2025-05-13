using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces.ModelsBase
{
    public enum TipoMaterialAuxiliar
    {
        Bin,
        Gaveta,
        Pallet,
        Otro
    }

    public interface IMaterialAuxiliar
    {
        TipoMaterialAuxiliar Tipo { get; set; }
        int Cantidad { get; set; }
        string Nota { get; set; }
    }
}
