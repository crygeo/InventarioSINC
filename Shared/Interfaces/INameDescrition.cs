using Shared.Interfaces.ModelsBase;

namespace Shared.Interfaces;

public interface INameDescrition : INickName
{
    string Descripcion { get; set; }
}