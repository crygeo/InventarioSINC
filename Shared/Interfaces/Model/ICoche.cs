using Shared.Interfaces.Model;

namespace Shared.Interfaces.ModelsBase;

public interface ICoche : IModelObj
{
    int Numero { get; set; }
    int Descripcion { get; set; }

    float PesoBruto => ProductoUnidad.Sum(a => a.PesoBruto);

    IReadOnlyList<IEmpaqueUnidad> ProductoUnidad { get; set; }
}