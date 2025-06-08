using System.Windows;
using System.Windows.Controls;
using Utilidades.Interfaces;

namespace Cliente.View.Dialog;

/// <summary>
/// Lógica de interacción para MessageDialog.xaml
/// </summary>
public partial class MessageDialog : UserControl, IDialog
{
    public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(nameof(Message), typeof(string), typeof(MessageDialog));

    public string Message
    {
        get => (string)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public MessageDialog()
    {
        InitializeComponent();
    }

    public string DialogNameIdentifier { get; set; } = $"Dialog_{Guid.NewGuid():N}";
    public required string DialogOpenIdentifier { get; set; }
}