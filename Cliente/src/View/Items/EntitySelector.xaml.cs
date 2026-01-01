using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Shared.Interfaces;

namespace Cliente.View.Items;

public partial class EntitySelector : UserControl
{
    public EntitySelector()
    {
        InitializeComponent();
    }

    #region ItemsSource

    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(
            nameof(ItemsSource),
            typeof(IEnumerable),
            typeof(EntitySelector),
            new PropertyMetadata(null, OnItemsSourceChanged)
        );

    private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (EntitySelector)d;

        if (e.NewValue == null)
            return;

        var view = CollectionViewSource.GetDefaultView(e.NewValue);
        view.Filter = control.FilterPredicate;

        control.View = view;
    }

    #endregion

    #region View

    public ICollectionView? View
    {
        get => (ICollectionView?)GetValue(ViewProperty);
        private set => SetValue(ViewProperty, value);
    }

    public static readonly DependencyProperty ViewProperty =
        DependencyProperty.Register(
            nameof(View),
            typeof(ICollectionView),
            typeof(EntitySelector)
        );

    #endregion

    #region SelectedItem

    public object? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.Register(
            nameof(SelectedItem),
            typeof(object),
            typeof(EntitySelector),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
            )
        );

    #endregion

    #region SelectedId

    public string? SelectedId
    {
        get => (string?)GetValue(SelectedIdProperty);
        set => SetValue(SelectedIdProperty, value);
    }

    public static readonly DependencyProperty SelectedIdProperty =
        DependencyProperty.Register(
            nameof(SelectedId),
            typeof(string),
            typeof(EntitySelector),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnSelectedIdChanged
            )
        );

    private static void OnSelectedIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (EntitySelector)d;

        if (control.View == null || e.NewValue is not string id)
            return;

        control.SelectedItem = control.View
            .Cast<IIdentifiable>()
            .FirstOrDefault(x => x.Id == id);
    }

    #endregion

    #region Filter

    public string FilterText
    {
        get => (string)GetValue(FilterTextProperty);
        set => SetValue(FilterTextProperty, value);
    }

    public static readonly DependencyProperty FilterTextProperty =
        DependencyProperty.Register(
            nameof(FilterText),
            typeof(string),
            typeof(EntitySelector),
            new PropertyMetadata(string.Empty, OnFilterChanged)
        );

    private static void OnFilterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (EntitySelector)d;
        control.View?.Refresh();
    }

    public Predicate<object> FilterPredicate { get; set; } = _ => true;

    #endregion
}
