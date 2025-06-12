using Cliente.Attributes;
using Cliente.Extencions;
using Cliente.Helpers;
using Cliente.Obj.Model;
using Cliente.Services;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Utilidades.Interfaces.Dialogs;
using BooleanToVisibilityConverter = Utilidades.Converters.BooleanToVisibilityConverter;

namespace Cliente.View.Dialog;


public partial class ProveedorDialog : UserControl, IDialog<Proveedor>

{
    //Dependency properties for dialog identifiers
    public static DependencyProperty DialogNameIdentifierProperty = DependencyProperty.Register(nameof(DialogNameIdentifier), typeof(string), typeof(ProveedorDialog), new PropertyMetadata(string.Empty));
    public static DependencyProperty DialogOpenIdentifierProperty = DependencyProperty.Register(nameof(DialogOpenIdentifier), typeof(string), typeof(ProveedorDialog), new PropertyMetadata(string.Empty));
    public static DependencyProperty AceptarCommandProperty = DependencyProperty.Register(nameof(AceptarCommand), typeof(IAsyncRelayCommand<Proveedor>), typeof(ProveedorDialog), new PropertyMetadata(null));
    public static DependencyProperty EntityProperty = DependencyProperty.Register(nameof(Entity), typeof(Proveedor), typeof(ProveedorDialog), new PropertyMetadata(null));
    public static DependencyProperty TextHeaderProperty = DependencyProperty.Register(nameof(TextHeader), typeof(string), typeof(ProveedorDialog), new PropertyMetadata(null));


    public string TextHeader
    {
        get => (string)GetValue(TextHeaderProperty);
        set => SetValue(TextHeaderProperty, value);
    }

    public string DialogNameIdentifier
    {
        get => (string)GetValue(DialogNameIdentifierProperty);
        set => SetValue(DialogNameIdentifierProperty, value);
    }

    public string DialogOpenIdentifier
    {
        get => (string)GetValue(DialogOpenIdentifierProperty);
        set => SetValue(DialogOpenIdentifierProperty, value);
    }

    public IAsyncRelayCommand<Proveedor> AceptarCommand
    {
        get => (IAsyncRelayCommand<Proveedor>)GetValue(AceptarCommandProperty);
        set => SetValue(AceptarCommandProperty, value);
    }

    public Proveedor Entity
    {
        get => (Proveedor)GetValue(EntityProperty);
        set => SetValue(EntityProperty, value);
    }
    public ProveedorDialog(Proveedor entity)
    {
        this.Entity = entity;

        InitializeComponent();

        GenerarCampos();
    }

    private void GenerarCampos()
    {
        // PanelEmpresa
        var panelEmpresa = new StackPanel();
        panelEmpresa.SetBinding(StackPanel.VisibilityProperty, new Binding("Entity.EsEmpresa")
        {
            RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(UserControl), 1),
            Converter = new BooleanToVisibilityConverter()
        });

        panelEmpresa.AgregarComponenteDinamico(Entity, nameof(Entity.RazonSocial));
        panelEmpresa.AgregarComponenteDinamico(Entity, nameof(Entity.RepresentanteLegal));

        // PanelPersona
        var panelPersona = new StackPanel();
        panelPersona.SetBinding(StackPanel.VisibilityProperty, new Binding("Entity.EsEmpresa")
        {
            RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(UserControl), 1),
            Converter = new  BooleanToVisibilityConverter(),
            ConverterParameter = "Invert"
        });

        panelPersona.AgregarComponenteDinamico(Entity, nameof(Entity.Cedula));
        panelPersona.AgregarComponenteDinamico(Entity, nameof(Entity.PrimerNombre));
        panelPersona.AgregarComponenteDinamico(Entity, nameof(Entity.SegundoNombre));
        panelPersona.AgregarComponenteDinamico(Entity, nameof(Entity.PrimerApellido));
        panelPersona.AgregarComponenteDinamico(Entity, nameof(Entity.SegundoApellido));
        panelPersona.AgregarComponenteDinamico(Entity, nameof(Entity.Celular));
        panelPersona.AgregarComponenteDinamico(Entity, nameof(Entity.FechaNacimiento));

        // Agregamos al PanelGeneral en orden
        PanelGeneral.AgregarComponenteDinamico(Entity, nameof(Entity.RUC));
        PanelGeneral.Children.Add(panelEmpresa);
        PanelGeneral.Children.Add(panelPersona);
        PanelGeneral.AgregarComponenteDinamico(Entity, nameof(Entity.Direccion));
    }


    private async void OnCancelarClick(object sender, RoutedEventArgs e)
    {
        await DialogService.Instance.CerrarSiEstaAbiertoYEsperar(this);
    }

    private async void OnAceptedClick(object sender, RoutedEventArgs e)
    {
        var errores = Entity?.ValidarCamposSolicitados();

        if (errores == null || !errores.Any())
            await AceptarCommand.TryEjecutarYCerrarDialogoAsync(this, Entity);
        else
            foreach (var error in errores)
            {
                DialogService.Instance.MensajeQueue.Enqueue(error);
            }
    }
}