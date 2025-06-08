using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cliente.View.Items;

/// <summary>
/// Lógica de interacción para ItemItemB.xaml
/// </summary>
public partial class ObjectItem : UserControl
{
    // Propiedad de Dependencia para el Item
    public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(nameof(Item), typeof(object), typeof(ObjectItem));
    public static readonly DependencyProperty IsSelectProperty = DependencyProperty.Register(nameof(IsSelect), typeof(bool), typeof(ObjectItem), new PropertyMetadata(false));
    public static readonly DependencyProperty EditarItemCommandProperty = DependencyProperty.Register(nameof(EditarItemCommand), typeof(ICommand), typeof(ObjectItem));
    public static readonly DependencyProperty EliminarItemCommandProperty = DependencyProperty.Register(nameof(EliminarItemCommand), typeof(ICommand), typeof(ObjectItem));
    public object Item
    {
        get => (object)GetValue(ItemProperty);
        set => SetValue(ItemProperty, value);
    }

    public bool IsSelect
    {
        get => (bool)GetValue(IsSelectProperty);
        set => SetValue(IsSelectProperty, value);
    }

    // Comando de Selección

    //public ICommand SeleccionarItemCommand
    //{
    //    get => (ICommand)GetValue(SeleccionarItemCommandProperty);
    //    set => SetValue(SeleccionarItemCommandProperty, value);
    //}
    public ICommand EditarItemCommand
    {
        get => (ICommand)GetValue(EditarItemCommandProperty);
        set => SetValue(EditarItemCommandProperty, value);
    }
    public ICommand EliminarItemCommand
    {
        get => (ICommand)GetValue(EliminarItemCommandProperty);
        set => SetValue(EliminarItemCommandProperty, value);
    }

    public ObjectItem()
    {
        InitializeComponent();

    }
}