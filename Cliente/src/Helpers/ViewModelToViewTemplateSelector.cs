using System.Windows;
using System.Windows.Controls;
using Cliente.src.ViewModel.Model;
using Cliente.View;
using Utilidades.Mvvm;

namespace Cliente.Helpers;

public class ViewModelToViewTemplateSelector : DataTemplateSelector
{
    public override DataTemplate? SelectTemplate(object item, DependencyObject container)
    {
        if (item is not ViewModelBase vm)
            return base.SelectTemplate(item, container);

        var entityType = GetEntityTypeFromViewModel(vm);


        if (entityType == null)
            return base.SelectTemplate(item, container);

        // Buscar un recurso DataTemplate con una clave basada en el tipo de entidad
        var resourceKey = $"ViewTemplate_{entityType.Name}";

        if (Application.Current.Resources[resourceKey] is DataTemplate template)
            return template;

        return new DataTemplate
        {
            VisualTree = new FrameworkElementFactory(typeof(PageBaseV))
        };
    }

    private Type? GetEntityTypeFromViewModel(object viewModel)
    {
        var currentType = viewModel.GetType();

        while (currentType != null)
        {
            if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == typeof(ViewModelServiceBase<>))
            {
                return currentType.GetGenericArguments().FirstOrDefault();
            }

            currentType = currentType.BaseType;
        }

        return null;
    }

}