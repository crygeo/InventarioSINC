using System;
using System.Threading.Tasks;

namespace Utilidades.Mvvm;

public interface IReferenceResolver
{
    Task<object> ResolveAsync(Type type, string id);
}