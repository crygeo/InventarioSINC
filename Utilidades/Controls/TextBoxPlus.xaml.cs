using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Utilidades.Controls;

/// <summary>
///     Lógica de interacción para TextBoxPlus.xaml
/// </summary>
public partial class TextBoxPlus : UserControl
{
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text),
        typeof(string), typeof(TextBoxPlus), new PropertyMetadata(string.Empty, OnTextChanged));

    public static readonly DependencyProperty TextVerticalAlignmentProperty =
        DependencyProperty.Register(nameof(TextVerticalAlignment), typeof(VerticalAlignment), typeof(TextBoxPlus),
            new PropertyMetadata(VerticalAlignment.Center));

    public static readonly DependencyProperty TextHorizontalAlignmentProperty =
        DependencyProperty.Register(nameof(TextHorizontalAlignment), typeof(TextAlignment), typeof(TextBoxPlus),
            new PropertyMetadata(TextAlignment.Left));

    public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(nameof(Placeholder),
        typeof(string), typeof(TextBoxPlus), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty PlaceholderForegroundProperty =
        DependencyProperty.Register(nameof(PlaceholderForeground), typeof(SolidColorBrush), typeof(TextBoxPlus),
            new PropertyMetadata(Brushes.Black));

    public static readonly DependencyProperty PlaceholderFontSizeProperty =
        DependencyProperty.Register(nameof(PlaceholderFontSize), typeof(int), typeof(TextBoxPlus),
            new PropertyMetadata(12));

    public static readonly DependencyProperty PlaceholderFontWeightProperty =
        DependencyProperty.Register(nameof(PlaceholderFontWeight), typeof(FontWeight), typeof(TextBoxPlus),
            new PropertyMetadata(FontWeights.Normal));

    public static readonly DependencyProperty PlaceholderVerticalAlignmentProperty =
        DependencyProperty.Register(nameof(PlaceholderVerticalAlignment), typeof(VerticalAlignment),
            typeof(TextBoxPlus), new PropertyMetadata(VerticalAlignment.Center));

    public static readonly DependencyProperty PlaceholderHorizontalAlignmentProperty =
        DependencyProperty.Register(nameof(PlaceholderHorizontalAlignment), typeof(HorizontalAlignment),
            typeof(TextBoxPlus), new PropertyMetadata(HorizontalAlignment.Left));

    public static readonly DependencyProperty PlaceholderVisibleProperty =
        DependencyProperty.Register(nameof(PlaceholderVisible), typeof(Visibility), typeof(TextBoxPlus),
            new PropertyMetadata(Visibility.Visible));


    public static readonly DependencyProperty CaretBrushProperty = DependencyProperty.Register(nameof(CaretBrush),
        typeof(SolidColorBrush), typeof(TextBoxPlus), new PropertyMetadata(Brushes.Black));

    public static readonly DependencyProperty IsFocusedBoxProperty =
        DependencyProperty.Register(nameof(IsFocusedBox), typeof(bool), typeof(TextBoxPlus),
            new PropertyMetadata(false));

    public TextBoxPlus()
    {
        InitializeComponent();
    }

    public SolidColorBrush CaretBrush
    {
        get => (SolidColorBrush)GetValue(CaretBrushProperty);
        set => SetValue(CaretBrushProperty, value);
    }

    public bool IsFocusedBox
    {
        get => (bool)GetValue(IsFocusedBoxProperty);
        set => SetValue(IsFocusedBoxProperty, value);
    }

    public Visibility PlaceholderVisible
    {
        get => (Visibility)GetValue(PlaceholderVisibleProperty);
        set => SetValue(PlaceholderVisibleProperty, value);
    }


    private void UpdatePlaceholderVisibility()
    {
        if (string.IsNullOrEmpty(Text))
            PlaceholderVisible = Visibility.Visible;
        else
            PlaceholderVisible = Visibility.Collapsed;
    }

    private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TextBoxPlus textBoxPlus) textBoxPlus.UpdatePlaceholderVisibility();
    }

    private void MainTextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        IsFocusedBox = true;
    }

    private void MainTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        IsFocusedBox = false;
    }


    #region Text

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public VerticalAlignment TextVerticalAlignment
    {
        get => (VerticalAlignment)GetValue(TextVerticalAlignmentProperty);
        set => SetValue(TextVerticalAlignmentProperty, value);
    }

    public TextAlignment TextHorizontalAlignment
    {
        get => (TextAlignment)GetValue(TextHorizontalAlignmentProperty);
        set => SetValue(TextHorizontalAlignmentProperty, value);
    }

    #endregion

    #region Placeholder

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public SolidColorBrush PlaceholderForeground
    {
        get => (SolidColorBrush)GetValue(PlaceholderForegroundProperty);
        set => SetValue(PlaceholderForegroundProperty, value);
    }

    public int PlaceholderFontSize
    {
        get => (int)GetValue(PlaceholderFontSizeProperty);
        set => SetValue(PlaceholderFontSizeProperty, value);
    }

    public FontWeight PlaceholderFontWeight
    {
        get => (FontWeight)GetValue(PlaceholderFontWeightProperty);
        set => SetValue(PlaceholderFontWeightProperty, value);
    }

    public VerticalAlignment PlaceholderVerticalAlignment
    {
        get => (VerticalAlignment)GetValue(PlaceholderVerticalAlignmentProperty);
        set => SetValue(PlaceholderVerticalAlignmentProperty, value);
    }

    public HorizontalAlignment PlaceholderHorizontalAlignment
    {
        get => (HorizontalAlignment)GetValue(PlaceholderHorizontalAlignmentProperty);
        set => SetValue(PlaceholderHorizontalAlignmentProperty, value);
    }

    #endregion
}