using System.Collections;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Cliente.Attributes;
using Cliente.Converter;
using Cliente.Obj.Model;
using Cliente.Services.Model;
using Cliente.View.Items;
using MaterialDesignThemes.Wpf;
using Shared.Interfaces;
using Shared.Interfaces.Model;
using Utilidades.Attributes;
using Utilidades.Controls;
using Utilidades.Converters;
using Utilidades.Dialogs;
using Utilidades.Interfaces;
using Application = System.Windows.Application;

namespace Cliente.Helpers;

public static class ComponetesHelp
{
    private static readonly Dictionary<Type, Func<object, string, string?, Type?, FrameworkElement>> _constructores = new();

    private static readonly Dictionary<Type, Type[]> CompatibilidadTipos = new()
    {
        { typeof(TextBox), new[] { typeof(string), typeof(int), typeof(float) } },
        { typeof(CheckBox), new[] { typeof(bool), typeof(bool?) } },
        { typeof(DatePicker), new[] { typeof(DateTime), typeof(DateTime?) } },
        { typeof(IdentificadoresSelect), new[] { typeof(IEnumerable<string>) } },
        { typeof(AtributesAdd), new[] { typeof(IEnumerable) } },
        { typeof(TimePicker), new[] { typeof(TimeSpan) } },
        { typeof(EntitySelector), new[] { typeof(string) } },
        // Puedes agregar más...
    };


    public static void AgregarComponenteDinamico(this Panel panel, object objetoModelo, string nombrePropiedad)
    {
        var comp = CrearComponente(objetoModelo, nombrePropiedad);
        panel.Children.Add(comp);
    }

    #region Crear componente basado en el atributo [Solicitar]
    public static FrameworkElement CrearComponente(object objetoModelo, string nombrePropiedad, string? hint = null)
    {
        var tipo = objetoModelo.GetType();
        var propInfo = tipo.GetProperty(nombrePropiedad)
                       ?? throw new ArgumentException($"La propiedad '{nombrePropiedad}' no existe en {tipo.Name}");

        var solicitarAttr = propInfo.GetCustomAttribute<SolicitarAttribute>()
                            ?? throw new InvalidOperationException(
                                $"La propiedad '{nombrePropiedad}' no tiene el atributo [Solicitar].");
        hint ??= solicitarAttr.Nombre;
        var itemType = solicitarAttr.ItemType;
        var typeEntity = solicitarAttr.EntityType;

        ValidarCompatibilidad(itemType, propInfo.PropertyType, nombrePropiedad);

        if (TryCreate(itemType, objetoModelo, nombrePropiedad, hint, typeEntity, out var componente))
            return componente;

        throw new Exception(
            $"No se pudo crear el componente para '{nombrePropiedad}'. Comprueve si existe una creacion para {itemType.Name}");
    }
    
    public static FrameworkElement CrearAtributesAdd(object objetoModelo, string nombrePropiedad, string? hint = null, Type? typeEntity = null)
    {
        var atradd = new AtributesAdd
        {
            DataContext = objetoModelo,
            Margin = new Thickness(0, 0, 0, 10)
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

    public static FrameworkElement CrearTextBoxConEstilo(object dataContext, string propiedad, string? hint = null, Type? typeEntity = null)
    {
        var tipo = dataContext.GetType();
        var propInfo = tipo.GetProperty(propiedad);

        if (propInfo == null)
            throw new ArgumentException($"La propiedad '{propiedad}' no existe en {tipo.Name}");

        var attr = propInfo.GetCustomAttribute<SolicitarAttribute>()
                   ?? throw new InvalidOperationException(
                       $"La propiedad '{propiedad}' no tiene el atributo [Solicitar].");

        // Verificación: InputBoxConvert debe estar definido
        if (attr.InputBoxConvert == InputBoxType.None)
            throw new InvalidOperationException($"Debe especificar InputBoxConvert en la propiedad '{propiedad}' del modelo {tipo.Name}.");

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

    public static FrameworkElement CrearIdentificadorSelect(object dataContext, string nombrePropiedad, string? hint = null, Type? typeEntity = null)
    {
        var identificadorSelect = new IdentificadoresSelect
        {
            DataContext = dataContext,
            Margin = new Thickness(5, 0, 5, 10),
            MinWidth = 200
        };

        var binding = new Binding(nombrePropiedad)
        {
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            ValidatesOnNotifyDataErrors = true
        };

        identificadorSelect.SetBinding(IdentificadoresSelect.IdValoresProperty, binding);

        return identificadorSelect;
    }

    public static FrameworkElement CrearDatePickerConEstilo(object dataContext, string propiedad, string? hint = null, Type? typeEntity = null)
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
        var attr = propInfo.GetCustomAttribute<SolicitarAttribute>() ??
                   throw new InvalidOperationException($"La propiedad '{propiedad}' no tiene el atributo [Solicitar].");

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
    
    public static FrameworkElement CrearCheckBoxConEstilo(object dataContext, string propiedad, string? hint = null, Type? typeEntity = null)
    {
        var tipo = dataContext.GetType();
        var propInfo = tipo.GetProperty(propiedad);

        if (propInfo == null)
            throw new ArgumentException($"La propiedad '{propiedad}' no existe en {tipo.Name}");

        var componente = new CheckBox
        {
            DataContext = dataContext,
            Margin = new Thickness(5, 0, 5, 10),
            MinWidth = 200,
            Content = hint
        };

        // Puedes agregar estilos si tienes uno definido
        if (Application.Current.TryFindResource("MaterialDesignDarkCheckBox") is Style componenteStyle)
            componente.Style = componenteStyle;

        // Hint (solo si usas un control que lo soporte)
        if (!string.IsNullOrEmpty(hint))
            HintAssist.SetHint(componente, hint);

        var binding = new Binding(propiedad)
        {
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            ValidatesOnNotifyDataErrors = true
        };

        componente.SetBinding(CheckBox.IsCheckedProperty, binding);

        return componente;
    }

    public static FrameworkElement CrearTimePickerConEstilo(object dataContext, string propiedad, string? hint = null, Type? typeEntity = null)
    {
        var tipo = dataContext.GetType();
        var propInfo = tipo.GetProperty(propiedad);

        if (propInfo == null)
            throw new ArgumentException($"La propiedad '{propiedad}' no existe en {tipo.Name}");

        var componente = new TimePicker
        {
            DataContext = dataContext,
            Margin = new Thickness(5, 5, 5, 10),
            MinWidth = 200,
        };

        // Puedes agregar estilos si tienes uno definido
        if (Application.Current.TryFindResource("MaterialDesignFloatingHintTimePicker") is Style componenteStyle)
            componente.Style = componenteStyle;

        // Hint (solo si usas un control que lo soporte)
        if (!string.IsNullOrEmpty(hint))
            HintAssist.SetHint(componente, hint);

        var binding = new Binding(propiedad)
        {
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            ValidatesOnNotifyDataErrors = true,
            Converter = new TimeSpanToDateTimeConverter()
        };

        componente.SetBinding(TimePicker.SelectedTimeProperty, binding);

        return componente;
    }

    public static FrameworkElement CrearEntitySelector(object dataContext, string nombrePropiedad, string? hint = null, Type? typeEntity = null)
    {
        var service = (IServiceClient)ServiceFactory.GetService(typeEntity);
        var entitySelector = new EntitySelector()
        {
            DataContext = dataContext,
            Margin = new Thickness(5, 0, 5, 10),
            MinWidth = 200,
            ItemsSource = service.GetAllFromCacheObj(),

        };

        var binding = new Binding(nombrePropiedad)
        {
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            ValidatesOnNotifyDataErrors = true
        };

        entitySelector.SetBinding(EntitySelector.SelectedIdProperty, binding);

        return entitySelector;
    }
    #endregion

    private static void ValidarCompatibilidad(Type itemType, Type tipoPropiedad, string nombrePropiedad)
    {
        if (!CompatibilidadTipos.TryGetValue(itemType, out var tiposAceptados))
            return; // ya se validó en Register

        if (!tiposAceptados.Any(t => t.IsAssignableFrom(tipoPropiedad)))
            throw new InvalidOperationException(
                $"El control '{itemType.Name}' no es compatible con la propiedad '{nombrePropiedad}' ({tipoPropiedad.Name}). " +
                $"Tipos permitidos: {string.Join(", ", tiposAceptados.Select(t => t.Name))}");
    }

    public static void Register(Type itemType, Func<object, string, string?, Type?, FrameworkElement> factory)
    {
        _constructores[itemType] = factory;
    }

    public static bool TryCreate(Type? itemType, object modelo, string nombreProp, string? hint, Type? typeEntity, out FrameworkElement? control)
    {
        if (itemType != null && _constructores.TryGetValue(itemType, out var factory))
        {
            control = factory(modelo, nombreProp, hint, typeEntity);
            control.Margin = new Thickness(5);
            return true;
        }

        control = null;
        return false;
    }

    public static string GetNombreEntidad<T>(Pluralidad pluralidad)
    {
        var nombreEntidad = "";

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
        Register(typeof(DatePicker), CrearDatePickerConEstilo);
        Register(typeof(TimePicker), CrearTimePickerConEstilo);
        Register(typeof(IdentificadoresSelect), CrearIdentificadorSelect);
        Register(typeof(CheckBox), CrearCheckBoxConEstilo);
        Register(typeof(EntitySelector), CrearEntitySelector);
        // Register(typeof(VariantesAdd), CrearVariantesAdd);
        // Register(typeof(ProveedorSelect), CrearProveedorSelect);
        // Register(typeof(EmpleadoSelected), CrearEmpleadoSelected);
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
            throw new InvalidOperationException($"El tipo {tipoDialogo.Name} no implementa IDialogBase.");

        // Buscar constructor que acepte TEntity
        var constructor = tipoDialogo.GetConstructor(new[] { modelo });

        if (constructor != null) return Activator.CreateInstance(tipoDialogo, intanciaEntity) as IDialog<TEntity>;

        // Si no hay constructor con entidad, usar por defecto y asignar manualmente
        var instancia = Activator.CreateInstance(tipoDialogo) as IDialog<TEntity>;
        if (instancia != null) instancia.Entity = intanciaEntity;

        return instancia;
    }
}

public enum Pluralidad
{
    Singular,
    Plural
}