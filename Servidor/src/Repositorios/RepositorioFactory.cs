using Servidor.src.Factory;
using Shared.Interfaces;
using Shared.Interfaces.Model;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

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