using Shared.Interfaces.ModelsBase;

namespace Shared.Interfaces.Model.Obj;

public interface IRecepcionCarga : IModelObj
{
    IEnumerable<string> IdIdentificadores { get; set; } // Valores aplicados como MacroLote, OP, etc.
    string IdProveedor { get; set; } // Referencia al proveedor registrado
    DateTime FechaIngreso { get; set; } // Fecha en que se recibió la carga
    IEnumerable<ICarga> Camiones { get; set; } // Lista de camiones involucrados
    float PesoTotal { get; set; } // Suma de pesos por camión o carga
    byte[]? GuiaGlobal { get; set; } // Documento guía o comprobante
    string Nota { get; set; } // Observaciones adicionales
}