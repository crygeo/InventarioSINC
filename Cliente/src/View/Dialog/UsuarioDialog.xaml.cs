using System.Windows;
using System.Windows.Controls;
using Cliente.Extencions;
using Cliente.Obj.Model;
using CommunityToolkit.Mvvm.Input;
using Utilidades.Interfaces;

namespace Cliente.View.Dialog;

/// <summary>
/// Lógica de interacción para UsuarioItemDetall.xaml
/// </summary>
public partial class UsuarioDialog : UserControl, IDialog
{
    public static readonly DependencyProperty UsuarioProperty = DependencyProperty.Register(nameof(Usuario), typeof(Usuario), typeof(UsuarioDialog));
    public static readonly DependencyProperty AceptedCommandProperty = DependencyProperty.Register(nameof(AceptedCommand), typeof(IAsyncRelayCommand), typeof(UsuarioDialog));
    public static readonly DependencyProperty TextHeaderProperty = DependencyProperty.Register(nameof(TextHeader), typeof(string), typeof(UsuarioDialog));
    public static readonly DependencyProperty CancelarCommandProperty = DependencyProperty.Register(nameof(CancelarCommand), typeof(IAsyncRelayCommand), typeof(AccountDialog), new PropertyMetadata(null));
    private string _dialogIdentifier;

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
    public IAsyncRelayCommand AceptedCommand
    {
        get => (IAsyncRelayCommand)GetValue(AceptedCommandProperty);
        set => SetValue(AceptedCommandProperty, value);
    }

    public string TextHeader
    {
        get => (string)GetValue(TextHeaderProperty);
        set => SetValue(TextHeaderProperty, value);
    }

    public UsuarioDialog()
    {
        InitializeComponent();
    }

    private async void ButtonCancelar(object sender, RoutedEventArgs e)
    {
        await CancelarCommand.TryEjecutarYCerrarDialogoAsync(this);
    }

    private async void ButtonAceptar(object sender, RoutedEventArgs e)
    {
        await AceptedCommand.TryEjecutarYCerrarDialogoAsync(this, Usuario);
    }

    public string DialogNameIdentifier { get; set; } = $"Dialog_{Guid.NewGuid():N}";
    public required string DialogOpenIdentifier { get; set; }
}