using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using Utilidades.Interfaces;

namespace Utilidades.Mvvm;

public class ReferenceContainer : ObservableObject, IReferenceContainer
{
    private readonly Dictionary<string, object> _data = new();

    public object? Get(string key)
        => _data.TryGetValue(key, out var value) ? value : null;

    public void Set(string key, object? value)
    {
        if (_data.TryGetValue(key, out var existing) && Equals(existing, value))
            return;

        _data[key] = value;

        // Notifica binding tipo: Ref[Proveedor]
        OnPropertyChanged($"Item[{key}]");
    }

    // 🔥 clave para binding en WPF
    public object? this[string key] => Get(key);
}