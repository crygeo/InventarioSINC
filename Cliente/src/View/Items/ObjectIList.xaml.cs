using System.Collections;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Cliente.Converter;
using Cliente.Helpers;
using CommunityToolkit.Mvvm.Input;
using Utilidades.Attributes;

namespace Cliente.View.Items;

/// <summary>
///     Lógica de interacción para ObjectIList.xaml
/// </summary>
public partial class ObjectIList : UserControl
{
    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(ObjectIList));

    public static readonly DependencyProperty ItemProperty =
        DependencyProperty.Register(nameof(Item), typeof(object), typeof(ObjectIList));

    public static readonly DependencyProperty TypeItemProperty =
        DependencyProperty.Register(nameof(TypeItem), typeof(Type), typeof(ObjectIList));

    public static readonly DependencyProperty EditarItemCommandProperty =
        DependencyProperty.Register(nameof(EditarItemCommand), typeof(IAsyncRelayCommand), typeof(ObjectIList));

    public static readonly DependencyProperty EliminarItemCommandProperty =
        DependencyProperty.Register(nameof(EliminarItemCommand), typeof(IAsyncRelayCommand), typeof(ObjectIList));

    public ObjectIList()
    {
        InitializeComponent();
        //DataContext = this;

        Loaded += ObjectIList_Loaded;
    }


    public IAsyncRelayCommand EditarItemCommand
    {
        get => (IAsyncRelayCommand)GetValue(EditarItemCommandProperty);
        set => SetValue(EditarItemCommandProperty, value);
    }

    public IAsyncRelayCommand EliminarItemCommand
    {
        get => (IAsyncRelayCommand)GetValue(EliminarItemCommandProperty);
        set => SetValue(EliminarItemCommandProperty, value);
    }

    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public object Item
    {
        get => (object)GetValue(ItemProperty);
        set => SetValue(ItemProperty, value);
    }

    public Type TypeItem
    {
        get => (Type)GetValue(TypeItemProperty);
        set => SetValue(TypeItemProperty, value);
    }

    private void ObjectIList_Loaded(object sender, RoutedEventArgs e)
    {
        DataGridColumnBuilder.BuildColumns(DataGridSolicitados, TypeItem);
    }

    

    private void TrySetItemType()
    {
        if (TypeItem != null)
            return;

        var item = ItemsSource?.Cast<object>()?.FirstOrDefault();
        if (item != null)
            TypeItem = item.GetType();
    }
}