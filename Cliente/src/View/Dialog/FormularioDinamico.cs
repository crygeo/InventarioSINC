using Cliente.Attributes;
using Cliente.Extencions;
using Cliente.Helpers;
using Cliente.Obj.Model;
using Cliente.Services;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Shared.Interfaces.Model;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Utilidades.Interfaces.Dialogs;

namespace Cliente.View.Dialog;

public class FormularioDinamico<TEntity> : UserControl, IDialog<TEntity>
{

    public static readonly DependencyProperty TextHeaderProperty = DependencyProperty.Register(nameof(TextHeader), typeof(string), typeof(FormularioDinamico<TEntity>), new PropertyMetadata(string.Empty));
    public static readonly DependencyProperty CancelarCommandProperty = DependencyProperty.Register(nameof(CancelarCommand), typeof(IAsyncRelayCommand<ModelBase<IModelObj>>), typeof(FormularioDinamico<TEntity>), new PropertyMetadata(null));
    public static readonly DependencyProperty AceptarCommandProperty = DependencyProperty.Register(nameof(AceptarCommand), typeof(IAsyncRelayCommand), typeof(FormularioDinamico<TEntity>), new PropertyMetadata(null));
    public static readonly DependencyProperty DialogNameIdentifierProperty = DependencyProperty.Register(nameof(DialogNameIdentifier), typeof(string), typeof(FormularioDinamico<TEntity>), new PropertyMetadata(null));



    public TEntity Entity { get; set; }
    public string TextHeader
    {
        get => (string)GetValue(TextHeaderProperty);
        set => SetValue(TextHeaderProperty, value);
    }
    public IAsyncRelayCommand<TEntity> AceptarCommand
    {
        get => (IAsyncRelayCommand<TEntity>)GetValue(AceptarCommandProperty);
        set => SetValue(AceptarCommandProperty, value);
    }
    public IAsyncRelayCommand CancelarCommand
    {
        get => (IAsyncRelayCommand)GetValue(CancelarCommandProperty);
        set => SetValue(CancelarCommandProperty, value);
    }
    public string DialogNameIdentifier
    {
        get => (string)GetValue(DialogNameIdentifierProperty);
        set => SetValue(DialogNameIdentifierProperty, value);
    }
    public required string DialogOpenIdentifier { get; set; }

    public WrapPanel FormTop { get; private set; }
    public StackPanel FormBot { get; private set; }
    public FormularioDinamico(TEntity entity)
    {
        this.Entity = entity;

        this.MaxWidth = 650;
        this.MaxHeight = 700;

        // Root DialogHost
        var dialogHost = new DialogHost
        {
            Identifier = DialogNameIdentifier
        };

        var grid = new Grid();
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });

        // Título
        var titulo = new TextBlock
        {
            Text = TextHeader ?? typeof(TEntity).Name,
            HorizontalAlignment = HorizontalAlignment.Center,
            TextWrapping = TextWrapping.WrapWithOverflow,
            VerticalAlignment = VerticalAlignment.Bottom,
            FontSize = 16,
            FontWeight = FontWeights.Bold,
            Foreground = Brushes.White
        };

        titulo.SetBinding(TextBlock.TextProperty, new Binding(nameof(TextHeader))
        {
            Source = this,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
        });

        Grid.SetRow(titulo, 0);
        grid.Children.Add(titulo);

        // Formulario
        var formContainer = new StackPanel { Margin = new Thickness(15) };
        FormTop = new WrapPanel();
        FormBot = new StackPanel();

        formContainer.Children.Add(FormTop);
        formContainer.Children.Add(FormBot);
        Grid.SetRow(formContainer, 1);
        grid.Children.Add(formContainer);

        // Botones
        var botones = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(10)
        };

        var btnCerrar = new Button
        {
            Content = "Cerrar",
            Margin = new Thickness(10, 0, 10, 0)
        };
        btnCerrar.Click += OnClose;

        var btnAceptar = new Button
        {
            Content = "Aceptar",
            Margin = new Thickness(10, 0, 10, 0)
        };
        btnAceptar.Click +=  OnAcepted;

        botones.Children.Add(btnCerrar);
        botones.Children.Add(btnAceptar);
        Grid.SetRow(botones, 2);
        grid.Children.Add(botones);

        dialogHost.Content = grid;
        this.Content = dialogHost;

        GenerarCampos();
    }



    private void GenerarCampos()
    {
        var propiedades = Entity.GetType().GetProperties()
            .Where(p => Attribute.IsDefined(p, typeof(SolicitarAttribute)));

        foreach (var propiedad in propiedades)
        {
            var atributo = propiedad.GetCustomAttribute<SolicitarAttribute>();
            string label = atributo?.Nombre ?? propiedad.Name;

            var componente = ComponetesHelp.CrearComponente(Entity, propiedad.Name);

            if (componente is TextBox)
            {
                FormTop.Children.Add(componente);
            }
            else
            {
                FormBot.Children.Add(componente);
            }
        }
    }
    private async void OnClose(object sender, RoutedEventArgs e)
    {
        await CancelarCommand.TryEjecutarYCerrarDialogoAsync(this);
    }
    private async void OnAcepted(object sender, RoutedEventArgs e)
    {
        var errores = Entity.ValidarCamposSolicitados();

        if (!errores.Any())
            await AceptarCommand.TryEjecutarYCerrarDialogoAsync(this, Entity);
        else
            foreach (var error in errores)
            {
                DialogService.Instance.MensajeQueue.Enqueue(error);
            }

        //await DialogServiceI.Instance.MostrarDialogo(string.Join("\n", errores));
    }
}