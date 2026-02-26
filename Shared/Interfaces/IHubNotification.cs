using Shared.Interfaces.Model;

namespace Shared.Interfaces;

public interface IHubNotification<T> where T : IModelObj
{
    Task NewItem(T obj);
    Task UpdateItem(T obj);
    Task DeleteItem(T obj);
    Task UpdateProperty(string entityId, string propertyName, object newPropertyValue);
    Task AddItemToListAsync(string entityId, string propertyName, object itemId);
    Task RemoveItemToListAsync(string entityId, string propertyName, object itemId);
}