using Shared.Interfaces.Model;

namespace Shared.Interfaces.ModelsBase;

public interface IProducto : IModelObj
{
    string Name { get; set; }
    string Description { get; set; }
}