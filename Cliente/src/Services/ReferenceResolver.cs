using Cliente.Services.Model;
using Utilidades.Interfaces;
using Utilidades.Mvvm;

namespace Cliente.Services;

public class ReferenceResolver : IReferenceResolver
{
    public async Task<object?> ResolveAsync(Type type, string id)
    {
        var service = (IServiceClient)ServiceFactory.GetService(type);

        var result = await service.GetByIdAsync(id);

        return result.Success ? result.EntityGet : null;
    }
}