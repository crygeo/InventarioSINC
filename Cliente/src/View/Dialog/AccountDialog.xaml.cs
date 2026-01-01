using System.Windows;
using System.Windows.Controls;
using Cliente.Extencions;
using Cliente.Obj.Model;
using CommunityToolkit.Mvvm.Input;
using Utilidades.Dialogs;

namespace Cliente.View.Dialog;

/// <summary>
///     Lógica de interacción para UsuarioItemDetall.xaml
/// </summary>
public partial class AccountDialog : UserControl, IDialog
{
    public static readonly DependencyProperty UsuarioProperty =
        DependencyProperty.Register(nameof(Usuario), typeof(Usuario), typeof(AccountDialog));

    public static readonly DependencyProperty AceptarCommandProperty =
        DependencyProperty.Register(nameof(AceptarCommand), typeof(IAsyncRelayCommand), typeof(AccountDialog));

    public static readonly DependencyProperty CloseSeccionCommandProperty =
        DependencyProperty.Register(nameof(CloseSeccionCommand), typeof(IAsyncRelayCommand), typeof(AccountDialog));

    public static readonly DependencyProperty ChangedPasswordCommandProperty =
        DependencyProperty.Register(nameof(ChangedPasswordCommand), typeof(IAsyncRelayCommand), typeof(AccountDialog));

    public static readonly DependencyProperty TextHeaderProperty =
        DependencyProperty.Register(nameof(TextHeader), typeof(string), typeof(AccountDialog));

    public static readonly DependencyProperty CancelarCommandProperty =
        DependencyProperty.Register(nameof(CancelarCommand), typeof(IAsyncRelayCommand), typeof(AccountDialog),
            new PropertyMetadata(null));


    public AccountDialog()
    {
        InitializeComponent();
    }

    public IAsyncRelayCommand CancelarCommand
    {
        get => (IAsyncRelayCommand)GetValue(CancelarCommandProperty);
        set => SetValue(CancelarCommandProperty, value);
    }

    public Usuario Usuario
    {
        get => (Usuario)GetValue(UsuarioProperty);
        set => SetValue(UsuarioProperty, value);
    }

    public IAsyncRelayCommand CloseSeccionCommand
    {
        get => (IAsyncRelayCommand)GetValue(CloseSeccionCommandProperty);
        set => SetValue(CloseSeccionCommandProperty, value);
    }

    public IAsyncRelayCommand ChangedPasswordCommand
    {
        get => (IAsyncRelayCommand)GetValue(ChangedPasswordCommandProperty);
        set => SetValue(ChangedPasswordCommandProperty, value);
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

    public required string DialogNameIdentifier { get; set; }
    public required string DialogOpenIdentifier { get; set; }

    private async void ButtonChangedPass_OnClick(object sender, RoutedEventArgs e)
    {
        await ChangedPasswordCommand.TryEjecutarAsync(Usuario);
    }

    private async void ButtonClose_OnClick(object sender, RoutedEventArgs e)
    {
        await CancelarCommand.TryEjecutarYCerrarDialogoAsync(this);
    }

    private async void ButtonLogOut_OnClick(object sender, RoutedEventArgs e)
    {
        await CloseSeccionCommand.TryEjecutarAsync(this);
    }
}