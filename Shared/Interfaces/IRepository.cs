using MongoDB.Driver;
using Shared.Interfaces.ModelsBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace Shared.Interfaces
{
    public interface IRepository<TObj> : ICrud<TObj> where TObj : IModelObj
    {
        IMongoCollection<TObj> Collection { get; }
        string NameCollection { get; }
        
    }

}
