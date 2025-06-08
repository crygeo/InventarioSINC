using System.Reflection;
using Cliente.Attributes;
using Cliente.Obj.Model;
using Cliente.src.ViewModel.Model;
using Cliente.ViewModel;
using Shared.Factory;
using Shared.Interfaces.Model;
using Utilidades.Interfaces;
using Utilidades.Mvvm;

namespace Cliente.Helpers;

public static class NavegacionFactory
{
    public static ItemNavigationM CrearItemNavigation<T>() where T : class, IModelObj, ISelectable, new()
    {
        var tipoEntidad = typeof(T);

        var attr = tipoEntidad.GetCustomAttribute<NavegacionAttribute>();

        if (attr == null) throw new InvalidOperationException($"La clase {tipoEntidad.Name} no tiene el atributo NavegacionAttribute.");

        // Crear el ViewModel correspondiente usando FactoryResolver
        var viewModel = ViewModelFactory.GetViewModel<T>();

        return new ItemNavigationM
        {
            Title = attr.TituloS,
            SelectedIcon = attr.SelectedIcon,
            UnselectedIcon = attr.UnselectedIcon,
            Notification = attr.Notification,
            Page = viewModel
        };
    }


    public static List<ItemNavigationM> CrearTodos()
    {
        var ls = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(IModelObj).IsAssignableFrom(t)
                        && typeof(ISelectable).IsAssignableFrom(t)
                        && t is { IsClass: true, IsAbstract: false }
                        && t.GetCustomAttribute<NavegacionAttribute>() != null)
            .Select(t =>
            {
                var method = typeof(NavegacionFactory).GetMethod(nameof(CrearItemNavigation))!
                    .MakeGenericMethod(t);
                return method.Invoke(null, null) as ItemNavigationM;
            })
            .Where(i => i != null)!
            .ToList();

        if(ls.Any() == false)
            throw new InvalidOperationException("No se encontraron clases que implementen IModelObj e ISelectable con el atributo NavegacionAttribute.");

        return ls;
    }

}