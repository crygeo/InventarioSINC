using Cliente.src.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente.src.Extencions
{
    public static class ListRolExtencions
    {
        public static void SelectRol(this List<Rol> rols, List<string> rolId)
        {
            foreach (var rol in rols)
            {
                rol.IsSelect = rolId.Contains(rol.Id);
            }

        }

        public static void SelectRol(this ObservableCollection<Rol> rols, List<string> rolId)
        {
            foreach (var rol in rols)
            {
                rol.IsSelect = rolId.Contains(rol.Id);
            }

        }
    }
}
