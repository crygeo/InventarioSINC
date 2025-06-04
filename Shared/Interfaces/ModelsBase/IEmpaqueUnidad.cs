using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces.ModelsBase
{
    public interface IEmpaqueUnidad
    {
        IRecepcionCarga RecepcionCarga { get; }
        IVariantes Variante { get; set; }
        float PesoUnidad { get; set; }
        float PesoBruto { get => PesoUnidad * Unidades; }
        int Unidades { get; set; }

        void AgregarUnidades(int cantidad);
        void QuitarUnidades(int cantidad);
    }
}
