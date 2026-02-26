using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace Servidor.Statics;

public static class Permisos
{
    private static List<string>? _list;

    public static List<string> List
    {
        get
        {
            if (_list == null)
                _list = GetPermisos();
            return _list;
        }
    }

    private static List<string> GetPermisos()
    {
        var permisos = new HashSet<string>();

        var assembly = Assembly.GetExecutingAssembly();

        var controllers = assembly.GetTypes()
            .Where(t =>
                typeof(ControllerBase).IsAssignableFrom(t) &&
                !t.IsAbstract);

        foreach (var controller in controllers)
        {
            var controllerName = controller.Name.Replace("Controller", "");

            var methods = controller.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(m =>
                    !m.IsDefined(typeof(NonActionAttribute)) &&
                    !m.IsSpecialName);

            foreach (var method in methods)
            {
                var actionName = NormalizeActionName(method.Name);
                permisos.Add($"{controllerName}.{actionName}");
            }
        }

        return permisos.ToList();
    }
    
    private static string NormalizeActionName(string methodName)
    {
        return methodName.EndsWith("Async")
            ? methodName[..^5]
            : methodName;
    }


}