using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace Cliente.Behaviors;

public class AutoCompleteComboBoxBehavior : Behavior<ComboBox>
{
    // =========================================================
    // Dependency Property: TextChangedCommand
    // =========================================================

    public static readonly DependencyProperty TextChangedCommandProperty =
        DependencyProperty.Register(
            nameof(TextChangedCommand),
            typeof(ICommand),
            typeof(AutoCompleteComboBoxBehavior),
            new PropertyMetadata(null)
        );

    public ICommand? TextChangedCommand
    {
        get => (ICommand?)GetValue(TextChangedCommandProperty);
        set => SetValue(TextChangedCommandProperty, value);
    }

    // =========================================================
    // Fields
    // =========================================================

    private TextBox? _textBox;
    private bool _suppressTextEvent;

    // =========================================================
    // Lifecycle
    // =========================================================

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

        if (_textBox != null)
            _textBox.TextChanged -= OnInternalTextChanged;
    }

    // =========================================================
    // Internal Setup
    // =========================================================

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        // IMPORTANTE: Desactivar TextSearch interno
        AssociatedObject.IsTextSearchEnabled = false;

        _textBox = AssociatedObject.Template
            .FindName("PART_EditableTextBox", AssociatedObject) as TextBox;

        if (_textBox != null)
            _textBox.TextChanged += OnInternalTextChanged;
    }

    // =========================================================
    // Text Handling
    // =========================================================

    private void OnInternalTextChanged(object sender, TextChangedEventArgs e)
    {
        if (_suppressTextEvent) return;
        if (_textBox == null) return;

        var texto = _textBox.Text;

        if (!string.IsNullOrWhiteSpace(texto))
        {
            AssociatedObject.Dispatcher.BeginInvoke(() =>
            {
                if (!AssociatedObject.IsDropDownOpen)
                    AssociatedObject.IsDropDownOpen = true;

                // 🔥 Restaurar caret al final sin seleccionar texto
                _textBox.CaretIndex = _textBox.Text.Length;
                _textBox.SelectionLength = 0;
            });
        }

        if (TextChangedCommand?.CanExecute(texto) == true)
            TextChangedCommand.Execute(texto);
    }

    // =========================================================
    // Selection Handling
    // =========================================================

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (AssociatedObject.SelectedItem == null) return;
        if (_textBox == null) return;

        _suppressTextEvent = true;

        // Dejar que el ComboBox maneje el DisplayMemberPath
        // Solo cerramos el dropdown
        AssociatedObject.IsDropDownOpen = false;

        _suppressTextEvent = false;
    }
}