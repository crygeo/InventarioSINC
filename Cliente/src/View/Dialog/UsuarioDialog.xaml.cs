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
public partial class UsuarioDialog : UserControl, IDialog<Usuario>
{
    public static readonly DependencyProperty EntityProperty =
        DependencyProperty.Register(nameof(Entity), typeof(Usuario), typeof(UsuarioDialog));

    public static readonly DependencyProperty AceptedCommandProperty =
        DependencyProperty.Register(nameof(AceptarCommand), typeof(IAsyncRelayCommand), typeof(UsuarioDialog));

    public static readonly DependencyProperty TextHeaderProperty =
        DependencyProperty.Register(nameof(TextHeader), typeof(string), typeof(UsuarioDialog));

    public static readonly DependencyProperty CancelarCommandProperty =
        DependencyProperty.Register(nameof(CancelarCommand), typeof(IAsyncRelayCommand), typeof(AccountDialog),
            new PropertyMetadata(null));

    private string _dialogIdentifier;

    public UsuarioDialog()
    {
        InitializeComponent();
    }

    public IAsyncRelayCommand CancelarCommand
    {
        get => (IAsyncRelayCommand)GetValue(CancelarCommandProperty);
        set => SetValue(CancelarCommandProperty, value);
    }

    public Usuario Entity
    {
        get => (Usuario)GetValue(EntityProperty);
        set => SetValue(EntityProperty, value);
    }

    public IAsyncRelayCommand<Usuario> AceptarCommand
    {
        get => (IAsyncRelayCommand<Usuario>)GetValue(AceptedCommandProperty);
        set => SetValue(AceptedCommandProperty, value);
    }

    public string TextHeader
    {
        get => (string)GetValue(TextHeaderProperty);
        set => SetValue(TextHeaderProperty, value);
    }

    public string DialogNameIdentifier { get; set; } = $"Dialog_{Guid.NewGuid():N}";
    public required string DialogOpenIdentifier { get; set; }

    private async void ButtonCancelar(object sender, RoutedEventArgs e)
    {
        await CancelarCommand.TryEjecutarYCerrarDialogoAsync(this);
    }

    private async void ButtonAceptar(object sender, RoutedEventArgs e)
    {
        await AceptarCommand.TryEjecutarYCerrarDialogoAsync(this, Entity);
    }
}