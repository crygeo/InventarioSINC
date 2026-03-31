namespace Shared.Interfaces.Model.Obj;

/// <summary>
/// Contrato que implementan las entidades capaces de contener
/// empleados directamente (Seccion sin grupos, Grupo).
/// El VM de empleados trabaja contra esta interfaz — no conoce
/// el tipo concreto del padre.
/// </summary>
public interface IEmpleadoContainer : IModelObj
{
    IList<string> EmpleadoIds { get; set; }
    string NombreContexto { get; }   // para el header de la vista
}