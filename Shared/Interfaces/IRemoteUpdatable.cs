namespace Shared.Interfaces;

public interface IRemoteUpdatable
{
    event Action? OnRemoteUpdated;
}