using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Utilidades.Controls;

/// <summary>
///     Lógica de interacción para TextBoxPlus.xaml
/// </summary>
public partial class PasswordBoxPlus : UserControl
{
    public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register(nameof(Password),
        typeof(string), typeof(PasswordBoxPlus), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty CharPasswordProperty =
        DependencyProperty.Register(nameof(CharPassword), typeof(char), typeof(PasswordBoxPlus),
            new PropertyMetadata('•'));

    public static readonly DependencyProperty IsPasswordVisibleProperty =
        DependencyProperty.Register(nameof(IsPasswordVisible), typeof(bool), typeof(PasswordBoxPlus),
            new PropertyMetadata(false));

    public static readonly DependencyProperty TextVerticalAlignmentProperty =
        DependencyProperty.Register(nameof(TextVerticalAlignment), typeof(VerticalAlignment),
            typeof(PasswordBoxPlus), new PropertyMetadata(VerticalAlignment.Center));

    public static readonly DependencyProperty TextHorizontalAlignmentProperty =
        DependencyProperty.Register(nameof(TextHorizontalAlignment), typeof(TextAlignment), typeof(PasswordBoxPlus),
            new PropertyMetadata(TextAlignment.Left));

    public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(nameof(Placeholder),
        typeof(string), typeof(PasswordBoxPlus), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty PlaceholderForegroundProperty =
        DependencyProperty.Register(nameof(PlaceholderForeground), typeof(SolidColorBrush), typeof(PasswordBoxPlus),
            new PropertyMetadata(Brushes.Black));

    public static readonly DependencyProperty PlaceholderFontSizeProperty =
        DependencyProperty.Register(nameof(PlaceholderFontSize), typeof(int), typeof(PasswordBoxPlus),
            new PropertyMetadata(12));

    public static readonly DependencyProperty PlaceholderFontWeightProperty =
        DependencyProperty.Register(nameof(PlaceholderFontWeight), typeof(FontWeight), typeof(PasswordBoxPlus),
            new PropertyMetadata(FontWeights.Normal));

    public static readonly DependencyProperty PlaceholderVerticalAlignmentProperty =
        DependencyProperty.Register(nameof(PlaceholderVerticalAlignment), typeof(VerticalAlignment),
            typeof(PasswordBoxPlus), new PropertyMetadata(VerticalAlignment.Center));

    public static readonly DependencyProperty PlaceholderHorizontalAlignmentProperty =
        DependencyProperty.Register(nameof(PlaceholderHorizontalAlignment), typeof(HorizontalAlignment),
            typeof(PasswordBoxPlus), new PropertyMetadata(HorizontalAlignment.Left));


    public static readonly DependencyProperty CaretBrushProperty = DependencyProperty.Register(nameof(CaretBrush),
        typeof(SolidColorBrush), typeof(PasswordBoxPlus), new PropertyMetadata(Brushes.Black));

    public static readonly DependencyProperty IsFocusedBoxProperty =
        DependencyProperty.Register(nameof(IsFocusedBox), typeof(bool), typeof(PasswordBoxPlus),
            new PropertyMetadata(false));

    private bool _isUpdatingText;


    public PasswordBoxPlus()
    {
        InitializeComponent();

        MouseDown += Focus_MouseDown;

        PasswordBox.PreviewTextInput += PasswordBox_PreviewTextInput;
        PasswordBox.TextChanged += PasswordBox_TextChanged;
        PasswordBox.PreviewKeyDown += PasswordBox_PreviewKeyDown;

        PasswordBox.GotFocus += (s, e) => IsFocusedBox = true;
        PasswordBox.LostFocus += (s, e) => IsFocusedBox = false;

        ToggleVisibilityButton.Click += ToggleVisibilityButton_Click;

        // Initialize visibility of placeholder
        UpdatePlaceholderVisibility();
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

    // Evento que captura cada tecla presionada antes de que aparezca en el TextBox
    private void PasswordBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        // Filtrar el salto de línea (no deseado)
        if (e.Text.Contains("\n") && !PasswordBox.AcceptsReturn)
        {
            e.Handled = true; // Evitar que se ingrese el salto de línea
            return;
        }

        var selectionStart = PasswordBox.SelectionStart;
        var selectionLength = PasswordBox.SelectionLength;

        if (selectionLength > 0)
            // Si hay texto seleccionado, lo eliminamos antes de insertar el nuevo carácter
            Password = Password.Remove(selectionStart, selectionLength);

        // Insertar el nuevo carácter en la posición actual del cursor
        Password = Password.Insert(selectionStart, e.Text);

        e.Handled = true; // Evita que el carácter se escriba directamente en el TextBox
        UpdateTextBoxDisplay();

        // Restaurar la posición del cursor después de actualizar el texto
        PasswordBox.CaretIndex = selectionStart + e.Text.Length;
    }


    private void PasswordBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Back || e.Key == Key.Delete)
        {
            var selectionStart = PasswordBox.SelectionStart;
            var selectionLength = PasswordBox.SelectionLength;

            if (selectionLength > 0)
            {
                // Elimina los caracteres seleccionados
                Password = Password.Remove(selectionStart, selectionLength);
            }
            else if (e.Key == Key.Back && selectionStart > 0)
            {
                // Si no hay selección, elimina el carácter anterior
                Password = Password.Remove(selectionStart - 1, 1);
                selectionStart--;
            }
            else if (e.Key == Key.Delete && selectionStart < Password.Length)
            {
                // Si no hay selección y se presiona Delete, elimina el carácter en la posición actual
                Password = Password.Remove(selectionStart, 1);
            }

            // Evita la acción predeterminada del TextBox
            e.Handled = true;

            // Actualiza la visualización
            UpdateTextBoxDisplay();
            PasswordBox.SelectionStart = selectionStart;
        }
    }


    private void UpdateTextBoxDisplay()
    {
        var IndexPB = PasswordBox.CaretIndex;

        _isUpdatingText = true;
        PasswordBox.Text = IsPasswordVisible ? Password : new string('•', Password.Length);
        _isUpdatingText = false;

        PasswordBox.CaretIndex = ++IndexPB;
    }

    private void PasswordBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!_isUpdatingText)
        {
            // Asegurar que la cantidad de * coincida con la longitud de la contraseña
            if (PasswordBox.Text.Length < Password.Length) Password = Password.Substring(0, PasswordBox.Text.Length);

            UpdateTextBoxDisplay();
        }

        UpdatePlaceholderVisibility();
    }

    private void Focus_MouseDown(object sender, MouseButtonEventArgs e)
    {
        PasswordBox.Focus();
    }


    private void UpdatePlaceholderVisibility()
    {
        if (string.IsNullOrEmpty(Password))
            PlaceholderText.Visibility = Visibility.Visible;
        else
            PlaceholderText.Visibility = Visibility.Hidden;
    }

    private void ToggleVisibilityButton_Click(object sender, RoutedEventArgs e)
    {
        IsPasswordVisible = !IsPasswordVisible; // Invierte la visibilidad
        var IndexPB = PasswordBox.CaretIndex;
        if (IsPasswordVisible)
            PasswordBox.Text = Password;
        else
            PasswordBox.Text = new string(CharPassword, PasswordBox.Text.Length);

        PasswordBox.CaretIndex = IndexPB;
    }


    #region Text

    private string SecretPassword { get; set; }

    public string Password
    {
        get => (string)GetValue(PasswordProperty);
        set => SetValue(PasswordProperty, value);
    }

    public bool IsPasswordVisible
    {
        get => (bool)GetValue(IsPasswordVisibleProperty);
        set => SetValue(IsPasswordVisibleProperty, value);
    }

    public char CharPassword
    {
        get => (char)GetValue(CharPasswordProperty);
        set => SetValue(CharPasswordProperty, value);
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