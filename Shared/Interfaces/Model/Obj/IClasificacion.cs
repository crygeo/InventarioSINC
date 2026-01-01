namespace Shared.Interfaces.Model.Obj;

public interface IClasificacion : IModelObj
{
    string IdRecepcionCarga { get; set; }
    float PesoDesecho { get; set; }
    float PesoNeto { get; set; }
}