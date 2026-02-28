using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Input;

namespace Utilidades.Dialogs;

/// <summary>
///     Lógica de interacción para MessageDialog.xaml
/// </summary>
public partial class TestDialog : UserControl, IDialog
{
    public static readonly DependencyProperty TextHeaderProperty =
        DependencyProperty.Register(nameof(TextHeader), typeof(string), typeof(TestDialog));

    public static readonly DependencyProperty MessageProperty =
        DependencyProperty.Register(nameof(Message), typeof(Dictionary<byte, object>), typeof(TestDialog));

    public static readonly DependencyProperty AceptarCommandProperty =
        DependencyProperty.Register(nameof(AceptarCommand), typeof(IAsyncRelayCommand), typeof(TestDialog));

    public TestDialog()
    {
        InitializeComponent();
    }

    public Dictionary<byte, object> Message
    {
        get => (Dictionary<byte, object>)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public IAsyncRelayCommand AceptarCommand
    {
        get => (IAsyncRelayCommand)GetValue(AceptarCommandProperty);
        set => SetValue(AceptarCommandProperty, value);
    }

    public string TextHeader
    {
        get => (string)GetValue(TextHeaderProperty);
        set => SetValue(TextHeaderProperty, value);
    }

    public string DialogNameIdentifier { get; set; } = $"Dialog_{Guid.NewGuid():N}";
    public required string DialogOpenIdentifier { get; set; }
}