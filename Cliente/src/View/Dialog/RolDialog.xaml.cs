using System.Windows;
using System.Windows.Controls;
using Cliente.Extencions;
using Cliente.Obj.Model;
using CommunityToolkit.Mvvm.Input;
using Utilidades.Extencions;
using Utilidades.Factory;
using Utilidades.Interfaces;

namespace Cliente.View.Dialog;

/// <summary>
/// Lógica de interacción para UsuarioItemDetall.xaml
/// </summary>
public partial class RolDialog : UserControl, IDialog
{
    private IDialog _dialogImplementation;
    public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(nameof(Item), typeof(Rol), typeof(RolDialog));
    public static readonly DependencyProperty ListPermsProperty = DependencyProperty.Register(nameof(ListPerms), typeof(List<Nodos>), typeof(RolDialog));
    public static readonly DependencyProperty AceptedCommandProperty = DependencyProperty.Register(nameof(AceptedCommand), typeof(IAsyncRelayCommand), typeof(RolDialog));
    public static readonly DependencyProperty TextHeaderProperty = DependencyProperty.Register(nameof(TextHeader), typeof(string), typeof(RolDialog));

    public Rol Item
    {
        get => (Rol)GetValue(ItemProperty);
        set => SetValue(ItemProperty, value);
    }

    public List<Nodos> ListPerms
    {
        get => (List<Nodos>)GetValue(ListPermsProperty);
        set => SetValue(ListPermsProperty, value);
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

    public RolDialog()
    {
        InitializeComponent();
    }

    private async void ButtonGuardar(object sender, RoutedEventArgs e)
    {
        var list = ListPerms.ObtenerSeleccionados().DescostruirArbol();
        Item.Permisos = list;

        await AceptedCommand.TryEjecutarYCerrarDialogoAsync(this, Item);
    }

    private async void ButtonCancelar(object sender, RoutedEventArgs e)
    {
        await AceptedCommand.TryEjecutarYCerrarDialogoAsync(this);
    }


    public string DialogNameIdentifier { get; set; } = $"Dialog_{Guid.NewGuid():N}";
    public required string DialogOpenIdentifier { get; set; }
}