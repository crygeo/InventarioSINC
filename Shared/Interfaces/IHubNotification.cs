using Shared.Interfaces.ModelsBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
    public interface IHubNotification<T> where T : IModelObj
    {
        Task NewItem(T obj);
        Task UpdateItem(T obj);
        Task DeleteItem(T obj);
    }
}
