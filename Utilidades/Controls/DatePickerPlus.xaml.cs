using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Utilidades.Controls;

/// <summary>
///     Lógica de interacción para DatePickerPlus.xaml
/// </summary>
public partial class DatePickerPlus : UserControl
{
    public static readonly DependencyProperty FechaProperty = DependencyProperty.Register(nameof(Fecha),
        typeof(DateTime), typeof(DatePickerPlus), new PropertyMetadata(DateTime.Now, OnTextChanged));

    public static readonly DependencyProperty TextVerticalAlignmentProperty =
        DependencyProperty.Register(nameof(TextVerticalAlignment), typeof(VerticalAlignment),
            typeof(DatePickerPlus), new PropertyMetadata(VerticalAlignment.Center));

    public static readonly DependencyProperty TextHorizontalAlignmentProperty =
        DependencyProperty.Register(nameof(TextHorizontalAlignment), typeof(TextAlignment), typeof(DatePickerPlus),
            new PropertyMetadata(TextAlignment.Left));

    public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(nameof(Placeholder),
        typeof(string), typeof(DatePickerPlus), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty PlaceholderForegroundProperty =
        DependencyProperty.Register(nameof(PlaceholderForeground), typeof(SolidColorBrush), typeof(DatePickerPlus),
            new PropertyMetadata(Brushes.Black));

    public static readonly DependencyProperty PlaceholderFontSizeProperty =
        DependencyProperty.Register(nameof(PlaceholderFontSize), typeof(int), typeof(DatePickerPlus),
            new PropertyMetadata(12));

    public static readonly DependencyProperty PlaceholderFontWeightProperty =
        DependencyProperty.Register(nameof(PlaceholderFontWeight), typeof(FontWeight), typeof(DatePickerPlus),
            new PropertyMetadata(FontWeights.Normal));

    public static readonly DependencyProperty PlaceholderVerticalAlignmentProperty =
        DependencyProperty.Register(nameof(PlaceholderVerticalAlignment), typeof(VerticalAlignment),
            typeof(DatePickerPlus), new PropertyMetadata(VerticalAlignment.Center));

    public static readonly DependencyProperty PlaceholderHorizontalAlignmentProperty =
        DependencyProperty.Register(nameof(PlaceholderHorizontalAlignment), typeof(HorizontalAlignment),
            typeof(DatePickerPlus), new PropertyMetadata(HorizontalAlignment.Left));

    public static readonly DependencyProperty PlaceholderVisibleProperty =
        DependencyProperty.Register(nameof(PlaceholderVisible), typeof(Visibility), typeof(DatePickerPlus),
            new PropertyMetadata(Visibility.Visible));


    public static readonly DependencyProperty CaretBrushProperty = DependencyProperty.Register(nameof(CaretBrush),
        typeof(SolidColorBrush), typeof(DatePickerPlus), new PropertyMetadata(Brushes.Black));

    public static readonly DependencyProperty IsFocusedBoxProperty =
        DependencyProperty.Register(nameof(IsFocusedBox), typeof(bool), typeof(DatePickerPlus),
            new PropertyMetadata(false));

    public DatePickerPlus()
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
        if (Fecha == DateTime.MinValue || Fecha == new DateTime(1, 1, 1))
            PlaceholderVisible = Visibility.Visible;
        else
            PlaceholderVisible = Visibility.Collapsed;
    }

    private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (DatePickerPlus)d;

        // Evitar recursión infinita
        if (e.NewValue is DateTime dateTime)
        {
            var newText = dateTime == DateTime.MinValue || dateTime == new DateTime(1, 1, 1)
                ? "" // Si es la fecha mínima, mostrar vacío
                : dateTime.ToString("dd/MM/yyyy"); // Formato deseado

            // Verifica si el texto realmente cambió antes de asignarlo
            if (control.MainTextBox.Text != newText) control.MainTextBox.Text = newText;
        }

        // Llamar a la actualización del placeholder
        control.UpdatePlaceholderVisibility();
    }


    private void MainTextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        IsFocusedBox = true;
    }

    private void MainTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        IsFocusedBox = false;
        if (DateTime.TryParseExact(MainTextBox.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var fecha))
            Fecha = fecha; // Se actualiza la propiedad de dependencia
        else
            MainTextBox.Text = Fecha == DateTime.MinValue || Fecha == new DateTime(1, 1, 1)
                ? ""
                : Fecha.ToString("dd/MM/yyyy");
    }

    // Evento para agregar automáticamente las barras "/"
    private void Fecha_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var textBox = sender as TextBox;
        if (textBox == null || !char.IsDigit(e.Text[0]))
        {
            e.Handled = true; // Ignorar letras y caracteres no numéricos
            return;
        }

        // Obtener el texto actual sin los caracteres seleccionados
        var text = textBox.Text.Remove(textBox.SelectionStart, textBox.SelectionLength) + e.Text;

        // Filtrar solo los dígitos (por si ya hay caracteres no deseados)
        text = new string(text.Where(char.IsDigit).ToArray());

        // Limitar a 8 caracteres (ddMMyyyy)
        if (text.Length > 8)
        {
            e.Handled = true;
            return;
        }

        // Formatear la fecha manualmente como dd/MM/yyyy
        var formattedText = "";
        for (var i = 0; i < text.Length; i++)
        {
            if (i == 2 || i == 4) formattedText += "/"; // Insertar "/" después del día y mes
            formattedText += text[i];
        }

        // Evitar recursión infinita al actualizar el TextBox
        textBox.Text = formattedText;
        textBox.SelectionStart = textBox.Text.Length; // Mover cursor al final
        e.Handled = true;
    }


    #region Text

    public DateTime Fecha
    {
        get => (DateTime)GetValue(FechaProperty);
        set => SetValue(FechaProperty, value);
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