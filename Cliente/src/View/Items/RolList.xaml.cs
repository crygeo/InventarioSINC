using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cliente.View.Items;

/// <summary>
///     Lógica de interacción para RolList.xaml
/// </summary>
public partial class RolList : UserControl
{
    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(RolList));

    public static readonly DependencyProperty ItemProperty =
        DependencyProperty.Register(nameof(Item), typeof(object), typeof(RolList));

    public static readonly DependencyProperty EditarItemCommandProperty =
        DependencyProperty.Register(nameof(EditarItemCommand), typeof(ICommand), typeof(RolList));

    public static readonly DependencyProperty EliminarItemCommandProperty =
        DependencyProperty.Register(nameof(EliminarItemCommand), typeof(ICommand), typeof(RolList));

    public RolList()
    {
        InitializeComponent();
        //DataContext = this;
    }


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

    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public object Item
    {
        get => (object)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    private void ListBoxItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is ListBoxItem item && Keyboard.IsKeyDown(Key.LeftShift))
        {
            item.IsSelected = !item.IsSelected;
            e.Handled = true; // Evita que WPF cambie la selección predeterminada
        }
    }
}