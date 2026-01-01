using System.Collections;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Cliente.Converter;
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
        GenerateColumns();
    }

    private void GenerateColumns()
    {
        TrySetItemType();

        if (TypeItem == null)
            throw new Exception("TypeItem is null");

        var properties = TypeItem.GetProperties()
            .Select(p => new
            {
                Prop = p,
                Vista = p.GetCustomAttribute<VistaAttribute>(),
                Solicitar = p.GetCustomAttribute<SolicitarAttribute>()
            })
            .Where(x => x.Vista != null && x.Vista.Visible)
            .OrderBy(x => x.Vista.Orden)
            .ToList();

        DataGridSolicitados.Columns.Clear();

        foreach (var item in properties)
        {
            var prop = item.Prop;
            var vista = item.Vista;
            var solicitar = item.Solicitar;

            var displayName =
                vista?.Nombre ??
                solicitar?.Nombre ??
                prop.Name;

            var isEnumerable =
                typeof(IEnumerable).IsAssignableFrom(prop.PropertyType)
                && prop.PropertyType != typeof(string);

            var binding = isEnumerable
                ? new Binding($"{prop.Name}.Count")
                : new Binding(prop.Name);

            if (vista?.LookupType != null)
                DataGridSolicitados.Columns.Add(new DataGridTextColumn
                {
                    Header = displayName,
                    Binding = new Binding(prop.Name)
                    {
                        Converter = new LookupValueConverter(vista.LookupType)
                    }
                });
            else
                DataGridSolicitados.Columns.Add(new DataGridTextColumn
                {
                    Header = displayName,
                    Binding = binding
                });
        }
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