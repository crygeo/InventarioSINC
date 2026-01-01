using System.Collections.ObjectModel;
using Cliente.Obj.Model;

namespace Cliente.Extencions;

public static class ListRolExtencions
{
    public static void SelectRol(this List<Rol> rols, List<string> rolId)
    {
        foreach (var rol in rols) rol.IsSelect = rolId.Contains(rol.Id);
    }

    public static void SelectRol(this ObservableCollection<Rol> rols, List<string> rolId)
    {
        foreach (var rol in rols) rol.IsSelect = rolId.Contains(rol.Id);
    }
}