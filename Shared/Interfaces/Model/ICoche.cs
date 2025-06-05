using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces.ModelsBase
{
    public interface ICoche : IModelObj
    {
        int Numero { get; set; }
        int Descripcion { get; set; }
        float PesoBruto { get => ProductoUnidad.Sum((a) => a.PesoBruto); }
        IReadOnlyList<IEmpaqueUnidad> ProductoUnidad { get; set; }
    }
}
