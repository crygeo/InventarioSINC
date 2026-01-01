using System.Linq.Expressions;
using MongoDB.Driver;

namespace Shared.Interfaces;

public interface IUpdateProperty<TEntity>
{
    Task<bool> UpdateProperty(string entityId,
        string selector,
        object newPropertyValue);

    
}
