using Cliente.Attributes;

namespace Cliente.Extencions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class EntityChangeExtensions
{
    public static IReadOnlyList<PropertyChange> GetChanges<T>(
        this T original,
        T modified)
    {
        if (original == null)
            throw new ArgumentNullException(nameof(original));
        if (modified == null)
            throw new ArgumentNullException(nameof(modified));

        var type = typeof(T);
        var changes = new List<PropertyChange>();

        var properties = type
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p =>
                p.CanRead &&
                p.CanWrite &&
                p.Name != "Id" &&
                !p.IsDefined(typeof(IgnoreChangeTrackingAttribute), inherit: true) &&
                (
                    p.PropertyType == typeof(string) ||
                    !typeof(IEnumerable).IsAssignableFrom(p.PropertyType)
                )
            );

        foreach (var property in properties)
        {
            var oldValue = property.GetValue(original);
            var newValue = property.GetValue(modified);

            if (AreEqual(property.PropertyType, oldValue, newValue))
                continue;

            changes.Add(new PropertyChange
            {
                Name = property.Name,
                OldValue = oldValue,
                NewValue = newValue
            });
        }

        return changes;
    }

    private static bool AreEqual(Type type, object? a, object? b)
    {
        if (ReferenceEquals(a, b))
            return true;

        if (a == null || b == null)
            return false;

        if (type == typeof(DateTime))
            return ((DateTime)a).Date == ((DateTime)b).Date;

        return a.Equals(b);
    }
}


public sealed class PropertyChange
{
    public string Name { get; init; } = default!;
    public object? OldValue { get; init; }
    public object? NewValue { get; init; }
}
