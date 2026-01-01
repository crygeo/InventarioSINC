namespace Shared.Interfaces.Model.Obj;

public interface IElementoJerarquico : IModelObj
{
    string IdPerteneciente { get; set; }
    string Nombre { get; set; }
    string Valor { get; set; }
    string Descripcion { get; set; }
    DateTime FechaCreacion { get; set; }
}