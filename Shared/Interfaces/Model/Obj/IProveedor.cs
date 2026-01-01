using Shared.Interfaces.ModelsBase;

namespace Shared.Interfaces.Model.Obj;

public interface IProveedor : IModelObj, IEmpresa, IPersona
{
    string RUC { get; set; }
    string Direccion { get; set; }

    bool EsEmpresa { get; set; }
}