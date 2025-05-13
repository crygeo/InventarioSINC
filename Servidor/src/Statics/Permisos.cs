using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Servidor.src.Statics
{
    public class Permisos
    {
        private static List<string>? _list = null;
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
            var list = new List<string>();
            var assambly = Assembly.GetEntryAssembly();
            var controllers = assambly?.GetTypes().Where(t => typeof(ControllerBase).IsAssignableFrom(t) && !t.IsAbstract);

            if (controllers == null) return list;
            foreach (var controller in controllers)
            {
                var methods = controller.GetMethods(BindingFlags.Instance | BindingFlags.Public);
                foreach (var method in methods)
                {
                    if (method.CustomAttributes.Any(a => a.AttributeType == typeof(ActionNameAttribute)))
                    {
                        var actionName = method.GetCustomAttribute<ActionNameAttribute>();
                        var FullName = $"{controller?.Name.Replace("Controller", "")}.{actionName?.Name}";
                        list.Add(FullName);
                    }
                }
            }
            return list;
        }
    }
}
