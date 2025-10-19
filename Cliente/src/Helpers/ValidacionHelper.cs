using Cliente.Attributes;
using Cliente.Obj.Model;
using Cliente.Services.Model;
using Cliente.View.Items;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Controls;
using Utilidades.Controls;

namespace Cliente.Helpers;

/// <summary>
/// Contiene utilidades de validación para modelos con atributos personalizados [Solicitar].
/// Valida campos automáticamente según sus reglas definidas en el atributo.
/// Compatible con formularios dinámicos, controles reutilizables y MVVM.
/// </summary>
public static class ValidacionHelper
{
    #region Validación General

    /// <summary>
    /// Valida todas las propiedades marcadas con [Solicitar] de un objeto.
    /// </summary>
    /// <param name="objeto">Instancia del modelo a validar.</param>
    /// <returns>Lista de mensajes de error encontrados.</returns>
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

    /// <summary>
    /// Valida una sola propiedad específica con el atributo [Solicitar].
    /// </summary>
    /// <param name="objeto">Instancia del modelo.</param>
    /// <param name="propiedadNombre">Nombre de la propiedad.</param>
    /// <returns>Lista de errores si los hay.</returns>
    public static List<string> ValidarPropiedadAtributo(this object objeto, string propiedadNombre)
    {
        var propiedad = objeto.GetType().GetProperty(propiedadNombre);
        if (propiedad == null) return new List<string>();

        return ValidarPropiedad(objeto, propiedad);
    }

    /// <summary>
    /// Método central que determina la validación a aplicar según el tipo de control (ItemType).
    /// </summary>
    private static List<string> ValidarPropiedad(object objeto, PropertyInfo propiedad)
    {
        var errores = new List<string>();

        var atributo = propiedad.GetCustomAttribute<SolicitarAttribute>();
        if (atributo == null) return errores;

        var valor = propiedad.GetValue(objeto);
        var nombreCampo = atributo.Nombre ?? propiedad.Name;

        if (valor == null)
            throw new ArgumentNullException(nameof(valor), $"El valor de la propiedad {nombreCampo} no puede ser nulo.");

        // Enrutamiento por tipo de control (ItemType)
        var itemType = atributo.ItemType ?? typeof(TextBox);

        switch (itemType.Name)
        {
            case nameof(TextBox):
                errores.AddRange(ValidarTextBox(valor, atributo, nombreCampo));
                break;

            case nameof(IdentificadoresSelect):
                errores.AddRange(ValidarIdentificadoresSelect(valor, atributo, nombreCampo));
                break;

            case nameof(ProveedorSelect):
                errores.AddRange(ValidarProveedorSelect(valor, atributo, nombreCampo));
                break;

            default:
                errores.AddRange(ValidarPorDefecto(valor, atributo, nombreCampo));
                break;
        }

        return errores;
    }

    #region Validaciones por tipo de control

    /// <summary>
    /// Valida campos de entrada tipo TextBox.
    /// </summary>
    private static List<string> ValidarTextBox(object valor, SolicitarAttribute atributo, string nombreCampo)
    {
        var errores = new List<string>();
        var texto = valor as string ?? "";

        if (atributo.Requerido && string.IsNullOrWhiteSpace(texto))
            errores.Add($"{nombreCampo} es obligatorio.");

        if (atributo.MinLength > 0 && texto.Length < atributo.MinLength)
            errores.Add($"{nombreCampo} debe tener al menos {atributo.MinLength} caracteres.");

        errores.AddRange(ValidarPorTipoInput(texto, atributo.InputBoxConvert, nombreCampo));

        return errores;
    }

    /// <summary>
    /// Valida que se hayan seleccionado todos los identificadores definidos en el servicio.
    /// </summary>
    private static List<string> ValidarIdentificadoresSelect(object valor, SolicitarAttribute atributo, string nombreCampo)
    {
        var errores = new List<string>();
        var identificadoresService = ServiceFactory.GetService<Identificador>();
        int totalEsperado = identificadoresService?.Collection?.Count() ?? 0;

        if (atributo.Requerido)
        {
            if (valor is not IEnumerable<string> seleccionados || seleccionados.Count() != totalEsperado)
                errores.Add($"Debes seleccionar todos los identificadores requeridos para '{nombreCampo}'.");
        }

        return errores;
    }

    /// <summary>
    /// Valida que se haya seleccionado un proveedor.
    /// </summary>
    private static List<string> ValidarProveedorSelect(object valor, SolicitarAttribute atributo, string nombreCampo)
    {
        var errores = new List<string>();

        if (atributo.Requerido)
        {
            if (valor is not string idProveedor || string.IsNullOrWhiteSpace(idProveedor))
                errores.Add($"Debe seleccionar un {nombreCampo.ToLower()}.");
        }

        return errores;
    }

    /// <summary>
    /// Validación genérica por defecto si no se reconoce el tipo de control.
    /// </summary>
    private static List<string> ValidarPorDefecto(object valor, SolicitarAttribute atributo, string nombreCampo)
    {
        var errores = new List<string>();

        if (atributo.Requerido)
        {
            bool esValido = valor switch
            {
                null => false,
                string str => !string.IsNullOrWhiteSpace(str),
                IEnumerable<string> lista => lista.Any(),
                _ => true
            };

            if (!esValido)
                errores.Add($"{nombreCampo} es obligatorio.");
        }

        return errores;
    }

    /// <summary>
    /// Validaciones específicas según el tipo de entrada (InputBoxType).
    /// </summary>
    private static List<string> ValidarPorTipoInput(string valor, InputBoxType tipo, string campo)
    {
        var errores = new List<string>();

        switch (tipo)
        {
            case InputBoxType.Phone:
            case InputBoxType.Dni:
                if (valor.Length != 10)
                    errores.Add($"{campo} debe tener exactamente 10 dígitos.");
                break;

            case InputBoxType.Ruc:
                if (valor.Length != 13)
                    errores.Add($"{campo} debe tener exactamente 13 dígitos.");
                break;

                // Aquí puedes agregar validación para Email, Username, etc.
        }

        return errores;
    }

    #endregion

    #endregion

    #region Validación Especial: Proveedor

    /// <summary>
    /// Validación específica para entidades tipo Proveedor,
    /// diferenciando entre persona natural y empresa.
    /// </summary>
    public static List<string> ValidarCamposSolicitados(this Proveedor objeto)
    {
        var errores = new List<string>();

        static List<string> Validar<T>(Proveedor obj, Expression<Func<Proveedor, T>> propExpr)
        {
            var prop = (propExpr.Body as MemberExpression)?.Member as PropertyInfo
                       ?? throw new Exception("Expresión inválida");
            return ValidacionHelper.ValidarPropiedad(obj, prop);
        }

        errores.AddRange(Validar(objeto, x => x.RUC));

        if (!objeto.EsEmpresa)
        {
            errores.AddRange(Validar(objeto, x => x.Cedula));
            errores.AddRange(Validar(objeto, x => x.PrimerNombre));
            errores.AddRange(Validar(objeto, x => x.PrimerApellido));
            errores.AddRange(Validar(objeto, x => x.Celular));
        }
        else
        {
            errores.AddRange(Validar(objeto, x => x.RazonSocial));
            errores.AddRange(Validar(objeto, x => x.RepresentanteLegal));
        }

        return errores;
    }

    #endregion
}
