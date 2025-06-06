using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Interfaces.Model;

namespace Shared.Interfaces
{
    public interface IController<TEntity, IResult> where TEntity : IModelObj
    {
        IService<TEntity> Service { get; }

        Task<IResult> GetAllAsync();
        Task<IResult> GetByIdAsync(string id);
        Task<IResult> CreateAsync(TEntity entity);
        Task<IResult> UpdateAsync(string id, TEntity entity);
        Task<IResult> DeleteAsync(string id);
    }
}
