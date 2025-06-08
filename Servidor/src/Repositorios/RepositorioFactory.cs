using Shared.Interfaces;
using Shared.Interfaces.Model;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using Shared.Factory;

namespace Servidor.src.Repositorios
{
    public static class RepositorioFactory
    {
        public static RepositorioBase<T> GetRepositorio<T>() where T : class, IModelObj
        {
            return FactoryResolver.Resolve<RepositorioBase<T>>();
        }

    }
}