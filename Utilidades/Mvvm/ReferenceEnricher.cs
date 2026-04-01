using System.Threading.Tasks;

namespace Utilidades.Mvvm;

public class ReferenceEnricher
{
    private readonly IReferenceResolver _resolver;

    public ReferenceEnricher(IReferenceResolver resolver)
    {
        _resolver = resolver;
    }

    public async Task EnrichAsync<T>(EntityWrapper<T> wrapper)
    {
        var metadata = ReferenceMetadataCache.Get(typeof(T));
        var model = wrapper.Model;

        foreach (var meta in metadata)
        {
            var id = meta.Property.GetValue(model) as string;

            if (string.IsNullOrEmpty(id))
                continue;

            var value = await _resolver.ResolveAsync(meta.TargetType, id);

            wrapper.Ref.Set(meta.Property.Name, value);
        }
    }
}