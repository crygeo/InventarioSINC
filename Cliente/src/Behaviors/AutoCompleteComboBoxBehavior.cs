using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.Xaml.Behaviors;

namespace Cliente.Behaviors;

/// <summary>
/// Behavior para ComboBox editable que actúa como AutoComplete.
/// Escucha el texto que escribe el usuario y dispara un comando/action
/// sin interferir con el SelectedItem ni el Text binding.
/// </summary>
public class AutoCompleteComboBoxBehavior : Behavior<ComboBox>
{
    // ================================
    // DP: Action que se ejecuta cuando el usuario escribe
    // ================================

    public static readonly DependencyProperty OnTextChangedProperty =
        DependencyProperty.Register(
            nameof(OnTextChanged),
            typeof(Action<string>),
            typeof(AutoCompleteComboBoxBehavior),
            new PropertyMetadata(null)
        );

    /// <summary>
    /// Acción que recibe el texto escrito por el usuario.
    /// El exterior decide qué hacer: filtrar local o llamar al servidor.
    /// </summary>
    public Action<string>? OnTextChanged
    {
        get => (Action<string>?)GetValue(OnTextChangedProperty);
        set => SetValue(OnTextChangedProperty, value);
    }

    // ================================
    // Ciclo de vida del Behavior
    // ================================

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.Loaded += OnLoaded;
        AssociatedObject.SelectionChanged += OnSelectionChanged;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.Loaded -= OnLoaded;
        AssociatedObject.SelectionChanged -= OnSelectionChanged;

        // Desuscribir del TextBox interno si ya fue cargado
        if (_textBox != null)
            _textBox.TextChanged -= OnInternalTextChanged;
    }

    // ================================
    // Acceso al TextBox interno del ComboBox editable
    // ================================

    private TextBox? _textBox;
    private bool _suprimirEvento;

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        // El TextBox interno del ComboBox editable se llama "PART_EditableTextBox"
        _textBox = AssociatedObject.Template.FindName("PART_EditableTextBox", AssociatedObject) as TextBox;

        if (_textBox != null)
            _textBox.TextChanged += OnInternalTextChanged;
    }

    private void OnInternalTextChanged(object sender, TextChangedEventArgs e)
    {
        if (_suprimirEvento) return;
        if (_textBox == null) return;

        var texto = _textBox.Text;

        // Abrir el dropdown mientras escribe
        if (!AssociatedObject.IsDropDownOpen && !string.IsNullOrWhiteSpace(texto))
            AssociatedObject.IsDropDownOpen = true;

        OnTextChanged?.Invoke(texto);
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (AssociatedObject.SelectedItem == null) return;

        _suprimirEvento = true;

        // Mostrar el texto del item seleccionado
        if (_textBox != null)
        {
            var item = AssociatedObject.SelectedItem;
            var displayPath = AssociatedObject.DisplayMemberPath;

            _textBox.Text = !string.IsNullOrEmpty(displayPath)
                ? item.GetType().GetProperty(displayPath)?.GetValue(item)?.ToString() ?? item.ToString()
                : item.ToString();
        }

        AssociatedObject.IsDropDownOpen = false;

        _suprimirEvento = false;
    }
}