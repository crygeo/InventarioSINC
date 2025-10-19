
using Shared.Attributes;
using System.Runtime.CompilerServices;

namespace Shared.Enums;

public enum TypeItem
{
    [Display("UNIQUE", "Todos")]
    UNIQUE,

    [Display("BAG", "Bolsa")]
    BAG,

    [Display("HEAD", "Cabeza")]
    HEAD,

    [Display("CAPEITEM", "Capa")]
    CAPEITEM,

    [Display("MAIN", "Mano Principal")]
    MAIN,

    [Display("ARMOR", "Armadura")]
    ARMOR,

    [Display("OFF", "Mano Secundaria")]
    OFF,

    [Display("POTION", "Poción")]
    POTION,

    [Display("SHOES", "Botas")]
    SHOES,

    [Display("MEAL", "Comida")]
    MEAL,

    [Display("MOUNT", "Montura")]
    MOUNT,

    [Display("2H", "Ambas Manos")]
    H2,

    [Display("SKIN", "Skin")]
    SKIN,
}


public enum Calidades
{

    [Display("Normal", "Normal")]
    Unknown = 0,

    [Display("Normal", "Normal")]
    Normal = 1,

    [Display("Bueno", "Bueno")]
    Bueno = 2,

    [Display("MuyBueno", "Muy Bueno")]
    MuyBueno = 3,

    [Display("Sobresaliente", "Sobresaliente")]
    Sobresaliente = 4,

    [Display("ObraMaestra", "Obra Maestra")]
    ObraMaestra = 5
}

public enum TierType
{
    [Display("NUll", "Sin Tier")]
    N0 = 0,
    [Display("T1", "Tier 1")]
    T1 = 1,
    [Display("T2", "Tier 2")]
    T2 = 2,
    [Display("T3", "Tier 3")]
    T3 = 3,
    [Display("T4", "Tier 4")]
    T4 = 4,
    [Display("T5", "Tier 5")]
    T5 = 5,
    [Display("T6", "Tier 6")]
    T6 = 6,
    [Display("T7", "Tier 7")]
    T7 = 7,
    [Display("T8", "Tier 8")]
    T8 = 8
}

public enum EncanType
{
    [Display("", "Sin Encantamiento")]
    E0,

    [Display("@1", "Encantamiento 1")]
    E1,

    [Display("@2", "Encantamiento 2")]
    E2,

    [Display("@3", "Encantamiento 3")]
    E3,

    [Display("@4", "Encantamiento 4")]
    E4
}
public enum Categoria
{
    other
}

public static class Enums
{
    public static bool TryParseEncan(this string encant, out EncanType result)
    {
        result = EncanType.E0; // valor por defecto

        // Si viene vacío → corresponde a Sin Encantamiento (E0)
        if (string.IsNullOrWhiteSpace(encant))
            return true;

        // Normalizamos: "1" -> "@1", "2" -> "@2" ...
        string normalizado = "@" + encant;

        foreach (var value in Enum.GetValues(typeof(EncanType)).Cast<EncanType>())
        {
            var display = value.GetDisplay();
            if (display?.NombreOriginal == normalizado)
            {
                result = value;
                return true;
            }
        }

        return false;
    }

}

public static class EnumExtensions
{
    public static DisplayAttribute? GetDisplay(this Enum value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString());
        return fieldInfo?
            .GetCustomAttributes(typeof(DisplayAttribute), false)
            .FirstOrDefault() as DisplayAttribute;
    }
}