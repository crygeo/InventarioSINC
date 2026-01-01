using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Cliente.View.Model;
using Cliente.ViewModel.Model;
using Utilidades.Mvvm;

namespace Cliente.Helpers;

public class ViewModelToViewTemplateSelector : DataTemplateSelector
{
    public override DataTemplate? SelectTemplate(object item, DependencyObject container)
    {
        if (item is not ViewModelBase vm)
            return base.SelectTemplate(item, container);

        // SubViewModelBase con lógica de vista padre
        if (vm is SubViewModelBase subVM) return TryFindTemplate($"{subVM.ID}Template");
        // Agrega más condiciones aquí si lo necesitas
        // ViewModelServiceBase<T> con inferencia por tipo
        var entityType = GetEntityTypeFromViewModel(vm);

        if (entityType != null)
        {
            var template = TryFindTemplate($"ViewTemplate_{entityType.Name}");
            if (template != null)
                return template;
        }

        // ✅ Fallback genérico: PageBaseV
        Debug.WriteLine($"🟠 Usando vista genérica (PageBaseV) para: {vm.GetType().Name}");
        return new DataTemplate
        {
            VisualTree = new FrameworkElementFactory(typeof(PageBaseV))
        };
    }

    private DataTemplate? TryFindTemplate(string key)
    {
        if (Application.Current.Resources[key] is DataTemplate template)
            return template;

        // Aquí podrías loguear si usas algún sistema de logs
        Debug.WriteLine($"⚠ No se encontró un DataTemplate con clave '{key}'");
        return null;
    }

    private Type? GetEntityTypeFromViewModel(object viewModel)
    {
        var currentType = viewModel.GetType();

        while (currentType != null)
        {
            if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == typeof(ViewModelServiceBase<>))
                return currentType.GetGenericArguments().FirstOrDefault();

            currentType = currentType.BaseType;
        }

        return null;
    }

    private DataTemplate CreateErrorTemplate(string typeName)
    {
        var factory = new FrameworkElementFactory(typeof(Border));
        factory.SetValue(Border.BackgroundProperty, Brushes.LightCoral);
        factory.SetValue(Border.PaddingProperty, new Thickness(10));

        var textFactory = new FrameworkElementFactory(typeof(TextBlock));
        textFactory.SetValue(TextBlock.TextProperty, $"❌ No se encontró DataTemplate para: {typeName}");
        textFactory.SetValue(TextBlock.ForegroundProperty, Brushes.White);
        textFactory.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);

        factory.AppendChild(textFactory);

        return new DataTemplate { VisualTree = factory };
    }
}