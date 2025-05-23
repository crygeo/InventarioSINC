using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces.ModelsBase
{
    public interface IClasificacion: IIdentifiable
    {
        IRecepcionCarga RecepcionCarga { get; }
        ICalidad Calidad { get; set; }
        float PesoDesecho { get; set; }
        float PesoNeto { get; set; }
        float PesoBruto { get; }
        float PesoBrutoProcesado { get; }
        float Rendimiento { get; }
        IReadOnlyList<ICoche> CochesList { get; }

        void AgregarProducto(IEmpaqueUnidad empaqueUnidad);
        void QuitarProducto(IEmpaqueUnidad empaqueUnidad);

    }
}
