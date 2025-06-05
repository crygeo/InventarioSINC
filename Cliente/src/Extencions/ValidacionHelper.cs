using Cliente.src.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utilidades.Controls;

namespace Cliente.src.Extencions
{
    public static class ValidacionHelper
    {
        public static List<string> ValidarCamposSolicitados(this object objeto)
        {
            var errores = new List<string>();
            var propiedades = objeto.GetType().GetProperties();

            foreach (var prop in propiedades)
            {
                errores.AddRange(ValidarPropiedad(objeto, prop));
            }

            return errores;
        }

        public static List<string> ValidarPropiedadAtributo(this object objeto, string propiedadNombre)
        {
            var propiedad = objeto.GetType().GetProperty(propiedadNombre);
            if (propiedad == null) return new List<string>();

            return ValidarPropiedad(objeto, propiedad);
        }

        // 🧩 MÉTODO CENTRAL COMÚN
        private static List<string> ValidarPropiedad(object objeto, PropertyInfo propiedad)
        {
            var errores = new List<string>();

            var atributo = propiedad.GetCustomAttribute<SolicitarAttribute>();
            if (atributo == null) return errores;

            var valor = propiedad.GetValue(objeto);
            var valorTexto = valor as string ?? string.Empty;
            string nombreCampo = atributo.Nombre ?? propiedad.Name;

            // Validación básica
            if (atributo.Requerido && string.IsNullOrWhiteSpace(valorTexto))
            {
                errores.Add($"{nombreCampo} es obligatorio.");
                return errores; // no validamos más si está vacío
            }

            if (atributo.MinLength > 0 && valorTexto.Length < atributo.MinLength)
            {
                errores.Add($"{nombreCampo} debe tener al menos {atributo.MinLength} caracteres.");
            }

            // Validaciones específicas según InputBoxConvert
            errores.AddRange(ValidarPorTipoInput(valorTexto, atributo.InputBoxConvert, nombreCampo));

            return errores;
        }

        // 📦 VALIDACIONES POR TIPO
        private static List<string> ValidarPorTipoInput(string valor, InputBoxType tipo, string campo)
        {
            var errores = new List<string>();

            switch (tipo)
            {
                case InputBoxType.Phone:
                    if (valor.Length != 10)
                        errores.Add($"{campo} debe tener exactamente 10 dígitos");
                    break;

                case InputBoxType.Dni:
                    if (valor.Length != 10)
                        errores.Add($"{campo} debe tener exactamente 10 dígitos.");
                    break;

                case InputBoxType.Ruc:
                    if (valor.Length != 13)
                        errores.Add($"{campo} debe tener exactamente 13 dígitos.");
                    break;

                    // Podés extender para Email, NickName, etc.
            }

            return errores;
        }
    }


}
