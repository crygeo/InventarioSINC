using Cliente.src.Attributes;
using Cliente.src.Model;
using Cliente.src.Services;
using Cliente.src.View.Items;
using MaterialDesignThemes.Wpf;
using Shared.Interfaces.ModelsBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Utilidades.Controls;
using Utilidades.Converters;

namespace Cliente.src.Extencions
{
    public static class ComponetesHelp
    {

        private static readonly Dictionary<Type, Func<object, string, string, FrameworkElement>> _constructores = new();

        public static FrameworkElement CrearComponente(object objetoModelo, string nombrePropiedad, string hint)
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


        public static AtributesAdd CrearAtributesAdd(object objetoModelo, string nombrePropiedad, string hint)
        {
            var atradd = new AtributesAdd
            {
                DataContext = objetoModelo,
                Margin = new Thickness(0, 0, 0, 10),

            };
            HintAssist.SetHint(atradd, hint);

            var binding = new Binding(nombrePropiedad)
            {
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                ValidatesOnNotifyDataErrors = true
            };

            atradd.SetBinding(AtributesAdd.ItemsSourceProperty, binding);

            return atradd;
        }
        public static VariantesAdd CrearVariantesAdd(object objetoModelo, string nombrePropiedad, string hint)
        {
            var atradd = new VariantesAdd()
            {
                DataContext = objetoModelo,
                Margin = new Thickness(0, 0, 0, 10),
                TypeItem = typeof(Variantes)

            };

            var binding = new Binding(nombrePropiedad)
            {
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                ValidatesOnNotifyDataErrors = true
            };

            atradd.SetBinding(VariantesAdd.ItemsSourceProperty, binding);

            return atradd;
        }
        public static TextBox CrearTextBoxConEstilo(object dataContext, string propiedad, string hint)
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

            HintAssist.SetHint(txt, hint);

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
        public static IdentificadoresSelect CrearIdentificadorSelect(object dataContext, string nombrePropiedad, string hint)
        {
            var identificadorSelect = new IdentificadoresSelect
            {
                DataContext = dataContext,
                Margin = new Thickness(5, 0, 5, 10),
                MinWidth = 200,
            };

            return identificadorSelect;
        }

        public static DatePicker CrearDatePickerConEstilo(object dataContext, string propiedad, string hint)
        {
            var datePicker = new DatePicker
            {
                DataContext = dataContext,
                Margin = new Thickness(5, 0, 5, 10),
                Style = (Style)Application.Current.FindResource("MaterialDesignFloatingHintDatePicker"),
                MinWidth = 150
            };

            HintAssist.SetHint(datePicker, hint);

            var binding = new Binding(propiedad)
            {
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                ValidatesOnNotifyDataErrors = true
            };

            datePicker.SetBinding(DatePicker.SelectedDateProperty, binding);

            return datePicker;
        }


        public static void Register(Type itemType, Func<object, string, string, FrameworkElement> factory)
        {
            _constructores[itemType] = factory;
        }
        public static bool TryCreate(Type? itemType, object modelo, string nombreProp, string hint, out FrameworkElement? control)
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

            var atributo = typeof(T).GetCustomAttribute<NombreEntidadAttribute>();

            switch (pluralidad)
            {
                case Pluralidad.Plural:
                    nombreEntidad = atributo?.Plural ?? $"{typeof(T).Name.ToLower()}s";
                    break;
                case Pluralidad.Singular:
                    nombreEntidad = atributo?.Singular ?? typeof(T).Name.ToLower();
                    break;

            }

            return nombreEntidad;
        }

        public static void RegistrarComponentesFormulario()
        {
            Register(typeof(AtributesAdd), CrearAtributesAdd);
            Register(typeof(TextBox), CrearTextBoxConEstilo);
            Register(typeof(VariantesAdd), CrearVariantesAdd);
            Register(typeof(DatePicker), CrearDatePickerConEstilo);
            Register(typeof(IdentificadoresSelect), CrearIdentificadorSelect);

        }
    }
    public enum Pluralidad
    {
        Singular,
        Plural
    }
}
