namespace Shared.Interfaces.Model.Obj;

public interface IRol : IModelObj
{
    string Nombre { get; set; } // Ejemplo: "Administrador", "Editor", "Usuario"
    List<string> Permisos { get; set; } // Ejemplo: ["Usuarios.Crear", "Usuarios.Eliminar"]
    bool IsAdmin { get; set; }
}