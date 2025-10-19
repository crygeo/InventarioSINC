using System.Windows;
using System.Windows.Controls;
using Cliente.Extencions;
using Cliente.Obj.Model;
using CommunityToolkit.Mvvm.Input;
using Utilidades.Extencions;
using Utilidades.Factory;
using Utilidades.Interfaces;
using Utilidades.Dialogs;

namespace Cliente.View.Dialog;

/// <summary>
/// Lógica de interacción para UsuarioItemDetall.xaml
/// </summary>
public partial class RolDialog : UserControl, IDialog<Rol>
{
    private IDialog _dialogImplementation;
    public static readonly DependencyProperty EntityProperty = DependencyProperty.Register(nameof(Entity), typeof(Rol), typeof(RolDialog));
    public static readonly DependencyProperty ListPermsProperty = DependencyProperty.Register(nameof(ListPerms), typeof(List<Nodos>), typeof(RolDialog));
    public static readonly DependencyProperty AceptedCommandProperty = DependencyProperty.Register(nameof(AceptarCommand), typeof(IAsyncRelayCommand), typeof(RolDialog));
    public static readonly DependencyProperty TextHeaderProperty = DependencyProperty.Register(nameof(TextHeader), typeof(string), typeof(RolDialog));

    public Rol Entity
    {
        get => (Rol)GetValue(EntityProperty);
        set => SetValue(EntityProperty, value);
    }

    public List<Nodos> ListPerms
    {
        get => (List<Nodos>)GetValue(ListPermsProperty);
        set => SetValue(ListPermsProperty, value);
    }

    public IAsyncRelayCommand<Rol> AceptarCommand
    {
        get => (IAsyncRelayCommand<Rol>)GetValue(AceptedCommandProperty);
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
        Entity.Permisos = list;

        await AceptarCommand.TryEjecutarYCerrarDialogoAsync(this, Entity);
    }

    private async void ButtonCancelar(object sender, RoutedEventArgs e)
    {
        await AceptarCommand.TryEjecutarYCerrarDialogoAsync(this);
    }


    public string DialogNameIdentifier { get; set; } = $"Dialog_{Guid.NewGuid():N}";
    public required string DialogOpenIdentifier { get; set; }
}