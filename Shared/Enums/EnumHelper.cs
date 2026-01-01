using System.Reflection;
using Shared.Attributes;

namespace Shared.Enums;

public static class EnumHelper
{
    public static string GetDisplayName(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attr = field.GetCustomAttribute<DisplayAttribute>();

        return attr?.NombreParaMostrar ?? value.ToString();
    }

    public static string GetNombreOriginal(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attr = field.GetCustomAttribute<DisplayAttribute>();

        return attr?.NombreOriginal ?? value.ToString();
    }
}