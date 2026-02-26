using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Cliente.Converter;
using Cliente.Extencions;
using Cliente.Helpers;
using Cliente.Obj.Model;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Shared.Interfaces.Model;
using Utilidades.Attributes;
using Utilidades.Dialogs;

namespace Cliente.View.Dialog;

public class FormularioDinamico<TEntity> : UserControl, IDialog<TEntity>, IDialogLifecycle
{
    public static readonly DependencyProperty TextHeaderProperty = DependencyProperty.Register(nameof(TextHeader),
        typeof(string), typeof(FormularioDinamico<TEntity>), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty CancelarCommandProperty =
        DependencyProperty.Register(nameof(CancelarCommand), typeof(IAsyncRelayCommand<ModelBase<IModelObj>>),
            typeof(FormularioDinamico<TEntity>), new PropertyMetadata(null));

    public static readonly DependencyProperty AceptarCommandProperty =
        DependencyProperty.Register(nameof(AceptarCommand), typeof(IAsyncRelayCommand),
            typeof(FormularioDinamico<TEntity>), new PropertyMetadata(null));

    public static readonly DependencyProperty DialogNameIdentifierProperty =
        DependencyProperty.Register(nameof(DialogNameIdentifier), typeof(string), typeof(FormularioDinamico<TEntity>),
            new PropertyMetadata(null));

    private readonly Dictionary<string, string> _nombreCampos;
    public WrapPanel FormTop { get; }
    public StackPanel FormBot { get; }


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
    private static readonly Type[] AllowedInputTypes =
    {
        typeof(TextBox),
        typeof(ComboBox),
        typeof(CheckBox),
        typeof(DatePicker),
        typeof(PasswordBox)
    };
   

    public FormularioDinamico(TEntity entity, Dictionary<string, string> nombreCampos = null)
    {
        _nombreCampos = nombreCampos ?? new Dictionary<string, string>();
        Entity = entity;

        MaxWidth = 650;
        MaxHeight = 700;

        // Root DialogHost
        var dialogHost = new DialogHost
        {
            Identifier = DialogNameIdentifier,
            
        };


        var grid = new Grid();
        grid.Focusable = false;
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
            Foreground = Brushes.White,
            Focusable = false
        };

        titulo.SetBinding(TextBlock.TextProperty, new Binding(nameof(TextHeader))
        {
            Source = this,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        });

        Grid.SetRow(titulo, 0);
        grid.Children.Add(titulo);

        // Formulario
        var formContainer = new StackPanel { Margin = new Thickness(15), Focusable = false};
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
        btnCerrar.IsCancel = true;
        var btnAceptar = new Button
        {
            Content = "Aceptar",
            Margin = new Thickness(10, 0, 10, 0)
        };
        btnAceptar.Click += OnAcepted;
        btnAceptar.IsDefault = true;

        botones.Children.Add(btnCerrar);
        botones.Children.Add(btnAceptar);
        Grid.SetRow(botones, 2);
        grid.Children.Add(botones);

        dialogHost.Content = grid;
        Content = dialogHost;

        GenerarCampos();
        
    }

    private async Task EjecutarAceptar()
    {
        if (Entity == null) return;

        var errores = Entity.ValidarCamposSolicitados();

        if (!errores.Any())
            await AceptarCommand.TryEjecutarYCerrarDialogoAsync(this, Entity);
        else
            foreach (var error in errores)
                DialogService.Instance.MensajeQueue.Enqueue(error);
    }

    private async Task EjecutarCancelar()
    {
        await CancelarCommand.TryEjecutarYCerrarDialogoAsync(this);
    }

    
    /*public void OnOpened()
    {
        Dispatcher.BeginInvoke(new Action(() =>
        {
            DebugFocusState(this);
        }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
    }*/
    
    public void OnOpened()
    {
        Dispatcher.BeginInvoke(new Action(() =>
        {
            var first = FindFirstInputControl(this);

            if (first != null)
            {
                Keyboard.ClearFocus();
                first.Focus();
                Keyboard.Focus(first);
            }

        }), DispatcherPriority.ApplicationIdle);
    }
    private Control? FindFirstInputControl(DependencyObject parent)
    {
        return GetAllChildren(parent)
            .OfType<Control>()
            .Where(c =>
                AllowedInputTypes.Contains(c.GetType()) &&
                c.IsEnabled &&
                c.IsVisible)
            .OrderBy(c => c.TabIndex)
            .FirstOrDefault();
    }
    
    /*private void DebugFocusState(DependencyObject parent)
    {
        var allControls = GetAllChildren(parent)
            .OfType<Control>()
            .ToList();

        foreach (var control in allControls)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Control: {control.GetType().Name} | " +
                $"Focusable: {control.Focusable} | " +
                $"IsEnabled: {control.IsEnabled} | " +
                $"IsVisible: {control.IsVisible} | " +
                $"IsKeyboardFocused: {control.IsKeyboardFocused}");
        }

        var firstFocusable = allControls
            .FirstOrDefault(c =>
                c.Focusable &&
                c.IsEnabled &&
                c.IsVisible);

        if (firstFocusable != null)
        {
            System.Diagnostics.Debug.WriteLine($"Intentando enfocar: {firstFocusable.GetType().Name}");

            firstFocusable.Focus();
            Keyboard.Focus(firstFocusable);

            System.Diagnostics.Debug.WriteLine(
                $"Después de enfocar -> IsKeyboardFocused: {firstFocusable.IsKeyboardFocused}");
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("No se encontró ningún control enfocable.");
        }
    }*/
    
    private IEnumerable<DependencyObject> GetAllChildren(DependencyObject parent)
    {
        if (parent == null)
            yield break;

        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);

            yield return child;

            foreach (var descendant in GetAllChildren(child))
                yield return descendant;
        }
    }

    public void OnClosed(){}

    private void GenerarCampos()
    {
        var propiedades = Entity.GetType().GetProperties()
            .Where(p => Attribute.IsDefined(p, typeof(SolicitarAttribute)));

        foreach (var propiedad in propiedades)
        {
            var atributo = propiedad.GetCustomAttribute<SolicitarAttribute>();
            var label = atributo?.Nombre ?? propiedad.Name;
            string? hint;
            _nombreCampos.TryGetValue(propiedad.Name, out hint);

            var componente = ComponetesHelp.CrearComponente(Entity, propiedad.Name, hint);
            var attr = propiedad.GetCustomAttribute<SolicitarAttribute>();

            if (!string.IsNullOrEmpty(attr?.VisibleWhen))
            {
                var visibilityBinding = new Binding(attr.VisibleWhen)
                {
                    Source = Entity,
                    Converter = new EqualityToVisibilityConverter(),
                    ConverterParameter = attr.VisibleWhenValue
                };

                componente.SetBinding(UIElement.VisibilityProperty, visibilityBinding);
            }
            
            if (componente is TextBox)
                FormTop.Children.Add(componente);
            else
                FormBot.Children.Add(componente);
        }
    }

    private async void OnClose(object sender, RoutedEventArgs e)
    {
        await CancelarCommand.TryEjecutarYCerrarDialogoAsync(this);
    }

    private async void OnAcepted(object sender, RoutedEventArgs e)
    {
        if (Entity == null) return;

        var errores = Entity.ValidarCamposSolicitados();

        if (!errores.Any())
            await AceptarCommand.TryEjecutarYCerrarDialogoAsync(this, Entity);
        else
            foreach (var error in errores)
                DialogService.Instance.MensajeQueue.Enqueue(error);

        //await DialogServiceI.Instance.MostrarDialogo(string.Join("\n", errores));
    }
}