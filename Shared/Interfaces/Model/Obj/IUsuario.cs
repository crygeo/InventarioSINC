using Shared.Interfaces.ModelsBase;

namespace Shared.Interfaces.Model.Obj;

public interface IUsuario : IModelObj, IPersona
{
    string User { get; set; }
    string Password { get; set; }
    List<string> Roles { get; set; }
}