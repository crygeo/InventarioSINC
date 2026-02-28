using System.Threading.Tasks;

namespace Utilidades.Interfaces;

public interface IActivable
{
    Task ActivateAsync();
}

public interface IDeactivable
{
    Task DeactivateAsync();
}