using Shared.Factory;
using Shared.Interfaces.Model;

namespace Servidor.Repositorios;

public static class RepositorioFactory
{
    public static RepositorioBase<T> GetRepositorio<T>() where T : class, IModelObj
    {
        return FactoryResolver.Resolve<RepositorioBase<T>>();
    }
}