using System.Text;

namespace Cliente;

public static class PruebaTemp
{
    private static readonly Dictionary<char, List<string>> asciiBinaryFont = new()
    {
        ['A'] = new List<string> { "01110", "10001", "11111", "10001", "10001" },
        ['B'] = new List<string> { "11110", "10001", "11110", "10001", "11110" },
        ['C'] = new List<string> { "01111", "10000", "10000", "10000", "01111" },
        ['D'] = new List<string> { "11110", "10001", "10001", "10001", "11110" },
        ['E'] = new List<string> { "11111", "10000", "11110", "10000", "11111" },
        ['F'] = new List<string> { "11111", "10000", "11110", "10000", "10000" },
        ['G'] = new List<string> { "01111", "10000", "10011", "10001", "01110" },
        ['H'] = new List<string> { "10001", "10001", "11111", "10001", "10001" },
        ['I'] = new List<string> { "11111", "00100", "00100", "00100", "11111" },
        ['J'] = new List<string> { "00111", "00001", "00001", "10001", "01110" },
        ['K'] = new List<string> { "10001", "10010", "11100", "10010", "10001" },
        ['L'] = new List<string> { "10000", "10000", "10000", "10000", "11111" },
        ['M'] = new List<string> { "10001", "11011", "10101", "10001", "10001" },
        ['N'] = new List<string> { "10001", "11001", "10101", "10011", "10001" },
        ['O'] = new List<string> { "01110", "10001", "10001", "10001", "01110" },
        ['P'] = new List<string> { "11110", "10001", "11110", "10000", "10000" },
        ['Q'] = new List<string> { "01110", "10001", "10001", "10101", "01101" },
        ['R'] = new List<string> { "11110", "10001", "11110", "10010", "10001" },
        ['S'] = new List<string> { "01111", "10000", "01110", "00001", "11110" },
        ['T'] = new List<string> { "11111", "00100", "00100", "00100", "00100" },
        ['V'] = new List<string> { "10001", "10001", "10001", "01010", "00100" },
        ['W'] = new List<string> { "10001", "10001", "10101", "10101", "01010" },
        ['X'] = new List<string> { "10001", "01010", "00100", "01010", "10001" },
        ['Y'] = new List<string> { "10001", "01010", "00100", "00100", "00100" },
        ['Z'] = new List<string> { "11111", "00010", "00100", "01000", "11111" },

        ['0'] = new List<string> { "01110", "10001", "10011", "10101", "01110" },
        ['1'] = new List<string> { "00100", "01100", "00100", "00100", "11111" },
        ['2'] = new List<string> { "01110", "00001", "00010", "00100", "11111" },
        ['3'] = new List<string> { "01110", "00001", "00110", "00001", "01110" },
        ['4'] = new List<string> { "10010", "10010", "11111", "00010", "00010" },
        ['5'] = new List<string> { "11111", "10000", "11110", "00001", "11110" },
        ['6'] = new List<string> { "01110", "10000", "11110", "10001", "01110" },
        ['7'] = new List<string> { "11111", "00001", "00010", "00100", "01000" },
        ['8'] = new List<string> { "01110", "10001", "01110", "10001", "01110" },
        ['9'] = new List<string> { "01110", "10001", "01111", "00001", "01110" },

        [' '] = new List<string> { "00000", "00000", "00000", "00000", "00000" },
        ['.'] = new List<string> { "00000", "00000", "00000", "00000", "11000" },
        ['…'] = new List<string> { "00000", "00000", "00000", "00000", "10101" }
    };

    public static void EjecutarPrueba()
    {
        // Aquí puedes agregar el código de prueba que desees ejecutar.
        ImprimirCaracteres("Prueba ejecutada correctamentewwwwwwwwwwwwwwww.");
    }

    private static void ContarDijitos(int numero)
    {
        var copia = numero;
        var contador = 0;

        numero = Math.Abs(numero); // Asegurarse que sea positivo
        if (numero == 0) contador = 1;
        else
            // Usar un bucle while para contar los dígitos
            while (numero > 0)
            {
                numero = numero / 10; // División entera
                contador++;
            }

        Console.WriteLine($"El número {copia} tiene {contador} dígitos.");
    }


    public static void ImprimirCaracteres(string texto, int charMax = 20, char lleno = '\u2588',
        char vacio = '\u2591')
    {
        texto = texto.ToUpper();
        var palabras = texto.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var lineaActual = "";
        var lineas = new List<string>();

        foreach (var palabra in palabras)
        {
            var palabraFinal = palabra.Length > charMax ? palabra.Substring(0, charMax - 1) + "…" : palabra;

            if ((lineaActual + " " + palabraFinal).Trim().Length <= charMax)
            {
                lineaActual = (lineaActual + " " + palabraFinal).Trim();
            }
            else
            {
                lineas.Add(lineaActual.PadRight(charMax));
                lineaActual = palabraFinal;
            }
        }

        if (!string.IsNullOrWhiteSpace(lineaActual))
            lineas.Add(lineaActual.PadRight(charMax));

        const int alturaAscii = 5;
        const int anchoAscii = 5;
        var separador = 2;
        var anchoLinea = charMax * (anchoAscii + separador) + separador; // incluye bordes y separadores

        var sb = new StringBuilder();

        sb.AppendLine(new string('0', anchoLinea));

        foreach (var linea in lineas)
        {
            var listText = linea.Select(c =>
                    asciiBinaryFont.TryGetValue(c, out var val)
                        ? val
                        : new List<string> { "?????", "?????", "?????", "?????", "?????" })
                .ToList();

            for (var capa = 0; capa < alturaAscii; capa++)
            {
                var listCapa = new string('0', separador) +
                               string.Join(new string('0', separador), listText.Select(l => l[capa])) +
                               new string('0', separador);
                sb.AppendLine(listCapa);
            }

            sb.AppendLine(new string('0', anchoLinea));
        }

        Console.WriteLine(sb.ToString().Replace("1", lleno.ToString()).Replace("0", vacio.ToString()));
    }
    /// <summary>
    /// Valida numeros bancarios
    /// </summary>
    /// <param name="numero"></param>
    /// <returns></returns>
    public static bool LuhnValidator(string numero)
    {
        if (string.IsNullOrWhiteSpace(numero) || !numero.All(char.IsDigit))
            return false;

        int suma = 0;
        bool duplicar = false;

        // Recorrer de derecha a izquierda
        for (int i = numero.Length - 1; i >= 0; i--)
        {
            int digito = numero[i] - '0';

            if (duplicar)
            {
                digito *= 2;

                if (digito > 9)
                    digito -= 9;
            }

            suma += digito;
            duplicar = !duplicar;
        }

        var resul = (suma % 10) == 0;
        Console.WriteLine(resul);
        return resul;
    }

}