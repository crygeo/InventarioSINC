namespace Shared.Interfaces.Model.Obj;

public interface IIdentificador : IModelObj
{
    string Name { get; set; }
    string Descripcion { get; set; }
    DateTime FechaCreacion { get; set; }
}