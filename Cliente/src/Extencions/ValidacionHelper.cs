using Cliente.src.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
                var attr = prop.GetCustomAttribute<SolicitarAttribute>();
                if (attr == null) continue;

                var valor = prop.GetValue(objeto) as string;

                if (attr.Requerido)
                {
                    if (string.IsNullOrWhiteSpace(valor))
                    {
                        errores.Add($"El campo '{attr.Nombre}' es obligatorio.");
                        continue;
                    }

                    if (valor.Length < attr.MinLength)
                    {
                        errores.Add($"El campo '{attr.Nombre}' debe tener al menos {attr.MinLength} caracteres.");
                    }
                }
            }

            return errores;
        }

        public static List<string> ValidarPropiedadAtributo(this object objeto, string propiedadNombre)
        {
            var propiedad = objeto.GetType().GetProperty(propiedadNombre);
            if (propiedad == null || !Attribute.IsDefined(propiedad, typeof(SolicitarAttribute)))
                return new List<string>();

            var atributo = propiedad.GetCustomAttribute<SolicitarAttribute>();
            var valor = propiedad.GetValue(objeto);
            string nombreCampo = atributo?.Nombre ?? propiedad.Name;

            var errores = new List<string>();

            if (atributo?.Requerido == true)
            {
                if (valor == null || (valor is string str && string.IsNullOrWhiteSpace(str)))
                    errores.Add($"{nombreCampo} es obligatorio.");
            }

            if (atributo?.MinLength > 0 && valor is string texto && texto.Length < atributo.MinLength)
                errores.Add($"{nombreCampo} debe tener al menos {atributo.MinLength} caracteres.");

            return errores;
        }

    }

}
