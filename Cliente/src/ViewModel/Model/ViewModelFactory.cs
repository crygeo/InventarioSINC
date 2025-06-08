using Shared.Factory;
using Shared.Interfaces.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cliente.Obj.Model;
using Utilidades.Interfaces;
using Utilidades.Mvvm;

namespace Cliente.src.ViewModel.Model
{
    public static class ViewModelFactory
    {

        public static ViewModelServiceBase<TEntity> GetViewModel<TEntity>() where TEntity : class, IModelObj, ISelectable, new()
        {
            return FactoryResolver.Resolve<ViewModelServiceBase<TEntity>>();
        }

    }
}
