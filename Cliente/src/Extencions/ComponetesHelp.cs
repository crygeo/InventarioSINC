using Cliente.src.Attributes;
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
using Cliente.src.Model;

namespace Cliente.src.Extencions
{
    public static class ComponetesHelp
    {

        private static readonly Dictionary<Type, Func<object, string, string, FrameworkElement>> _constructores = new();

        public static FrameworkElement CrearComponente(object objetoModelo, string nombrePropiedad, string hint)
        {
            var tipo = objetoModelo.GetType();
            var propInfo = tipo.GetProperty(nombrePropiedad);

            if (propInfo == null)
                throw new ArgumentException($"La propiedad '{nombrePropiedad}' no existe en {tipo.Name}");

            var solicitarAttr = propInfo.GetCustomAttribute<SolicitarAttribute>();

            if (solicitarAttr?.ItemType == null)
                throw new Exception($"La propiedad '{nombrePropiedad}' no tiene definido ItemType en el atributo [Solicitar].");

            if (TryCreate(solicitarAttr.ItemType, objetoModelo, nombrePropiedad, hint, out var componente))
                return componente;

            throw new Exception($"No se ha registrado ningún generador de componente para el tipo {solicitarAttr.ItemType.Name}.");
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
            HintAssist.SetHint(atradd, hint);

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
                ValidatesOnNotifyDataErrors = true
            };

            txt.SetBinding(TextBox.TextProperty, binding);

            return txt;
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
        }
    }
    public enum Pluralidad
    {
        Singular,
        Plural
    }
}
