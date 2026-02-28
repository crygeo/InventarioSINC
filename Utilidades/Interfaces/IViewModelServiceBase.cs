using System;
using System.Threading.Tasks;

namespace Utilidades.Interfaces;

public interface IViewModelServiceBase<TEntity>  : ICrudViewModel, IPagedViewModel, IActivable, IDeactivable, IDisposable
{

}