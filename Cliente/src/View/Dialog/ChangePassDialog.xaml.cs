using System.Windows;
using System.Windows.Controls;
using Cliente.Extencions;
using CommunityToolkit.Mvvm.Input;
using Utilidades.Dialogs;

namespace Cliente.View.Dialog;

/// <summary>
/// Lógica de interacción para MessageDialog.xaml
/// </summary>
public partial class ChangePassDialog : UserControl, IDialog
{
    public static readonly DependencyProperty AceptarCommandProperty = DependencyProperty.Register(nameof(AceptarCommand), typeof(IAsyncRelayCommand), typeof(ChangePassDialog));
    public static readonly DependencyProperty CancelCommandProperty = DependencyProperty.Register(nameof(CancelCommand), typeof(IAsyncRelayCommand), typeof(ChangePassDialog));
    public static readonly DependencyProperty OldPasswordProperty = DependencyProperty.Register(nameof(OldPassword), typeof(string), typeof(ChangePassDialog), new PropertyMetadata(""));
    public static readonly DependencyProperty NewPasswordProperty = DependencyProperty.Register(nameof(NewPassword), typeof(string), typeof(ChangePassDialog));
    public static readonly DependencyProperty ConfirmPasswordProperty = DependencyProperty.Register(nameof(ConfirmPassword), typeof(string), typeof(ChangePassDialog));
    public static readonly DependencyProperty OldPasswordRequiredProperty = DependencyProperty.Register(nameof(OldPasswordRequired), typeof(Visibility), typeof(ChangePassDialog));
    public static readonly DependencyProperty DialogNameIdentifierProperty = DependencyProperty.Register(nameof(DialogNameIdentifier), typeof(string), typeof(ChangePassDialog));
    private string _textHeader;


    public IAsyncRelayCommand CancelCommand
    {
        get => (IAsyncRelayCommand)GetValue(CancelCommandProperty);
        set => SetValue(CancelCommandProperty, value);
    }

    public IAsyncRelayCommand AceptarCommand
    {
        get => (IAsyncRelayCommand)GetValue(AceptarCommandProperty);
        set => SetValue(AceptarCommandProperty, value);
    }

    public string OldPassword
    {
        get => (string)GetValue(OldPasswordProperty);
        set => SetValue(OldPasswordProperty, value);
    }
    public string NewPassword
    {
        get => (string)GetValue(NewPasswordProperty);
        set => SetValue(NewPasswordProperty, value);
    }
    public string ConfirmPassword
    {
        get => (string)GetValue(ConfirmPasswordProperty);
        set => SetValue(ConfirmPasswordProperty, value);
    }
    public Visibility OldPasswordRequired
    {
        get => (Visibility)GetValue(OldPasswordRequiredProperty);
        set => SetValue(OldPasswordRequiredProperty, value);
    }

    public string TextHeader
    {
        get => _textHeader;
        set => _textHeader = value;
    }

    public string DialogNameIdentifier { get; set; } = $"Dialog_{Guid.NewGuid():N}";

    public required string DialogOpenIdentifier
    {
        get => (string)GetValue(DialogNameIdentifierProperty);
        set => SetValue(DialogNameIdentifierProperty, value);
    }

    public ChangePassDialog()
    {
        InitializeComponent();
    }

    private async void OnCancel(object sender, RoutedEventArgs e)
    {
        await CancelCommand.TryEjecutarYCerrarDialogoAsync(this);
    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
        if (NewPassword == ConfirmPassword && !string.IsNullOrEmpty(NewPassword))
            await AceptarCommand.TryEjecutarYCerrarDialogoAsync(this);
    }

}