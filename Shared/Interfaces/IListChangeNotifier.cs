namespace Shared.Interfaces;

public interface IListChangeNotifier
{
    Task ItemAddedAsync(
        string entityId,
        string propertyName,
        string itemId);

    Task ItemRemovedAsync(
        string entityId,
        string propertyName,
        string itemId);
}
