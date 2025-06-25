    using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Cliente.Extencions;
using CommunityToolkit.Mvvm.Input;
using Utilidades.Interfaces;
using Utilidades.Interfaces.Dialogs;

namespace Cliente.View.Dialog;

/// <summary>
/// Lógica de interacción para MessageDialog.xaml
/// </summary>
public partial class ConfirmDialog : UserControl, IDialog
{
    public static readonly DependencyProperty AceptarCommandProperty = DependencyProperty.Register(nameof(AceptarCommand), typeof(IAsyncRelayCommand), typeof(ConfirmDialog));
    public static readonly DependencyProperty CancelarCommandProperty = DependencyProperty.Register(nameof(CancelarCommand), typeof(IAsyncRelayCommand), typeof(ConfirmDialog));
    public static readonly DependencyProperty TextHeaderProperty = DependencyProperty.Register(nameof(TextHeader), typeof(string), typeof(ConfirmDialog));
    public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(nameof(Message), typeof(string), typeof(ConfirmDialog));

    public IAsyncRelayCommand CancelarCommand
    {
        get => (IAsyncRelayCommand)GetValue(CancelarCommandProperty);
        set => SetValue(CancelarCommandProperty, value);
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
    public string Message
    {
        get => (string)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public string DialogNameIdentifier { get; set; } = $"Dialog_{Guid.NewGuid():N}";
    public required string DialogOpenIdentifier { get; set; }

    public ConfirmDialog()
    {
        InitializeComponent();
        this.PreviewKeyDown += ConfirmDialog_PreviewKeyDown;
    }

    private async void ConfirmDialog_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            e.Handled = true;
            await AceptarCommand.TryEjecutarYCerrarDialogoAsync(this);
        }
    }

    private async void OnCancel(object sender, RoutedEventArgs e)
    {
        await CancelarCommand.TryEjecutarYCerrarDialogoAsync(this);
    }

    private async void OnAcepted(object sender, RoutedEventArgs e)
    {
        await AceptarCommand.TryEjecutarYCerrarDialogoAsync(this);
    }
}