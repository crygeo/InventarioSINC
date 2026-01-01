using Shared.Interfaces.Model;

namespace Shared.Interfaces;

public interface IUpdatable
{
    bool Updatable { get; set; }
    void Update(IModelObj entity);
}