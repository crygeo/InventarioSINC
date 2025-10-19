using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente
{
    public static class PruebaTemp
    {
        public static void EjecutarPrueba()
        {
            // Aquí puedes agregar el código de prueba que desees ejecutar.
            ImprimirCaracteres("Prueba ejecutada correctamentewwwwwwwwwwwwwwww.");
        }

        private static void ContarDijitos(int numero)
        {
            int copia = numero;
            int contador = 0;

            numero = Math.Abs(numero); // Asegurarse que sea positivo
            if (numero == 0) contador = 1;
            else
            {
                // Usar un bucle while para contar los dígitos
                while (numero > 0)
                {
                    numero = numero / 10; // División entera
                    contador++;
                }
            }

            Console.WriteLine($"El número {copia} tiene {contador} dígitos.");
        }


        public static void ImprimirCaracteres(string texto, int charMax = 20, char lleno = '\u2588', char vacio = '\u2591')
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
            int separador = 2;
            int anchoLinea = charMax * (anchoAscii + separador) + separador; // incluye bordes y separadores

            var sb = new System.Text.StringBuilder();

            sb.AppendLine(new string('0', anchoLinea));

            foreach (var linea in lineas)
            {
                var listText = linea.Select(c =>
                        asciiBinaryFont.TryGetValue(c, out var val)
                            ? val
                            : new List<string> { "?????", "?????", "?????", "?????", "?????" })
                    .ToList();

                for (int capa = 0; capa < alturaAscii; capa++)
                {
                    var listCapa = new string('0', separador) + string.Join(new string('0', separador), listText.Select(l => l[capa])) + new string('0', separador);
                    sb.AppendLine(listCapa);
                }

                sb.AppendLine(new string('0', anchoLinea));
            }

            Console.WriteLine(sb.ToString().Replace("1", lleno.ToString()).Replace("0", vacio.ToString()));
        }



        private static Dictionary<char, List<string>> asciiBinaryFont = new Dictionary<char, List<string>>
        {
            ['A'] = new() { "01110", "10001", "11111", "10001", "10001" },
            ['B'] = new() { "11110", "10001", "11110", "10001", "11110" },
            ['C'] = new() { "01111", "10000", "10000", "10000", "01111" },
            ['D'] = new() { "11110", "10001", "10001", "10001", "11110" },
            ['E'] = new() { "11111", "10000", "11110", "10000", "11111" },
            ['F'] = new() { "11111", "10000", "11110", "10000", "10000" },
            ['G'] = new() { "01111", "10000", "10011", "10001", "01110" },
            ['H'] = new() { "10001", "10001", "11111", "10001", "10001" },
            ['I'] = new() { "11111", "00100", "00100", "00100", "11111" },
            ['J'] = new() { "00111", "00001", "00001", "10001", "01110" },
            ['K'] = new() { "10001", "10010", "11100", "10010", "10001" },
            ['L'] = new() { "10000", "10000", "10000", "10000", "11111" },
            ['M'] = new() { "10001", "11011", "10101", "10001", "10001" },
            ['N'] = new() { "10001", "11001", "10101", "10011", "10001" },
            ['O'] = new() { "01110", "10001", "10001", "10001", "01110" },
            ['P'] = new() { "11110", "10001", "11110", "10000", "10000" },
            ['Q'] = new() { "01110", "10001", "10001", "10101", "01101" },
            ['R'] = new() { "11110", "10001", "11110", "10010", "10001" },
            ['S'] = new() { "01111", "10000", "01110", "00001", "11110" },
            ['T'] = new() { "11111", "00100", "00100", "00100", "00100" },
            ['U'] = new() { "10001", "10001", "10001", "10001", "01110" },
            ['V'] = new() { "10001", "10001", "10001", "01010", "00100" },
            ['W'] = new() { "10001", "10001", "10101", "10101", "01010" },
            ['X'] = new() { "10001", "01010", "00100", "01010", "10001" },
            ['Y'] = new() { "10001", "01010", "00100", "00100", "00100" },
            ['Z'] = new() { "11111", "00010", "00100", "01000", "11111" },

            ['0'] = new() { "01110", "10001", "10011", "10101", "01110" },
            ['1'] = new() { "00100", "01100", "00100", "00100", "11111" },
            ['2'] = new() { "01110", "00001", "00010", "00100", "11111" },
            ['3'] = new() { "01110", "00001", "00110", "00001", "01110" },
            ['4'] = new() { "10010", "10010", "11111", "00010", "00010" },
            ['5'] = new() { "11111", "10000", "11110", "00001", "11110" },
            ['6'] = new() { "01110", "10000", "11110", "10001", "01110" },
            ['7'] = new() { "11111", "00001", "00010", "00100", "01000" },
            ['8'] = new() { "01110", "10001", "01110", "10001", "01110" },
            ['9'] = new() { "01110", "10001", "01111", "00001", "01110" },

            [' '] = new() { "00000", "00000", "00000", "00000", "00000" },
            ['.'] = new() { "00000", "00000", "00000", "00000", "11000" },
            ['…'] = new() { "00000", "00000", "00000", "00000", "10101" },
        };
    }
}
