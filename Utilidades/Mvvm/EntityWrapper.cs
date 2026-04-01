using CommunityToolkit.Mvvm.ComponentModel;

namespace Utilidades.Mvvm;

public class EntityWrapper<T> : ObservableObject
{
    public T Model { get; }

    public ReferenceContainer Ref { get; } = new();

    public EntityWrapper(T model)
    {
        Model = model;
    }
}