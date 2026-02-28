using System.Text.RegularExpressions;

namespace Utilidades.Converters;

public static class StringExtensions
{
    private const int MaxLengName = 20; // Máximo de caracteres para nombres y apellidos

    public static string ToRUC(this string text)
    {
        text = text.QuitarEspacios().ToNumber().MaxLength(13);

        if (text.Length < 13)
            return text; // No aplicar formato si está incompleto

        var tipo = text[2]; // tercer dígito define el tipo

        return tipo switch
        {
            '6' => FormatoSociedadPublica(text),
            '9' => FormatoSociedadPrivada(text),
            _ => FormatoPersonaNatural(text)
        };
    }

    public static string FromRUC(this string text)
    {
        return text.ToNumber().MaxLength(13);
    }


    public static string ToPhone(this string text)
    {
        text = text.QuitarEspacios().ToNumber();

        text = text.Length > 10 ? text.Substring(0, 10) : text;

        if (text.Length > 6)
            return $"{text[..3]} {text.Substring(3, 3)} {text.Substring(6)}";
        if (text.Length > 3)
            return $"{text[..3]} {text.Substring(3)}";
        return text;
    }

    public static string FromPhone(this string text)
    {
        return text.ToNumber().MaxLength(10);
    }


    public static string ToDni(this string text)
    {
        text = text.QuitarEspacios().ToNumber().MaxLength(10);

        if (text.Length < 10)
            return text;
        // Formatear el DNI correctamente: xxxxxxxxx-x
        return $"{text.Substring(0, 9)}-{text.Substring(9, 1)}";
    }

    public static string FromDni(this string text)
    {
        return text.ToNumber().MaxLength(10);
    }


    public static string ToEmail(this string text)
    {
        return text.ToLower();
    }

    public static string FromEmail(this string text)
    {
        return text.ToEmail();
    }

    public static string ToName(this string text)
    {
        text = text.QuitarEspacios().MaxLength(MaxLengName);

        if (text.Length < 1)
            return text;
        text = text.ToLower(); // Convertimos todo a minúscula y eliminamos espacios extra
        return char.ToUpper(text[0]) + text.Substring(1);
    }

    public static string FromName(this string text)
    {
        return text.ToName();
    }

    public static string ToNickName(this string text)
    {
        return text.QuitarEspacios().MaxLength(MaxLengName);
    }

    public static string FromNickName(this string text)
    {
        return text.ToNickName();
    }

    public static string ToDate(this string text)
    {
        return text;
    }

    public static string FromDate(this string text)
    {
        return text;
    }

    public static string MaxLength(this string text, int max)
    {
        if (text == null)
            return string.Empty;

        return text.Length <= max ? text : text.Substring(0, max);
    }

    public static string ToNumber(this string text)
    {
        // Eliminar cualquier carácter que no sea un número
        //return new string(text.Where(char.IsDigit).ToArray()); // Otra forma de hacerlo: Consume menos recursos
        return Regex.Replace(text, "[^0-9]", ""); //Consume mas recursos pero es mas legible
    }
    public static string FromNumber(this string text)
    {
        return ToNumber(text);
    }

    public static string QuitarEspacios(this string text)
    {
        return Regex.Replace(text, @"\s+", "").Trim();
    }


    private static string FormatoPersonaNatural(string ruc)
    {
        // Ejemplo: 1716537341001 → 1716537341 001
        return $"{ruc.Substring(0, 10)} {ruc[10..]}";
    }

    private static string FormatoSociedadPrivada(string ruc)
    {
        // Ejemplo: 0296537341001 → 02 9 6537341 001
        return $"{ruc.Substring(0, 2)} {ruc[2]} {ruc.Substring(3, 7)} {ruc[10..]}";
    }

    private static string FormatoSociedadPublica(string ruc)
    {
        // Ejemplo: 0266537341001 → 02 6 6537341 001
        return $"{ruc.Substring(0, 2)} {ruc[2]} {ruc.Substring(3, 7)} {ruc[10..]}";
    }
}