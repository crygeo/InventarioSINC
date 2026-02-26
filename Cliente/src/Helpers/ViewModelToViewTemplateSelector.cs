using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Cliente.View.Model;
using Cliente.ViewModel.Model;
using Utilidades.Attributes;
using Utilidades.Mvvm;

namespace Cliente.Helpers;


public class ViewModelToViewTemplateSelector : DataTemplateSelector
{
    public override DataTemplate? SelectTemplate(object item, DependencyObject container)
    {
        if (item is not ViewModelBase vm)
            return base.SelectTemplate(item, container);

        var viewType = GetViewTypeFromViewModel(vm);

        if (viewType == null)
            viewType = typeof(PageBaseV);

        return CreateTemplate(viewType);
    }

    private Type? GetViewTypeFromViewModel(ViewModelBase vm)
    {
        var entityType = GetEntityTypeFromViewModel(vm);
        if (entityType == null)
            return null;

        var attr = entityType
            .GetCustomAttributes(typeof(AutoViewModelAttribute), true)
            .FirstOrDefault() as AutoViewModelAttribute;

        return attr?.ViewType;
    }


    private Type? GetEntityTypeFromViewModel(ViewModelBase viewModel)
    {
        var currentType = viewModel.GetType();

        while (currentType != null)
        {
            if (currentType.IsGenericType &&
                currentType.GetGenericTypeDefinition() == typeof(ViewModelServiceBase<>))
            {
                return currentType.GetGenericArguments().FirstOrDefault();
            }

            currentType = currentType.BaseType;
        }

        return null;
    }


    private DataTemplate CreateTemplate(Type viewType)
    {
        var factory = new FrameworkElementFactory(viewType);
        return new DataTemplate { VisualTree = factory };
    }
}
