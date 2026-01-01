using Shared.Interfaces.Model.Obj;

namespace Shared.Interfaces.ModelsBase;

public interface IEmpaqueUnidad
{
    IRecepcionCarga RecepcionCarga { get; }
    string IdValorVariante { get; set; }
    float PesoUnidad { get; set; }

    float PesoBruto => PesoUnidad * Unidades;

    int Unidades { get; set; }

    void AgregarUnidades(int cantidad);
    void QuitarUnidades(int cantidad);
}