namespace Utilidades.Interfaces;

public interface IReferenceContainer
{
    object? Get(string key);
    void Set(string key, object? value);
}