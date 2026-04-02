using System.Collections.Generic;
using System.Linq;
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
        var model    = wrapper.Model;

        foreach (var meta in metadata)
        {
            var rawValue = meta.Property.GetValue(model);

            if (meta.IsCollection)
            {
                var ids = (rawValue as IEnumerable<string>) ?? Enumerable.Empty<string>();
                var resolved = new List<object>();

                foreach (var id in ids)
                {
                    if (string.IsNullOrEmpty(id)) continue;
                    var item = await _resolver.ResolveAsync(meta.TargetType, id);
                    if (item != null) resolved.Add(item);
                }

                wrapper.Ref.Set(meta.Property.Name, resolved);
            }
            else
            {
                var id = rawValue as string;
                if (string.IsNullOrEmpty(id)) continue;

                var value = await _resolver.ResolveAsync(meta.TargetType, id);
                wrapper.Ref.Set(meta.Property.Name, value);
            }
        }
    }
}