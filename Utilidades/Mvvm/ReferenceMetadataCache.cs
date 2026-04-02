using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Utilidades.Attributes;

namespace Utilidades.Mvvm;

public static class ReferenceMetadataCache
{
    private static readonly ConcurrentDictionary<Type, List<ReferenceMetadata>> _cache = new();

    public static List<ReferenceMetadata> Get(Type type)
    {
        return _cache.GetOrAdd(type, t =>
        {
            return t.GetProperties()
                .Select(p => new
                {
                    Prop = p,
                    Vista = p.GetCustomAttribute<VistaAttribute>()
                })
                .Where(x => x.Vista?.LookupType != null)
                .Select(x =>
                {
                    var propType = x.Prop.PropertyType;
                    var isCollection =
                        typeof(IEnumerable).IsAssignableFrom(propType)
                        && propType != typeof(string);

                    return new ReferenceMetadata
                    {
                        Property    = x.Prop,
                        TargetType  = x.Vista!.LookupType!,
                        IsCollection = isCollection,
                        DisplayMember = x.Vista.DisplayMember ?? "Nombre"
                    };
                })
                .ToList();
        });
    }
}