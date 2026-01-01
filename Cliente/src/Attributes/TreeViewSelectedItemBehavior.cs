
using System.Windows;
using System.Windows.Controls;

namespace Cliente.Attributes;
public static class TreeViewSelectedItemBehavior
{
    public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.RegisterAttached(
            "SelectedItem",
            typeof(object),
            typeof(TreeViewSelectedItemBehavior),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

    public static object GetSelectedItem(DependencyObject obj)
        => obj.GetValue(SelectedItemProperty);

    public static void SetSelectedItem(DependencyObject obj, object value)
        => obj.SetValue(SelectedItemProperty, value);

    public static readonly DependencyProperty EnableProperty =
        DependencyProperty.RegisterAttached(
            "Enable",
            typeof(bool),
            typeof(TreeViewSelectedItemBehavior),
            new PropertyMetadata(false, OnEnableChanged)
        );

    public static bool GetEnable(DependencyObject obj)
        => (bool)obj.GetValue(EnableProperty);

    public static void SetEnable(DependencyObject obj, bool value)
        => obj.SetValue(EnableProperty, value);

    private static void OnEnableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not TreeView tree)
            return;

        if ((bool)e.NewValue)
            tree.SelectedItemChanged += Tree_SelectedItemChanged;
        else
            tree.SelectedItemChanged -= Tree_SelectedItemChanged;
    }

    private static void Tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        SetSelectedItem((TreeView)sender, e.NewValue);
    }
}
