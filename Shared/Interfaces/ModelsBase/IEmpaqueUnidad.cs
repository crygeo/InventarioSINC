using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces.ModelsBase
{
    public interface IEmpaqueUnidad
    {
        IClase Clase { get; set; }
        ITalla Talla { get; set; }
        float PesoUnidad { get; set; }
        int Unidades { get; set; }

        void AgregarUnidades(int cantidad);
        void QuitarUnidades(int cantidad);
    }
}
