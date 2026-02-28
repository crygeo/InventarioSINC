using System;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Input;

namespace Utilidades.Dialogs;

/// <summary>
///     Lógica de interacción para MessageDialog.xaml
/// </summary>
public partial class MessageDialog : UserControl, IDialog
{
    public static readonly DependencyProperty MessageProperty =
        DependencyProperty.Register(nameof(Message), typeof(string), typeof(MessageDialog));

    public static readonly DependencyProperty AceptarCommandProperty =
        DependencyProperty.Register(nameof(AceptarCommand), typeof(IAsyncRelayCommand), typeof(MessageDialog));

    public MessageDialog()
    {
        InitializeComponent();
    }

    public string Message
    {
        get => (string)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public IAsyncRelayCommand AceptarCommand
    {
        get => (IAsyncRelayCommand)GetValue(AceptarCommandProperty);
        set => SetValue(AceptarCommandProperty, value);
    }

    public string TextHeader { get; set; }

    public string DialogNameIdentifier { get; set; } = $"Dialog_{Guid.NewGuid():N}";
    public required string DialogOpenIdentifier { get; set; }
}