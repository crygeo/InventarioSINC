using Cliente.Attributes;
using Cliente.Converter;
using Cliente.Obj.Model;
using Cliente.Services.Model;
using Cliente.View.Items;
using MaterialDesignThemes.Wpf;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Utilidades.Controls;
using Utilidades.Converters;
using Utilidades.Dialogs;
using Utilidades.Interfaces;
using static System.Net.Mime.MediaTypeNames;
using Application = System.Windows.Application;

namespace Cliente.Helpers;

public static class ComponetesHelp
{

    private static readonly Dictionary<Type, Func<object, string, string?, FrameworkElement>> _constructores = new();

    public static FrameworkElement CrearComponente(object objetoModelo, string nombrePropiedad, string? hint = null)
    {
        var tipo = objetoModelo.GetType();
        var propInfo = tipo.GetProperty(nombrePropiedad)
                       ?? throw new ArgumentException($"La propiedad '{nombrePropiedad}' no existe en {tipo.Name}");

        var solicitarAttr = propInfo.GetCustomAttribute<SolicitarAttribute>()
                            ?? throw new InvalidOperationException($"La propiedad '{nombrePropiedad}' no tiene el atributo [Solicitar].");

        if (TryCreate(solicitarAttr.ItemType, objetoModelo, nombrePropiedad, hint, out var componente))
            return componente;

        throw new Exception($"No se pudo crear el componente para '{nombrePropiedad}'.");
    }

    public static void AgregarComponenteDinamico(this Panel Panel, object objetoModelo, string nombrePropiedad)
    {
        var comp = CrearComponente(objetoModelo, nombrePropiedad);
        Panel.Children.Add(comp);
    }


    public static AtributesAdd CrearAtributesAdd(object objetoModelo, string nombrePropiedad, string? hint = null)
    {
        var atradd = new AtributesAdd
        {
            DataContext = objetoModelo,
            Margin = new Thickness(0, 0, 0, 10),

        };

        var binding = new Binding(nombrePropiedad)
        {
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            ValidatesOnNotifyDataErrors = true
        };

        atradd.SetBinding(AtributesAdd.ItemsSourceProperty, binding);

        return atradd;
    }
    public static TextBox CrearTextBoxConEstilo(object dataContext, string propiedad, string? hint = null)
    {
        var tipo = dataContext.GetType();
        var propInfo = tipo.GetProperty(propiedad);

        if (propInfo == null)
            throw new ArgumentException($"La propiedad '{propiedad}' no existe en {tipo.Name}");

        var attr = propInfo.GetCustomAttribute<SolicitarAttribute>()
                   ?? throw new InvalidOperationException($"La propiedad '{propiedad}' no tiene el atributo [Solicitar].");

        // Verificación: InputBoxConvert debe estar definido
        if (attr.InputBoxConvert == InputBoxType.None)
            throw new InvalidOperationException($"Debe especificar InputBoxConvert en la propiedad '{propiedad}'.");

        var txt = new TextBox
        {
            DataContext = dataContext,
            Margin = new Thickness(5, 0, 5, 10),
            Style = (Style)Application.Current.FindResource("MaterialDesignFloatingHintTextBox"),
            MinWidth = 200
        };

        HintAssist.SetHint(txt, hint ?? attr.Nombre ?? propiedad);

        var binding = new Binding(propiedad)
        {
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            ValidatesOnNotifyDataErrors = true,
            Converter = new InputBoxConverter(),
            ConverterParameter = attr.InputBoxConvert.ToString()
        };

        txt.SetBinding(TextBox.TextProperty, binding);

        return txt;
    }
    public static IdentificadoresSelect CrearIdentificadorSelect(object dataContext, string nombrePropiedad, string? hint = null)
    {
        var identificadorSelect = new IdentificadoresSelect
        {
            DataContext = dataContext,
            Margin = new Thickness(5, 0, 5, 10),
            MinWidth = 200,
        };

        var binding = new Binding(nombrePropiedad)
        {
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            ValidatesOnNotifyDataErrors = true,
        };

        identificadorSelect.SetBinding(IdentificadoresSelect.IdValoresProperty, binding);

        return identificadorSelect;
    }
    public static DatePicker CrearDatePickerConEstilo(object dataContext, string propiedad, string? hint = null)
    {
        var datePicker = new DatePicker
        {
            DataContext = dataContext,
            Margin = new Thickness(5, 0, 5, 10),
            Style = (Style)Application.Current.FindResource("MaterialDesignFloatingHintDatePicker"),
            MinWidth = 150
        };

        var tipo = dataContext.GetType();
        var propInfo = tipo.GetProperty(propiedad);
        if (propInfo == null) throw new ArgumentException($"La propiedad '{propiedad}' no existe en {tipo.Name}");
        var attr = propInfo.GetCustomAttribute<SolicitarAttribute>() ?? throw new InvalidOperationException($"La propiedad '{propiedad}' no tiene el atributo [Solicitar].");

        HintAssist.SetHint(datePicker, attr.Nombre ?? propiedad);

        var binding = new Binding(propiedad)
        {
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            ValidatesOnNotifyDataErrors = true
        };

        datePicker.SetBinding(DatePicker.SelectedDateProperty, binding);

        return datePicker;
    }
    public static ProveedorSelect CrearProveedorSelect(object dataContext, string propiedad, string? hint = null)
    {
        var proveedorSelect = new ProveedorSelect
        {
            DataContext = dataContext,
            Margin = new Thickness(5, 0, 5, 10),
            MinWidth = 200,
        };

        var converter = new IdToProveedorConverter
        {
            ListaProveedores = ServiceFactory.GetService<Proveedor>().Collection
        };

        var binding = new Binding(propiedad)
        {
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            ValidatesOnNotifyDataErrors = true,
            Converter = converter

        };
        proveedorSelect.SetBinding(ProveedorSelect.ProveedorProperty, binding);

        return proveedorSelect;
    }

    public static void Register(Type itemType, Func<object, string, string?, FrameworkElement> factory)
    {
        _constructores[itemType] = factory;
    }
    public static bool TryCreate(Type? itemType, object modelo, string nombreProp, string? hint, out FrameworkElement? control)
    {
        if (itemType != null && _constructores.TryGetValue(itemType, out var factory))
        {
            control = factory(modelo, nombreProp, hint);
            control.Margin = new Thickness(5);
            return true;
        }

        control = null;
        return false;
    }

    public static string GetNombreEntidad<T>(Pluralidad pluralidad)
    {
        string nombreEntidad = "";

        var atributo = typeof(T).GetCustomAttribute<NavegacionAttribute>();

        switch (pluralidad)
        {
            case Pluralidad.Plural:
                nombreEntidad = atributo?.TituloP ?? $"{typeof(T).Name.ToLower()}s";
                break;
            case Pluralidad.Singular:
                nombreEntidad = atributo?.TituloS ?? typeof(T).Name.ToLower();
                break;

        }

        return nombreEntidad;
    }

    public static void RegistrarComponentesFormulario()
    {
        Register(typeof(AtributesAdd), CrearAtributesAdd);
        Register(typeof(TextBox), CrearTextBoxConEstilo);
        //Register(typeof(VariantesAdd), CrearVariantesAdd);
        Register(typeof(DatePicker), CrearDatePickerConEstilo);
        Register(typeof(IdentificadoresSelect), CrearIdentificadorSelect);
        Register(typeof(ProveedorSelect), CrearProveedorSelect);

    }

    public static IDialog<TEntity>? GetDialogoPersonalizado<TEntity>(TEntity intanciaEntity)
    {
        if (intanciaEntity == null)
            throw new ArgumentNullException(nameof(intanciaEntity));

        var modelo = typeof(TEntity);

        var attr = modelo.GetCustomAttribute<NavegacionAttribute>();
        if (attr?.DialogoPersonalizado == null)
            return null;

        var tipoDialogo = attr.DialogoPersonalizado;

        if (!typeof(IDialogBase).IsAssignableFrom(tipoDialogo))
        {
            throw new InvalidOperationException($"El tipo {tipoDialogo.Name} no implementa IDialogBase.");
        }

        // Buscar constructor que acepte TEntity
        var constructor = tipoDialogo.GetConstructor(new[] { modelo });

        if (constructor != null)
        {
            return Activator.CreateInstance(tipoDialogo, intanciaEntity) as IDialog<TEntity>;
        }

        // Si no hay constructor con entidad, usar por defecto y asignar manualmente
        var instancia = Activator.CreateInstance(tipoDialogo) as IDialog<TEntity>;
        if (instancia != null)
        {
            instancia.Entity = intanciaEntity;
        }

        return instancia;
    }




}
public enum Pluralidad
{
    Singular,
    Plural
}